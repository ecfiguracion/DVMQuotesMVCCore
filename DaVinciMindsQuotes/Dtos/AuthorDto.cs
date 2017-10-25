using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DaVinciMindsQuotes.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }

        public int ParentAuthorId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Website { get; set; }
    }
}
