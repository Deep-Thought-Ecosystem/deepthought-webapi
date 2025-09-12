using System.ComponentModel.DataAnnotations;

namespace WebApi42.DTO
{
    public class UserDTO
    {

        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserName { get; set; }
        = string.Empty;
        public UserDTO() { }

    }

    public class UserRegisterAuthDTO
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }


    public class UserLoginAuthDTO
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
    public class Class1
    {

    }
}
