using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Controllers
{
    public class MemberBooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberBooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MemberBooks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MemberBooks.Include(m => m.Book).Include(m => m.Member);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MemberBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberBook = await _context.MemberBooks
                .Include(m => m.Book)
                .Include(m => m.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (memberBook == null)
            {
                return NotFound();
            }

            return View(memberBook);
        }

        // GET: MemberBooks/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name");
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EMail");
            return View();
        }

        // POST: MemberBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,BookId,DeliveryDate,IsTakenBack")] MemberBook memberBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", memberBook.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EMail", memberBook.MemberId);
            return View(memberBook);
        }

        // GET: MemberBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberBook = await _context.MemberBooks.FindAsync(id);
            if (memberBook == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", memberBook.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EMail", memberBook.MemberId);
            return View(memberBook);
        }

        // POST: MemberBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,BookId,DeliveryDate,IsTakenBack,Id")] MemberBook memberBook)
        {
            if (id != memberBook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberBookExists(memberBook.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", memberBook.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EMail", memberBook.MemberId);
            return View(memberBook);
        }

        // GET: MemberBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberBook = await _context.MemberBooks
                .Include(m => m.Book)
                .Include(m => m.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (memberBook == null)
            {
                return NotFound();
            }

            return View(memberBook);
        }

        // POST: MemberBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberBook = await _context.MemberBooks.FindAsync(id);
            _context.MemberBooks.Remove(memberBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberBookExists(int id)
        {
            return _context.MemberBooks.Any(e => e.Id == id);
        }
    }
}
