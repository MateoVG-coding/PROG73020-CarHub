﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Entities
{
    public class ListingDbContext : IdentityDbContext<User>
    {
        public ListingDbContext(DbContextOptions<ListingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listings> Listings { get; set; }
        public DbSet<Cars> Cars { get; set; }
        public DbSet<Reviews> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Relationships

            // Listings - Cars (One-to-One or Many-to-One)
            modelBuilder.Entity<Listings>()
                .HasOne(l => l.Car)
                .WithMany()
                .HasForeignKey(l => l.CarId); // Assuming CarId is the foreign key property in Listings
        }
    }
}
