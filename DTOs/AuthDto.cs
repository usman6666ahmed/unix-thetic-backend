using System.ComponentModel.DataAnnotations;

namespace API.DTOs {
  public class RegisterDto {
    [Required]
    [MaxLength(30)]
    [MinLength(3)]
    public string Username { get; set;}

    [MinLength(6)]
    public string Password { get; set;}
  }

  public class LoginDto: RegisterDto {}
}
