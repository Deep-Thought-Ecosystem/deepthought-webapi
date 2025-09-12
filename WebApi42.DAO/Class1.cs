using Microsoft.EntityFrameworkCore;

namespace WebApi42.DAO
{

    public class User
    {
        public User()
        {
            CreateAt = DateTime.Now.TimeOfDay;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
        public string UserName { get; set; }
        = string.Empty;

        public TimeSpan CreateAt { get; set; }



    }
    public class UserDBCOntext(DbContextOptions<UserDBCOntext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();

    }
}
