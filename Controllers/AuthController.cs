using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class AuthController : BaseController
  {
    private readonly DataContext _context;

    public AuthController(DataContext context)
    {
      _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterDto registerDto)
    {

      if (await UserExists(registerDto.Username))
      {
        return BadRequest("Username is taken");
      }

      using var hmac = new HMACSHA512();
      var user = new User
      {
        Username = registerDto.Username.ToLower(),
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        PasswordSalt = hmac.Key
      };
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      return user;
    }


    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(LoginDto loginDto) {

      var user = await _context.Users.SingleOrDefaultAsync(
        user => user.Username == loginDto.Username
      );

      if (user == null) return Unauthorized("User not found");

      var hmac = new HMACSHA512(user.PasswordSalt);
      var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

      if (!CompareHashes(computedHash, user.PasswordHash)) return Unauthorized("Incorrect password");

      return user;
    }

    private async Task<bool> UserExists(string username)
    {
      return await _context.Users.AnyAsync(user => user.Username == username.ToLower());
    }

    private bool CompareHashes(byte[] h1, byte[] h2) {
      if (h1.Length != h2.Length) return false;
      for(var i=0; i<h1.Length; i++) {
        if (h1[i] != h2[i]) return false;
      }
      return true;
    }
  }

}
