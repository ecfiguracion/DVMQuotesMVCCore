using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DaVinciMindsQuotes.Models;

namespace DaVinciMindsQuotes.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<QuoteCategory> QuoteCategory { get; set; }
        public DbSet<QuoteSource> QuoteSource { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Quotes> Quotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //An example on how to override the table name instead using the default plurals
            //as per example, without the override the EF will create table Authors
            //builder.Entity<Author>().ToTable("Author");
        }
    }
}
