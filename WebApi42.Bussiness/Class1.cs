using WebApi42.DAO;
using WebApi42.DTO;

namespace WebApi42.Bussiness
{
  

    public class UsersService(UserDBCOntext context) {
        public async Task<Guid> Add(UserDTO userDTO) {

            var user = new User { 
            UserName = userDTO.UserName,
             Email = userDTO.Email,
              Name = userDTO.Name,
               Password = userDTO.Password,
            };
            var obj = await context.Users.AddAsync(user);

            await context.SaveChangesAsync();
        
            return user.Id;
        
        }
        public async Task Delete(Guid id) { }
        public async Task Update(Guid id,UserDTO user) { }
        public async Task Get(Guid id) { }
        public async Task<List<UserDTO>> List() { return new List<UserDTO>(); }
    }
}
