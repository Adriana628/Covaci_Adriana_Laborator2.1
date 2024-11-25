using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Covaci_Adriana_Laborator2.Data;
using Covaci_Adriana_Laborator2.Models;

namespace Covaci_Adriana_Laborator2.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // Metodă pentru a popula lista de autori
        private void PopulateAuthorsDropDownList(object selectedAuthor = null)
        {
            var authorsQuery = from a in _context.Author
                               select new
                               {
                                   a.ID,
                                   FullName = a.FirstName + " " + a.LastName
                               };
            ViewData["AuthorID"] = new SelectList(authorsQuery, "ID", "FullName", selectedAuthor);
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var libraryContext = _context.Book.Include(b => b.Genre).Include(b => b.Author);
            return View(await libraryContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Genre)
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["GenreID"] = new SelectList(_context.Genre, "ID", "Name"); // Populează genurile
            PopulateAuthorsDropDownList();  // Folosește metoda pentru a popula autorii
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,AuthorID,Price,GenreID")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-populează dropdown-urile dacă validarea eșuează
            ViewData["GenreID"] = new SelectList(_context.Genre, "ID", "Name", book.GenreID);
            PopulateAuthorsDropDownList(book.AuthorID);
            return View(book);
        }


        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewData["GenreID"] = new SelectList(_context.Genre, "ID", "Name", book.GenreID); // Populează genurile
            PopulateAuthorsDropDownList(book.AuthorID); // Populează autorii
            return View(book);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,AuthorID,Price,GenreID")] Book book)
        {
            if (id != book.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ID))
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

            // Re-populează dropdown-urile dacă validarea eșuează
            ViewData["GenreID"] = new SelectList(_context.Genre, "ID", "Name", book.GenreID);
            PopulateAuthorsDropDownList(book.AuthorID);
            return View(book);
        }


        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Genre)
                .Include(b => b.Author)  // Corectare aici: includem Author, nu AuthorID
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.ID == id);
        }
    }
}
