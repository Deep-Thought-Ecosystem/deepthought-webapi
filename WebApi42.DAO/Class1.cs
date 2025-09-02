using Microsoft.EntityFrameworkCore;

namespace WebApi42.DAO
{

    public class User { 
    
    public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        = string.Empty;
        public User() { }

    }
    public class UserDBCOntext(DbContextOptions<UserDBCOntext> options):DbContext(options) 
    {
        public DbSet<User> Users => Set<User>();

    }
}
