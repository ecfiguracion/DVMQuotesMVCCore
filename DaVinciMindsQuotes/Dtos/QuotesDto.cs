using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DaVinciMindsQuotes.Dtos
{
    public class QuotesDto
    {
        public int Id { get; set; }

        [Required]
        public QuoteCategoryDto Category { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string Quotation { get; set; }

        public AuthorDto Author { get; set; }
        public int AuthorId { get; set; }

        public QuoteSourceDto Source { get; set; }
        public int SourceId { get; set; }

        [StringLength(100)]
        public string Keywords { get; set; }

        public string InterestingFacts { get; set; }

        public string Notes { get; set; }
    }
}
