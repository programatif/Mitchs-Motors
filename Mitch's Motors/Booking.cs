using System;
using System.Collections.Generic;
using System.Text;
using Mitch_s_Motors;

namespace Mitch_s_Motors
{
    internal class Booking
    {
        // Attributes
        public DateTime Date { get; set; }
        public Vehicle Vehicle {  get; set; }
        public Customer Customer { get; set; }
        public Staff Staff { get; set; }

        private string Status = "Booked";
        private DateTime StartTime;
        private DateTime EndTime;

        // Constructor
        public Booking(DateTime date, Vehicle vehicle, Customer customer, Staff staff)
        {
            Date = date;
            Vehicle = vehicle;
            Customer = customer;
            Staff = staff;
        }

        // Behaviors 

        // Changes status to active, marking the test drive as having started and marking the staff member as being unavailable
        public void start()
        {
            if (Status == "Booked")
            {
                if (Staff.Currently_available)
                {
                    StartTime = DateTime.Now;
                    Status = "Active";
                    Staff.Currently_available = false;

                    Console.WriteLine("\n**INFO**\n    - Successfully started the test drive session");
                }
                else
                {
                    Console.WriteLine($"\n**WARNING**\n    - {Staff} is currently already on a test drive and is unavailable.");
                }
            } else
            {
                Console.WriteLine($"\n**WARNING**\n    - The current status of this booking is {Status}, therefore it cannot be started");
            }
            

        }

        // Changes status to compelte, marking the test drive as having been completed and marking the staff member as available
        public void end() {
            if (Status == "Active")
            {
                EndTime = DateTime.Now;
                Status = "Complete";
                Staff.Currently_available = true;

                Console.WriteLine($"\n**INFO**\n    - Successfully ended the test drive session");
            } else
            {
                Console.WriteLine($"\n**WARNING**\n    - The current status of the booking is {Status}, therefore it cannot be ended");
            }
            
        }

        // Changes the status to missed, marks the test drive as being missed and the customer as having missed a booking
        public void missed()
        {
            if (Status == "Booked")
            {
                Status = "Missed";
                Customer.missedBooking();
            } else
            {
                Console.WriteLine($"\n**INFO**\n    - Status is currently {Status}, therefore the booking cannot be marked as missed");
            }
        }

        // Changes the status to cancelled 
        public void cancel()
        {
            if (Status == "Booked")
            {
                Status = "Cancelled";
                Console.WriteLine("\n**INFO**\n    - Booking successfully cancelled.");
            } else {
                Console.WriteLine($"\n**INFO**\n    - Status is currently {Status}, therefore the booking cannot be cancelled.");
            }
        }

        // Output a full summary of everything in the class
        public void summary()
        {
            string startTime = Status == "Complete" || Status == "Active" ? StartTime.ToString() : "N/A";
            string endTime = Status == "Complete" ? EndTime.ToString() : "N/A";

            Console.WriteLine($"\n**Booking**\n    - Booked Date: {Date}\n    - Vehicle: {Vehicle.Registration}\n    - Customer: {Customer.Name}\n    - Customer Contact: {Customer.Email}\n    - Staff: {Staff.Name}\n    - Current Status: {Status}\n    - Session Started: {startTime}\n    - Session Ended: {endTime}");
        }

    }
}
