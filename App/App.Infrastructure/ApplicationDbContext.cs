using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships, constraints, and seed data here
        }

        // Define DbSet properties for your domain entities
        //public DbSet<trinvoice> trinvoiceItems { get; set; }
        //public DbSet<trinvoicedetail> trinvoicedetailItems { get; set; }
        //public DbSet<ltcourierfee> ltcourierfeeItems { get; set; }
        //public DbSet<mscourier> mscourierItems { get; set; }
        //public DbSet<mspayment> mspaymentItems { get; set; }
        //public DbSet<msproduct> msproductItems { get; set; }
        //public DbSet<User> userItems { get; set; }

    }
}
