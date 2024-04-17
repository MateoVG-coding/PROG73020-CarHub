using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            // Listings - User (Many-to-One)
            modelBuilder.Entity<Listings>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.Username); // Assuming UserId is the foreign key property in Listings

            // Reviews - User (Many-to-One)
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.Username); // Assuming UserId is the foreign key property in Reviews
        }
    }
}
