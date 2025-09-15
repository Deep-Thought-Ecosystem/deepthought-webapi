using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
        public string UserName { get; set; }
        = string.Empty;

        public string Role {  get; set; }= string.Empty;

        public TimeSpan CreateAt { get; set; }



    }
    public class UserDBCOntext(DbContextOptions<UserDBCOntext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); // ✅ Modern, preferred way
        }

    }
}
