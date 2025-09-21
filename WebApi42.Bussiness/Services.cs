using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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

        private async Task<TokenResponseDTO> CreateRefreshToken(User user)
        {
            var toekn = GenerateTocken(user);
            var refreshToekn = await GenerateAndSaveRefreshTokenAsync(user);
            // Get the host name of the machine running the API
            string hostName = Dns.GetHostName();

            // Get the IP addresses associated with that host name
            IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);

            // Find the first IPv4 address (you might need to adjust based on your network)
            IPAddress hostIpAddress = ipHostEntry.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            return new TokenResponseDTO { AccessToken = toekn, RefreshToken = refreshToekn, HostIpAddress= hostIpAddress.ToString() };
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
