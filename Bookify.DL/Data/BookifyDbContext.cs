using Bookify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bookify.DL.Data
{
    public class BookifyDbContext : IdentityDbContext<Customer>
    {
        public BookifyDbContext() { }     
        public BookifyDbContext(DbContextOptions<BookifyDbContext> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Invoice> Invoices { get; set; }        
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ReservedRoom> ReservedRooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<HotelImage> HotelImages { get; set; }

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

            modelBuilder.Entity<RoomType>()
                .Property(rt => rt.BasePrice)
                .HasColumnType("decimal(18,2)");

            // Configure Room.Status as enum
            modelBuilder.Entity<Room>()
                .Property(r => r.Status)
                .HasConversion<int>();

            // Configure Room-RoomType relationship
            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomType)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure RoomImage-RoomType relationship
            modelBuilder.Entity<RoomImage>()
                .HasOne(ri => ri.RoomType)
                .WithMany(rt => rt.GalleryImages)
                .HasForeignKey(ri => ri.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure HotelImage-Hotel relationship
            modelBuilder.Entity<HotelImage>()
                .HasOne(hi => hi.Hotel)
                .WithMany(h => h.GalleryImages)
                .HasForeignKey(hi => hi.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Hotel-RoomType relationship
            modelBuilder.Entity<RoomType>()
                .HasOne(rt => rt.Hotel)
                .WithMany(h => h.RoomTypes)
                .HasForeignKey(rt => rt.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
