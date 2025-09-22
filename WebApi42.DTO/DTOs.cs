using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WebApi42.DTO
{

   
    public record RefreshTokenRequestDTO
    {
        public required Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
    public record TokenResponseDTO
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public string? HostIpAddress { get; set; }
        public string HostName { get; set; }
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
