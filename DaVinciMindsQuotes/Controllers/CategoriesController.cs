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
    public class CategoriesController : Controller
    {
        private const string SORT_PARAM_NAME = "NameSortParam";
        private const string SORT_BY_NAME_DESC = "Name_Desc";
        private const string CURRENT_FILTER = "CurrentFilter";
        private const string CURRENT_SORT = "CurrentSort";

        private ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
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

            var categories = from c in _context.QuoteCategory select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(c => c.Name.Contains(searchString));
            }

            if (sortOrder == SORT_BY_NAME_DESC)
                categories = categories.OrderByDescending(c => c.Name);
            else
                categories = categories.OrderBy(c => c.Name);

            int pageSize = 5;

            return View(await PaginatedList<QuoteCategory>.CreateAsync(categories.AsNoTracking(), page ?? 1, pageSize));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var category = await _context.QuoteCategory
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.QuoteCategory
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View("Form", category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.QuoteCategory
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (category != null)
            {
                _context.QuoteCategory.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Categories");
        }

        public IActionResult New()
        {
            var category = new QuoteCategory();
            return View("Form", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(QuoteCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Form", category);
                }

                if (category.Id == 0)
                {
                    _context.Add(category);
                }
                else
                {
                    var categoriesInDb = await _context.QuoteCategory.SingleOrDefaultAsync(c => c.Id == category.Id);
                    categoriesInDb.Name = category.Name;
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return RedirectToAction("Index", "Categories");
        }
    }
}