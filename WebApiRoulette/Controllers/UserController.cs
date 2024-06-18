using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRoulette.Context;
using WebApiRoulette.Models;

namespace WebApiRoulette.Controllers
{
    //[EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{Name}")]
        public async Task<ActionResult<User>> GetUser(string Name)
        {
            
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == Name);
            var user = await _context.Users.FirstOrDefaultAsync(u => EF.Functions.Like(u.Name, Name));

            if (user == null)
            {

                //return new NotFoundObjectResult(new { Message = "Usuario no encontrado" });
                    return new NoContentResult();
            }

            return new OkObjectResult(user);
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{name}")]
        public async Task<IActionResult> PutUser(string name, User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Name == name);

            if (existingUser == null)
            {
                return NotFound();
            }

            if (existingUser.Name != user.Name)
            {
                //return BadRequest();
                existingUser.Name = user.Name;
            }
                existingUser.Saldo = user.Saldo;

            _context.Entry(existingUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.Name = user.Name.ToLower();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { Name = user.Name }, user);
        }

        // DELETE: api/User/5
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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
