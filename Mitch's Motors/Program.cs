using Mitch_s_Motors;

Vehicle new_vehicle = new Vehicle("GDR 4SD2", "Hatchback", 6500.00, "Silver", 7, 27.5, "Ford", 2020);
Customer sharon = new Customer("Sharon", "1 Testing Ave", "sharon@gmail.com", "+44 7348 342209");
Staff dylan = new Staff("Dylan");

DateTime booking_date = DateTime.Parse("2026-06-12 13:30:00");
Booking new_booking = new Booking(booking_date, new_vehicle, sharon, dylan);

new_booking.summary();
dylan.summary();

new_booking.start();
new_booking.summary();
dylan.summary();
new_booking.end();
new_booking.summary();
dylan.summary();