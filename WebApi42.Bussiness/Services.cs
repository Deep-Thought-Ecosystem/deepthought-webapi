using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebApi42.DAO;
using WebApi42.DTO;

namespace WebApi42.Bussiness
{


    public class AuthService(UserDBCOntext context, IConfiguration configuration) : IAuthService
    {

        public async Task<User> RegisterAsync(UserRegisterAuthDTO userDTO)
        {


            if (await context.Users.AnyAsync(e => e.Email == userDTO.Email))
            {
                throw new InvalidDataException("Email already registered");
            }
            var user1 = new User
            {
                Name = "na",
                UserName = "na",
                Email = userDTO.Email,
                Role = userDTO.Role
            };

            user1.PasswordHashed = new PasswordHasher<User>().HashPassword(user1, userDTO.Password);

            var obj = await context.Users.AddAsync(user1);

            await context.SaveChangesAsync();

            return user1;

        }

        public async Task<TokenResponseDTO> LoginAsync(UserLoginAuthDTO id)
        {
            var user = await context.Users.FirstOrDefaultAsync(e => e.Email == id.Email);
            if (user == null)
            {
                throw new InvalidDataException("User does not exist");
            }

            var passwordHashed = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHashed, id.Password);

            if (passwordHashed == PasswordVerificationResult.Failed)
            {
                throw new InvalidDataException("Password does not match");
            }
            return await CreateRefreshToken(user);
        }
        static (string? HostBridgeIp, string? HostBridgeMac) GetDockerHostBridgeInfo()
        {
            // 1) Get the default gateway IPv4 (host's bridge address from inside the container)
            string? gatewayIp = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up &&
                            n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties().GatewayAddresses)
                .Select(g => g?.Address)
                .FirstOrDefault(a => a is not null && a.AddressFamily == AddressFamily.InterNetwork &&
                                     !IPAddress.IsLoopback(a))?
                .ToString();

            // 2) Best-effort: read the ARP table for that IP to get the MAC
            string? mac = null;
            if (!string.IsNullOrWhiteSpace(gatewayIp) && File.Exists("/proc/net/arp"))
            {
                // Touch the neighbor so ARP is populated (won't error if it fails)
                try
                {
                    using var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    s.Blocking = false;
                    s.ConnectAsync(gatewayIp, 1).Wait(TimeSpan.FromMilliseconds(50));
                }
                catch { /* ignore */ }

                foreach (var line in File.ReadLines("/proc/net/arp").Skip(1))
                {
                    var parts = Regex.Split(line.Trim(), @"\s+");
                    if (parts.Length >= 4 && parts[0] == gatewayIp && parts[3] != "00:00:00:00:00:00")
                    {
                        mac = parts[3];
                        break;
                    }
                }
            }

            return (gatewayIp, mac);
        }
        private async Task<TokenResponseDTO> CreateRefreshToken(User user)
        {
            var toekn = GenerateTocken(user);
            var refreshToekn = await GenerateAndSaveRefreshTokenAsync(user);
            // Get the host name of the machine running the API
            var hostLanIp = Environment.GetEnvironmentVariable("HOST_LAN_IP");
            var hostName = Environment.GetEnvironmentVariable("HOST_NAME");
            (hostLanIp, _) = hostLanIp is not null ? (hostLanIp, null) : GetDockerHostBridgeInfo();
            return new TokenResponseDTO { AccessToken = toekn, RefreshToken = refreshToekn, HostIpAddress = hostLanIp, HostName= hostName };
        }

        private string GenerateTocken(User user)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Email,user.Email.ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds

            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        }
        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(e => e.Id == userId);
            if (user is null || user.RefreshToekn != refreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
            {
                return null;
            }

            return user;
        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = generateRefreshToken();
            user.RefreshToekn = refreshToken;
            user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);
            //context.Users.Update(user);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private string generateRefreshToken()
        {

            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO id)
        {
            var user = await ValidateRefreshTokenAsync(id.UserId, id.RefreshToken);
            if (user is null)
            {
                return null;
            }
            return await CreateRefreshToken(user);
        }
    }
}
