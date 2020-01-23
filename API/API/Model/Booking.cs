using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime Time { get; set; }
        public Car Car { get; set; }
    }
}
