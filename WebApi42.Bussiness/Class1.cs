using Microsoft.AspNetCore.Identity;
using WebApi42.DAO;
using WebApi42.DTO;

namespace WebApi42.Bussiness
{
  

    public class UsersService(UserDBCOntext context) {
        public static User user = new();
        public async Task<Guid> Add(UserRegisterAuthDTO userDTO) {

            var user1 = new User { 
             Email = userDTO.Email,
               PasswordHashed =new PasswordHasher<User>().HashPassword(user, userDTO.Password),
            };
            var obj = await context.Users.AddAsync(user1);

            await context.SaveChangesAsync();

            user.Email = user1.Email;
            user.PasswordHashed = user1.PasswordHashed;
        
            return user1.Id;
        
        }
        public async Task Delete(Guid id) {
        
        
        }
        public async Task Update(Guid id,UserDTO user) { }
        public async Task Get(UserLoginAuthDTO id) { }
        public async Task<List<UserDTO>> List() { return new List<UserDTO>(); }
    }
}
