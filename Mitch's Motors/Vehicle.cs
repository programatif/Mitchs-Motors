using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Mitch_s_Motors
{
    internal class Vehicle
    {
        // Attributes
        public string Registration { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public string Colour { get; set; }
        public int Seats { get; set; }
        public double Boot_space { get; set; }
        public string Brand { get; set; }
        public int Year { get; set; }


        // Constructors
        public Vehicle(string registration, string type, double price, string colour, int seats, double boot_space, string brand, int year)
        {
            Registration = registration;
            Type = type;
            Price = price;
            Colour = colour;
            Seats = seats;
            Boot_space = boot_space;
            Brand = brand;
            Year = year;
        }


        // Behaviours

        // Return a dictionary of attributes to be used in code rather than output to the user, like the other summary does
        public Dictionary<string, object> summary_dict()
        {
            Dictionary<string, object> summary = new Dictionary<string, object>
            {
                { "Registration", Registration },
                { "Type", Type },
                { "Price", Price },
                { "Colour", Colour },
                { "Seats", Seats },
                { "Boot Space", Boot_space  },
                { "Brand", Brand },
                { "Year", Year }
            };

            return summary;
        }

        // Output a full summary of everything in the class
        public void summary()
        {
            Console.WriteLine($"\n**Vehicle**\n    - Registration: {Registration}\n    - Type: {Type}\n    - Price: {Price}\n    - Colour: {Colour}\n    - Steats: {Seats}\n    - Boot Size: {Boot_space}\n    - Brand: {Brand}\n    - Year Created: {Year}\n\n");
        }


    }
}
