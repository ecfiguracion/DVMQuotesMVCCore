using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DaVinciMindsQuotes.Models;
using DaVinciMindsQuotes.Data;
using DaVinciMindsQuotes.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DaVinciMindsQuotes.Controllers
{
    public class AuthorsController : Controller
    {
        private const string SORT_PARAM_NAME = "NameSortParam";
        private const string SORT_BY_NAME_DESC = "Name_Desc";
        private const string CURRENT_FILTER = "CurrentFilter";
        private const string CURRENT_SORT = "CurrentSort";

        private ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData[CURRENT_SORT] = sortOrder;
            ViewData[SORT_PARAM_NAME] = String.IsNullOrEmpty(sortOrder) ? SORT_BY_NAME_DESC : string.Empty;

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewData[CURRENT_FILTER] = searchString;

            var authors = from c in _context.Author select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                authors = authors.Where(c => c.Name.Contains(searchString));
            }

            if (sortOrder == SORT_BY_NAME_DESC)
                authors = authors.OrderByDescending(c => c.Name);
            else
                authors = authors.OrderBy(c => c.Name);

            int pageSize = 5;

            return View(await PaginatedList<Author>.CreateAsync(authors.AsNoTracking(), page ?? 1, pageSize));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var author = await _context.Author
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var author = await _context.Author
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View("Form", author);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Author
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (author != null)
            {
                _context.Author.Remove(author);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Authors");
        }

        public IActionResult New()
        {
            var author = new Author();
            return View("Form", author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Author author)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Form", author);
                }

                if (author.Id == 0)
                {
                    _context.Add(author);
                }
                else
                {
                    var authorsInDb = await _context.Author.SingleOrDefaultAsync(c => c.Id == author.Id);
                    authorsInDb.Name = author.Name;
                    authorsInDb.Website = author.Website;
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return RedirectToAction("Index", "Authors");
        }
    }
}