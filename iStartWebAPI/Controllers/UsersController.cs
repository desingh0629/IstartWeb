using iStartWebAPI.Data;
using iStartWebAPI.Helpers;
using iStartWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iStartWebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> GetUsersById(int Id)
        {
            return await _context.Users.Where(s=>s.Id == Id).FirstOrDefaultAsync();
        }


        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {
            try
            {
                if (user.Id != 0)
                {
                    var data = await _context.Users.Where(s => s.Id == user.Id).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        data.Name = user.Name;
                        data.Gender = user.Gender;
                        data.Password = user.Password;
                        data.Email = user.Email;
                        await _context.SaveChangesAsync();
                        return Ok("Updated");
                    }
                }
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }
            catch (Exception)
            {

                throw;
            }
        } 
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<LoginVM>> Login(LoginVM user)
        {
            var checkIsexist = await _context.Users.Where(s => s.Email == user.Email && s.Password == user.Password).FirstOrDefaultAsync();
            if(checkIsexist is null)
            {
                return BadRequest("Invalid User");
            }
            return Ok(checkIsexist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
