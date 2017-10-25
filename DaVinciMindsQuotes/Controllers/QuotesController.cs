using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DaVinciMindsQuotes.Models;
using DaVinciMindsQuotes.Data;
using DaVinciMindsQuotes.Utilities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DaVinciMindsQuotes.Controllers
{
    public class QuotesController : Controller
    {
        private const string SORT_PARAM_NAME = "NameSortParam";
        private const string SORT_BY_NAME_DESC = "Name_Desc";
        private const string CURRENT_FILTER = "CurrentFilter";
        private const string CURRENT_SORT = "CurrentSort";

        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public QuotesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            var quotes = from c in _context.Quotes select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                quotes = quotes.Where(c => c.Quotation.Contains(searchString));
            }

            if (sortOrder == SORT_BY_NAME_DESC)
                quotes = quotes.OrderByDescending(c => c.Quotation);
            else
                quotes = quotes.OrderBy(c => c.Quotation);

            quotes = quotes.Include(i => i.Category).Include(i => i.Author);

            int pageSize = 5;

            return View(await PaginatedList<Quotes>.CreateAsync(quotes.AsNoTracking(), page ?? 1, pageSize));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var quote = await _context.Quotes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var quoteInDB = await _context.Quotes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (quoteInDB == null)
            {
                return NotFound();
            }

            var model = new QuotesViewModel();
            _mapper.Map(quoteInDB, model);

            this.SetQuotesDropDownSources(model);

            return View("Form", model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var quote = await _context.Quotes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (quote != null)
            {
                _context.Quotes.Remove(quote);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Quotes");
        }

        public async Task<IActionResult> New()
        {
            var quote = new QuotesViewModel();
            this.SetQuotesDropDownSources(quote);
            return View("Form", quote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(QuotesViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // I don't like this, will need to repeat filling up values for drop down lists
                    // Perhaps best to convert these sources to WEB API and invoke it on client side
                    this.SetQuotesDropDownSources(model);
                    return View("Form", model);
                }

                if (model.Id == 0)
                {
                    var quote = _mapper.Map<Quotes>(model);
                    _context.Add(quote);
                }
                else
                {
                    var quotesInDb = await _context.Quotes.SingleOrDefaultAsync(c => c.Id == model.Id);
                    _mapper.Map(model, quotesInDb);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return RedirectToAction("Index", "Quotes");
        }

        private void SetQuotesDropDownSources(QuotesViewModel model)
        {
            var categories = _context.QuoteCategory.ToList();
            var sources = _context.QuoteSource.ToList();
            var authors = _context.Author.ToList();

            model.Categories = categories;
            model.Sources = sources;
            model.Authors = authors;
        }
    }
}