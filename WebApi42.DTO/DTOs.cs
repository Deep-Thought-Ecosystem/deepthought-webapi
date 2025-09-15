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
    public record RefreshTokenRequestDTO
    {
        public required Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
    public record TokenResponseDTO
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        // public required DateTime RefreshTokenExpireTime { get; set; }
    }

    public record UserRegisterAuthDTO
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

    }


    public record UserLoginAuthDTO
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
