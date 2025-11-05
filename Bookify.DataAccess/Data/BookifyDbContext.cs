using Bookify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bookify.DL.Data
{
    public class BookifyDbContext : IdentityDbContext<IdentityUser>
    {
        public BookifyDbContext() { }     
        public BookifyDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Invoice> Invoice { get; set; }        
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ReservedRoom> ReservedRooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  multiple cascade paths
            modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Reservation)
            .WithOne(r => r.Invoice)
            .HasForeignKey<Invoice>(i => i.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany()
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Hotel>()
            .HasOne(h => h.Company)
            .WithMany(c => c.Hotels)
            .HasForeignKey(h => h.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Room>()
                .Property(r => r.Price)
                .HasColumnType("decimal(18,2)");

        }

    }
}
