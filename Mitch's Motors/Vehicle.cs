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

        // Output a full summary of everything in the class
        public void summary()
        {
            Console.WriteLine($"\n**Vehicle**\n    - Registration: {Registration}\n    - Type: {Type}\n    - Price: £{Price}\n    - Colour: {Colour}\n    - Steats: {Seats}\n    - Boot Size: {Boot_space}l\n    - Brand: {Brand}\n    - Year Created: {Year}\n\n");
        }


    }
}
