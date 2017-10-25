using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DaVinciMindsQuotes.Models;
using DaVinciMindsQuotes.Dtos;

namespace DaVinciMindsQuotes
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {         
            CreateMap<QuotesViewModel, Quotes>();
            CreateMap<Quotes, QuotesViewModel>();
        }
    }
}
