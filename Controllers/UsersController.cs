using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class UsersController : BaseController
  {
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
      _context = context;
    }

    //  /users/
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
      return await _context.Users.ToListAsync();
    }

    //  /users/:id
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
      return await _context.Users.FindAsync(id);
    }
  }
}
