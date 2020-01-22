using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    public class CarBookingContext: DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public CarBookingContext(DbContextOptions<CarBookingContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
