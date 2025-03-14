using HotelReservations.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
namespace HotelReservations.Data
{
    public sealed class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Users>().Property(p => p.Created)
               .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Users>().Property(p => p.Modified)
               .HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
		public DbSet<Rooms> Rooms { get; set; }
		public DbSet<RoomType> RoomTypes{ get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationDetails> ReservationDetails { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
