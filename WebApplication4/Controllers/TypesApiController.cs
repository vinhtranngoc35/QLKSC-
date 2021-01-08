using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TypesApiController : ControllerBase
    {
        private readonly ProductContext _context;

        public TypesApiController(ProductContext context)
        {
            _context = context;
        }

        // GET: api/TypesApi
        [HttpGet]
        public IActionResult GetTypes()
        {
            return Ok(_context.Types.ToList());
        }

        // GET: api/TypesApi/5
        //[HttpGet("{id}")]
        //public async IActionResult GetTypeProduct(int id)
        //{
        //    var @type = await _context.Types.FindAsync(id);

        //    if (@type == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(@type);
        //}

        //    // PUT: api/TypesApi/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for
        //    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutType(int id, Type @type)
        //    {
        //        if (id != @type.ID)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(@type).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TypeExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return NoContent();
        //    }

        //    // POST: api/TypesApi
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for
        //    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //    [HttpPost]
        //    public async Task<ActionResult<Type>> PostType(Type @type)
        //    {
        //        _context.Types.Add(@type);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetType", new { id = @type.ID }, @type);
        //    }

        //    // DELETE: api/TypesApi/5
        //    [HttpDelete("{id}")]
        //    public async Task<ActionResult<Type>> DeleteType(int id)
        //    {
        //        var @type = await _context.Types.FindAsync(id);
        //        if (@type == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Types.Remove(@type);
        //        await _context.SaveChangesAsync();

        //        return @type;
        //    }

        //    private bool TypeExists(int id)
        //    {
        //        return _context.Types.Any(e => e.ID == id);
        //    }
        //}
    }
}
