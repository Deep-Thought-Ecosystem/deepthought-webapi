using System.ComponentModel.DataAnnotations;

namespace WebApi42.DTO
{
    public record UserDTO
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

    public record UserRegisterAuthDTO
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }


    public record UserLoginAuthDTO
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
