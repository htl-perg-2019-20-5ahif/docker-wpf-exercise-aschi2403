using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Model;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarBookingContext _context;

        public CarsController(CarBookingContext context)
        {
            _context = context;
            AddTestData();
        }

        private void AddTestData()
        {
            if (_context.Cars.ToList().Count > 0)
                return;
            
            _context.Cars.Add(new Car { Name = "Ferrari" });
            _context.Cars.Add(new Car { Name = "Tesla" });

            _context.SaveChanges();
        }

        //List all available Cars
        // GET: api/Cars
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Car>>> GetAvailableCars()
        {
            return (await _context.Cars.Include(c => c.Bookings).ToListAsync())
                .Where(car => car.Bookings.TrueForAll(booking => booking.Time.Date != DateTime.Today.Date))
                .Select(c =>
                {
                    c.Bookings = null;
                    return c;
                }).ToList();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
