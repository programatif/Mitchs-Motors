using System;
using System.Collections.Generic;
using System.Text;

namespace Mitch_s_Motors
{
    internal class Customer
    {
        // Attributes
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        // phone stored as string to account for potential extra characters, such as "+44"
        public string Phone { get; set; }
        private int Missed { get; set; } = 0;

        // Constructor 
        public Customer(string name, string address, string email, string phone)
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
        }

        // Behaviours

        // Increment missed bookings count by one
        public void missedBooking()
        {
            Missed++;
            
            if (! isEligible())
            {
                Console.WriteLine($"**INFO**\n    - {Name} has reached 3 missed bookings and is no longer eligible to book another test drive.");
            } else
            {
                string plural = Missed > 1 ? "s" : "";
                Console.WriteLine($"**INFO**\n    - {Name} has missed {Missed} booking{plural}, they can miss {3 - Missed} more until they are unable to book another test drive.");
            }
        }

        // Return true if customer has missed less than 3 test drive bookings
        public bool isEligible()
        {
            return Missed < 3;
        }

        // Output a full summary of everything in the class
        public void summary()
        {
            Console.WriteLine($"\n**Customer**\n    - Name: {Name}\n    - Address: {Address}\n    - Email: {Email}\n    - Phone Number: {Phone}\n    - Total Bookings Missed: {Missed}\n\n");
        }
    }
}
