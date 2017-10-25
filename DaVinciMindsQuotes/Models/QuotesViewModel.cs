using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaVinciMindsQuotes.Dtos;
using System.ComponentModel.DataAnnotations;

namespace DaVinciMindsQuotes.Models
{
    public class QuotesViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        public string Quotation { get; set; }

        [Display(Name ="Author")]
        public int AuthorId { get; set; }

        [Display(Name = "Source")]
        public int SourceId { get; set; }

        [StringLength(100)]
        public string Keywords { get; set; }

        public string InterestingFacts { get; set; }

        public string Notes { get; set; }

        public IEnumerable<QuoteCategory> Categories { get; set; }
        public IEnumerable<QuoteSource> Sources { get; set; }
        public IEnumerable<Author> Authors { get; set; }

    }
}
