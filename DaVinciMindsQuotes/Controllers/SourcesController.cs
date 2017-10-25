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
    public class SourcesController : Controller
    {
        private const string SORT_PARAM_NAME = "NameSortParam";
        private const string SORT_BY_NAME_DESC = "Name_Desc";
        private const string CURRENT_FILTER = "CurrentFilter";
        private const string CURRENT_SORT = "CurrentSort";

        private ApplicationDbContext _context;

        public SourcesController(ApplicationDbContext context)
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

            var sources = from c in _context.QuoteSource select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                sources = sources.Where(c => c.Name.Contains(searchString));
            }

            if (sortOrder == SORT_BY_NAME_DESC)
                sources = sources.OrderByDescending(c => c.Name);
            else
                sources = sources.OrderBy(c => c.Name);

            int pageSize = 5;

            return View(await PaginatedList<QuoteSource>.CreateAsync(sources.AsNoTracking(), page ?? 1, pageSize));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var sources = await _context.QuoteSource
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (sources == null)
            {
                return NotFound();
            }

            return View(sources);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sources = await _context.QuoteSource
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (sources == null)
            {
                return NotFound();
            }

            return View("Form", sources);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var sources = await _context.QuoteSource
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (sources != null)
            {
                _context.QuoteSource.Remove(sources);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Sources");
        }

        public IActionResult New()
        {
            var sources = new QuoteSource();
            return View("Form", sources);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(QuoteSource sources)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Form", sources);
                }

                if (sources.Id == 0)
                {
                    _context.Add(sources);
                }
                else
                {
                    var sourcesInDb = await _context.QuoteSource.SingleOrDefaultAsync(c => c.Id == sources.Id);
                    sourcesInDb.Name = sources.Name;
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return RedirectToAction("Index", "Sources");
        }
    }
}