using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi42.DAO;
using WebApi42.DTO;
using Microsoft.Extensions.Primitives;    // Not strictly needed for GetValue, but for completeness
using Microsoft.Extensions.DependencyInjection; // Not strictly needed for GetValue

namespace WebApi42.Bussiness
{


    public class AuthService(UserDBCOntext context, IConfiguration configuration)
    {
        //public static User user = new();
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
                Role=userDTO.Role
            };

            user1.PasswordHashed = new PasswordHasher<User>().HashPassword(user1, userDTO.Password);

            var obj = await context.Users.AddAsync(user1);

            await context.SaveChangesAsync();

            //user.Email = user1.Email;
            //user.PasswordHashed = user1.PasswordHashed;

            return user1;

        }

        public async Task<User> LoginAsync(UserLoginAuthDTO id)
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

            return user;
        }

    

    }
}
