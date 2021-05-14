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
        public async Task<IActionResult> Index(int? memberId)
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "FullName", memberId);

            var query = _context.MemberBooks.AsQueryable();

            if (memberId != null)
                query = query.Where(mb => mb.MemberId == memberId);

            var applicationDbContext = query.Include(m => m.Book).Include(m => m.Member);

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
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "FullName");
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
                var book = await _context.Books
                    .Include(x => x.Author)
                    .FirstOrDefaultAsync(b => b.Id == memberBook.BookId);
                if (book == null)
                    return NotFound();

                var member = await _context.Members
                    .Include(m => m.MemberBooks)
                    .FirstOrDefaultAsync(m => m.Id == memberBook.MemberId);
                if (member == null)
                    return NotFound();

                var nonTakenBackBookNames = await _context.MemberBooks
                    .Include(mb => mb.Book)
                    .Where(mb => !mb.IsTakenBack && mb.DeliveryDate < DateTime.Now.AddDays(-15))
                    .Select(mb => mb.Book.Name)
                    .ToListAsync();

                if (book.Quantity == 0)
                {
                    ModelState.AddModelError(string.Empty, $"\"{book.Author.FullName} - {book.Name}\" adlı kitap mevcut değildir!");
                }
                else if (nonTakenBackBookNames.Any())
                {
                    ModelState.AddModelError(string.Empty, $"\"{member.FullName}\" adlı üyenin 15 gün içerisinde teslim etmediği kitap(lar) mevcut olduğu için yeni kitap teslimi yapılamaz!{Environment.NewLine}Teslim edilmesi gereken kitap(lar):");

                    foreach (var bookName in nonTakenBackBookNames)
                        ModelState.AddModelError(string.Empty, bookName);
                }
                else if (member.MemberBooks.Any(mb => mb.BookId == memberBook.BookId && !mb.IsTakenBack))
                {
                    ModelState.AddModelError(string.Empty, $"\"{member.FullName}\" adlı üye \"{book.Name}\" adlı kitabı daha önce teslim almıştır!");
                }
                else if (member.MemberBooks.Count(b => !b.IsTakenBack) == 3)
                {
                    ModelState.AddModelError(string.Empty, $"\"{member.FullName}\" adlı üye en fazla 3 adet kitap teslim alabilir!");
                }
                else
                {
                    book.Quantity--;
                    _context.Update(book);

                    _context.Add(memberBook);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            FillDropdowns(memberBook);
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
            FillDropdowns(memberBook);
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
            FillDropdowns(memberBook);
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

            var book = await _context.Books.FindAsync(memberBook.BookId);
            if (book != null && !memberBook.IsTakenBack)
                book.Quantity++;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: MemberBooks/TakeBack/5
        public async Task<IActionResult> TakeBack(int? id)
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

        // POST: MemberBooks/TakeBack/5
        [HttpPost, ActionName("TakeBack")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeBackConfirmed(int id)
        {
            var memberBook = await _context.MemberBooks.FindAsync(id);
            memberBook.IsTakenBack = true;

            var book = await _context.Books.FindAsync(memberBook.BookId);
            if (book != null)
                book.Quantity++;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberBookExists(int id)
        {
            return _context.MemberBooks.Any(e => e.Id == id);
        }

        private void FillDropdowns(MemberBook memberBook)
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", memberBook.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "FullName", memberBook.MemberId);
        }
    }
}
