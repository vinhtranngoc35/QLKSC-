using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class TypeRoomsController : Controller
    {
        private readonly ProductContext _context;

        public TypeRoomsController(ProductContext context)
        {
            _context = context;
        }

        // GET: TypeRooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeRooms.ToListAsync());
        }

        // GET: TypeRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeRoom = await _context.TypeRooms
                .FirstOrDefaultAsync(m => m.ID == id);
            if (typeRoom == null)
            {
                return NotFound();
            }

            return View(typeRoom);
        }

        // GET: TypeRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Price,IsDelete")] TypeRoom typeRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeRoom);
        }

        // GET: TypeRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeRoom = await _context.TypeRooms.FindAsync(id);
            if (typeRoom == null)
            {
                return NotFound();
            }
            return View(typeRoom);
        }

        // POST: TypeRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price,IsDelete")] TypeRoom typeRoom)
        {
            if (id != typeRoom.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeRoomExists(typeRoom.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(typeRoom);
        }

        // GET: TypeRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeRoom = await _context.TypeRooms
                .FirstOrDefaultAsync(m => m.ID == id);
            if (typeRoom == null)
            {
                return NotFound();
            }

            return View(typeRoom);
        }

        // POST: TypeRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeRoom = await _context.TypeRooms.FindAsync(id);
            _context.TypeRooms.Remove(typeRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeRoomExists(int id)
        {
            return _context.TypeRooms.Any(e => e.ID == id);
        }
    }
}
