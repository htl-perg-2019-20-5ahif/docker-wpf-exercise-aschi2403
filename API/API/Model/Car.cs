using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Model
{
    public class Car
    {
        [JsonPropertyName("carId")]
        public int CarId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public List<Booking> Bookings { get; set; }  

        public Car()
        {
            Bookings = new List<Booking>();
        }
    }
}
