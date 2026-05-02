using Mitch_s_Motors;
using System.Collections;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

// Initialise lists to store all user-created objects in memory to be called and used in the menu
List<Vehicle> vehicles = new List<Vehicle>();
List<Customer> customers = new List<Customer>();
List<Staff> staffs = new List<Staff>();
List<Booking> bookings = new List<Booking>();


// Dictionary to store all menu options and their attached functions - stored this way to make updating easier and code smaller/simpler to manage
Dictionary<string, Action> menuItems = new Dictionary<string, Action>
{
	{ "Register New Vehicle", addVehicle },
	{ "View All Vehicles", viewVehicles },
	{ "Remove Vehicle From System\n", deleteVehicle },
	{ "Add New Staff Member", addStaff },
	{ "View All Staff Members", viewStaff },
	{ "Remove Staff Member From System\n", deleteStaff },
	{ "Add New Customer", addCustomer },
	{ "View All Customers", viewCustomers },
	{ "Remove Customer From System\n", deleteCustomer },
	{ "Create A Booking", addBooking },
	{ "View and Manage Bookings\n", viewBookings }
};


// Functions

// Output a long list of tags to break up the terminal content
static void tags()
{
    Console.WriteLine("\n######################################################\n");
}

// Confirmation message to pause output until user has confirmed they have read the content on screen
static void confirm()
{
	Console.WriteLine("\nPress enter to continue...");
	Console.ReadLine();
}

// Commonly used validation to confirm that inputs are not null and gracefully handle errors
static string notNullInput(string title)
{
	while (true)
	{
		Console.WriteLine(title);
		string input = Console.ReadLine();

		if (string.IsNullOrWhiteSpace(input))
		{
			tags();
            Console.WriteLine($"\n**ERROR**\n    - Input cannot be blank, please try again");
			confirm();
		} else
		{
			return input;
		}
	}
}

// Commonly used validation to confirm that an input is not null, and can be converted to an integer and gracefully handle any errors
static int convertInt(string title)
{
	while (true)
	{
		string input = notNullInput(title);

		try
		{
			int converted = int.Parse(input);
			return converted;
		}
		catch (Exception e)
		{
			tags();
            Console.WriteLine($"\n**ERROR**\n    - {input} is not a number, please try again");
			confirm();
		}
	}
}

// Commonly used validation to confirm that an input is not null, and can be convered into a double and gracefully handle any errors
static double convertDouble(string title)
{
    while (true)
    {
        string input = notNullInput(title);

        try
        {
            double converted = double.Parse(input);
            return converted;
        }
        catch (Exception e)
        {
            tags();
            Console.WriteLine($"\n**ERROR**\n    - {input} is not a number, please try again");
            confirm();
        }
    }
}

// Create a new vehicle and save it in the vehicles list
void addVehicle()
{
	try
	{
		tags();
        Console.WriteLine($"\n**Register New Vehicle**\n");

		string Registration = notNullInput("Enter Registration: ");
		string Type = notNullInput("Enter Type (e.g., Van, SUV, Hatchback...): ");
		double Price = convertDouble("Enter Price: ");
		string Colour = notNullInput("Enter Colour: ");
		int Seats = convertInt("Enter Number of Seats: ");
		double Boot_space = convertDouble("Enter Boot Size (Liters): ");
		string Brand = notNullInput("Enter Brand: ");
		int Year = convertInt("Enter Year: ");

        Vehicle new_vehicle = new Vehicle(Registration, Type, Price, Colour, Seats, Boot_space, Brand, Year);
		vehicles.Add( new_vehicle );

        Console.WriteLine("\nVehicle Successfully Registered");
        confirm();
    }
	catch (Exception e)
	{
		tags();
        Console.WriteLine($"\n**ERROR**\n    - {e.Message}");
        confirm();
    }
}

// Loop through and output all vehicles on the vehicles list, also filter down to individual vehicles
void viewVehicles()
{
	while (true)
	{
		try
		{
			tags();
			Console.WriteLine($"\n**All Vehicles**\n");

			int index = 1;
			if (vehicles.Count > 0)
			{
				foreach (Vehicle vehicle in vehicles)
				{
					Console.WriteLine($"    {index++}) {vehicle.Registration}");
				}
			}

			int convertedInput = convertInt($"\nIf you wish to view details about a single car, enter its associated number - or press \"{index}\" to exit.");

			if (convertedInput > 0 && convertedInput < index)
			{
				Vehicle selectedVehicle = vehicles[convertedInput - 1];

				index = 1;

				tags();
				List<string> keys = selectedVehicle.summary_dict().Keys.ToList();

				foreach (string info in keys)
				{
					Console.WriteLine($"    {index++}) {info}: {selectedVehicle.summary_dict()[info]}");
				}

                confirm();


                // TODO: Finish editing vehicles - works other than getting correct datatype to save the input as

                //Console.WriteLine($"If you wish to edit any detials about this car, press the associated number. Else press \"{index}\".");

                //string secondSelect = Console.ReadLine();
                //int convertedSecondSelect = int.Parse(secondSelect);

                //if (convertedSecondSelect > 0 && convertedSecondSelect < index)
                //{
                //	tags();
                //	Console.WriteLine($"What should {keys[convertedSecondSelect - 1]} be updated to");
                //	string update = Console.ReadLine();
                //	selectedVehicle
                //}

            }
			else if (convertedInput == index)
			{
				break;
			}
			else
			{
				tags();
				Console.WriteLine($"\n**ERROR**\n    - Please enter a valid number between 1 and {index}.");
                confirm();
            }
		}
		catch (Exception e)
		{
			tags();
            Console.WriteLine($"\n**ERROR**\n    - {e.Message}");
            confirm();
        }
	}

}

// Create a new staff member and add them to the staff list
void addStaff()
{
    try
    {
		tags();
        Console.WriteLine($"\n**Register New Staff**\n");

		string name = notNullInput("Enter the name of the staff member");

		Staff newStaff = new Staff(name);
		staffs.Add(newStaff);

        Console.WriteLine($"\nSuccessfully added {name}");
        confirm();
    }
    catch (Exception e)
    {
		tags();
        Console.WriteLine($"\n**ERROR**\n    - {e.Message}");
        confirm();
    }
}

// View all staff members on the staff list and edit individuals
void viewStaff()
{
	tags();
	Console.WriteLine("\n**All Staff**\n");
	foreach (Staff staff in staffs)
	{
		staff.summary();
	}

    confirm();
}

// Create a new customer and add to customer list
void addCustomer()
{
    try
    {
        tags();
        Console.WriteLine($"\n**Register New Customer**\n");

		string name = notNullInput("Enter the name of the customer: ");
		string address = notNullInput("Enter the home address of the customer: ");
		string email = notNullInput("Enter the email address of the customer: ");
		string phone = notNullInput("Enter the phone number of the customer: ");

		Customer newCustomer = new Customer(name, address, email, phone);
		customers.Add(newCustomer);

        Console.WriteLine($"\nSuccessfully added {name}");
        confirm();
    }
    catch (Exception e)
    {
		tags();
        Console.WriteLine($"\n**ERROR**\n    - {e.Message}");
        confirm();
    }
}

// View all customers in the customers list and go into more detail
void viewCustomers()
{
	while (true)
	{
		tags();
		Console.WriteLine("\n**All Customers**\n");
		int index = 1;
		string warning = "";
		foreach (Customer customer in customers)
		{
			if (customer.Missed > 3)
			{
				warning = " - WARNING: This customer is not allowed to book any test drives";
			} 

			Console.WriteLine($"    {index++}) {customer.Name}{warning}");
		}

		int convertedSelection = convertInt($"If you wish to view more in detail, enter the associated number or press \"{index}\" to exit");

		tags();
		if ( convertedSelection > 0 && convertedSelection < index)
		{
			Customer selectedCustomer = customers[convertedSelection - 1];
			selectedCustomer.summary();

            Console.WriteLine($"\nIf you wish to edit {selectedCustomer.Name}'s profile then press \"1\", else press any key");
			string selection = Console.ReadLine();
			
			if ( selection == "1")
			{
				selectedCustomer.Name = notNullInput("Enter the name of the customer: ");
				selectedCustomer.Address = notNullInput("Enter the home address of the customer: ");
				selectedCustomer.Email = notNullInput("Enter the email address of the customer: ");
				selectedCustomer.Phone = notNullInput("Enter the phone number of the customer: ");

                Console.WriteLine($"\nSuccessfully updated {selectedCustomer.Name}");
                confirm();
            } 
		} else if (convertedSelection == index)
		{
			break;
		}
	}
}

// Delete a vehicle
void deleteVehicle()
{
	while (true)
	{
		tags();
		int index = 1;
		Console.WriteLine($"\n**Remove a Vehicle**\n");

		foreach (Vehicle vehicle in vehicles)
		{
			Console.WriteLine($"    {index++}) {vehicle.Registration}");
		}

		int convertedSelection = convertInt($"\nSelect a vehicle that you want to remove, or press \"{index}\" to cancel");

		if (convertedSelection > 0 && convertedSelection < index)
		{
			vehicles.RemoveAt(convertedSelection - 1);
			Console.WriteLine("\nVehicle successfully removed from the system");
            confirm();
            break;
		}
		else if (convertedSelection == index)
		{
			Console.WriteLine("\nCancelled");
			break;
		}
		else
		{
			tags();
			Console.WriteLine($"\n**ERROR**\n    - Please enter a valid vehicle or press \"{index}\" to cancel");
            confirm();
        }
	}
}

// Delete a Staff Member
void deleteStaff()
{
    while (true)
    {
        tags();
        int index = 1;
        Console.WriteLine($"\n**Remove a Staff Member**\n");

        foreach (Staff staff in staffs)
        {
            Console.WriteLine($"    {index++}) {staff.Name}");
        }

		int convertedSelection = convertInt($"\nSelect a staff that you want to remove, or press \"{index}\" to cancel");

        if (convertedSelection > 0 && convertedSelection < index)
        {
            staffs.RemoveAt(convertedSelection - 1);
            Console.WriteLine("\nStaff member successfully removed from the system");
            confirm();
            break;
        }
        else if (convertedSelection == index)
        {
            Console.WriteLine("\nCancelled");
            confirm();
            break;
        }
        else
        {
			tags();
            Console.WriteLine($"\n**ERROR**\n    - Please enter a valid staff member or press \"{index}\" to cancel");
            confirm();
        }
    }
}

// Delete a customer from the system
void deleteCustomer()
{
    while (true)
    {
        tags();
        int index = 1;
        Console.WriteLine($"\n**Remove a Customer**\n");

        foreach (Customer customer in customers)
        {
            Console.WriteLine($"    {index++}) {customer.Name}");
        }

		int convertedSelection = convertInt($"\nSelect a customer that you want to remove, or press \"{index}\" to cancel");

        if (convertedSelection > 0 && convertedSelection < index)
        {
            staffs.RemoveAt(convertedSelection - 1);
            Console.WriteLine("\nCustomer successfully removed from the system");
            confirm();
            break;
        }
        else if (convertedSelection == index)
        {
            Console.WriteLine("\nCancelled");
            confirm();
            break;
        }
        else
        {
			tags();
            Console.WriteLine($"\n**ERROR**\n    - Please enter a valid customer or press \"{index}\" to cancel");
            confirm();
        }
    }
}


// Bookings

// Create bookings and save to list
void addBooking()
{
	tags();
	int index = 1;
    Console.WriteLine("**Create a Booking**");

	bool runnable = vehicles.Count > 0 && customers.Count > 0 && staffs.Count > 0;
	if (!runnable)
	{
		tags();
        Console.WriteLine($"\nPlease make sure there is at least one Staff, Customer and Vehicle in the system before attempting to make a booking.");
        confirm();
        return;
	}

	DateTime selectDate;
	Vehicle selectVehicle;
	Customer selectCustomer;
	Staff selectStaff;

    Console.WriteLine("What is the date and time of the booking? (YYYY-MM-DD HH:SS)");
	string date = Console.ReadLine();
	selectDate = DateTime.Parse(date);

	while (true)
	{
        index = 1;
        Console.WriteLine("\nVehicles:");
		foreach (Vehicle vehicle in vehicles)
		{
			Console.WriteLine($"    {index++}) {vehicle.Registration}");
		}

		int convertedSelection = convertInt("\nEnter the number of the vehicle you wish to use: ");

		if (convertedSelection > 0 && convertedSelection < index)
		{
			selectVehicle = vehicles[convertedSelection - 1];
			break;
		}
		else
		{
			tags();
			Console.WriteLine($"\n**ERROR**\nPlease enter a valid vehicle number between 1 and {index}.");
            confirm();
        }
	}

	
    while (true)
    {
        index = 1;
        Console.WriteLine("\nEligible Customers for this booking (if over 3 missed test drives then they become ineligible and will not be listed here):");
        foreach (Customer customer in customers)
        {
			if (customer.isEligible())
			{
				Console.WriteLine($"    {index++}) {customer.Name}");
			}
        }

		int convertedSelection = convertInt("\nEnter the number of the customer for the booking");

		if (convertedSelection > 0 && convertedSelection < index)
        {
            selectCustomer = customers[convertedSelection - 1];
			break;
        }
        else
        {
			tags();
            Console.WriteLine($"\n**ERROR**\nPlease enter a valid customer number between 1 and {index}.");
            confirm();
        }
    }

    
    while (true)
    {
        index = 1;
        Console.WriteLine("\nStaff Members");
        foreach (Staff staff in staffs)
        {
            Console.WriteLine($"    {index++}) {staff.Name}");
        }

		int convertedSelection = convertInt("\nEnter the number of the Staff member for this booking:");

        if (convertedSelection > 0 && convertedSelection < index)
        {
            selectStaff = staffs[convertedSelection - 1];
			break;
        }
        else
        {
			tags();
            Console.WriteLine($"\n**ERROR**\nPlease enter a valid staff number between 1 and {index}.");
            confirm();
        }
    }

	Booking newBooking = new Booking(selectDate, selectVehicle, selectCustomer, selectStaff);
	bookings.Add(newBooking);

	tags();
    Console.WriteLine("Successfully added booking:");
	newBooking.summary();
    confirm();

}

// View all bookings and go into detail with them
void viewBookings()
{
	while (true)
	{
		tags();
		int index = 1;
		Console.WriteLine("**View All Bookings**");
		foreach (Booking booking in bookings)
		{
			Console.WriteLine($"    {index++}) {booking.Customer.Name} - Status: {booking.Status}");
		}

		int convertedSelected = convertInt($"\nIf you wish to view more details, edit or change a booking status, enter the number associated with the booking or press \"{index}\" to cancel");

		if (convertedSelected > 0 && convertedSelected < index)
		{
			while (true)
			{
				tags();
				Booking selectedBooking = bookings[convertedSelected - 1];
				selectedBooking.summary();

				string choice = notNullInput($"\n\nSelect your desired action:\n    1) Update Status\n    2) Edit Booking\n    3) Exit");

				if (choice == "1")
				{
					tags();
                    Console.WriteLine($"**Change {selectedBooking.Customer.Name} | {selectedBooking.Date} Status**");

					if (selectedBooking.Status == "Booked")
					{
						Console.WriteLine("    1) Begin Test Drive\n    2) Cancel Booking\n    3) Mark as Missed\n    4) Cancel");
						string usrChoice = Console.ReadLine();

						if (usrChoice == "1")
						{
							selectedBooking.start();
						} else if (usrChoice == "2")
						{
							selectedBooking.cancel();
						} else if (usrChoice == "3")
						{
							selectedBooking.missed();
						} else
						{
                            Console.WriteLine("\nCancelled");
                        }
					} else if (selectedBooking.Status == "Active")
					{
                        Console.WriteLine("    1) End Test Drive\n    2) Cancel");
						string usrChoice = Console.ReadLine();

						if (usrChoice == "1")
						{
							selectedBooking.end();
						} else
						{
                            Console.WriteLine("\nCancelled");
						}
					} else
					{
                        Console.WriteLine($"\nThis booking is currently marked as {selectedBooking.Status}, therefore it's status cannot be updated");
					}

                    confirm();

                } else if (choice == "2")
				{
					tags();
                    Console.WriteLine($"**Edit {selectedBooking.Customer.Name} | {selectedBooking.Date} Booking**");

                    // Get users choices to update booking
                    bool runnable = vehicles.Count > 0 && customers.Count > 0 && staffs.Count > 0;
                    if (!runnable)
                    {
                        Console.WriteLine($"\nPlease make sure there is at least one Staff, Customer and Vehicle in the system before attempting to make a booking.");
                        confirm();
                        return;
                    }

                    DateTime selectDate;
                    Vehicle selectVehicle;
                    Customer selectCustomer;
                    Staff selectStaff;

					while (true)
					{
						string date = notNullInput("What is the date and time of the booking? (YYYY-MM-DD HH:MM)");

						try
						{
							selectDate = DateTime.Parse(date);
							break;
						}
						catch (Exception e)
						{
							Console.WriteLine($"Failed to convert {date} to date, make sure you follow the format YYYY-MM-DD HH:MM");
						}
					}

                    

                    while (true)
                    {
                        index = 1;
                        Console.WriteLine("\nVehicles:");
                        foreach (Vehicle vehicle in vehicles)
                        {
                            Console.WriteLine($"    {index++}) {vehicle.Registration}");
                        }

						int convertedSelection = convertInt("\nSelect the number of the vehicle you wish to use:");

                        if (convertedSelection > 0 && convertedSelection < index)
                        {
                            selectVehicle = vehicles[convertedSelection - 1];
                            break;
                        }
                        else
                        {
							tags();
                            Console.WriteLine($"\n**ERROR**\nPlease enter a valid vehicle number between 1 and {index}.");
                            confirm();
                        }
                    }


                    while (true)
                    {
                        index = 1;
                        Console.WriteLine("\nCustomers:");
                        foreach (Customer customer in customers)
                        {
                            Console.WriteLine($"    {index++}) {customer.Name}");
                        }

						int convertedSelection = convertInt("\nSelect the number of the Customer for this booking");

                        if (convertedSelection > 0 && convertedSelection < index)
                        {
                            selectCustomer = customers[convertedSelection - 1];
                            break;
                        }
                        else
                        {
							tags();
                            Console.WriteLine($"\n**ERROR**\nPlease enter a valid customer number between 1 and {index}.");
                            confirm();
                        }
                    }


                    while (true)
                    {
                        index = 1;
                        Console.WriteLine("\nStaff:");
                        foreach (Staff staff in staffs)
                        {
                            Console.WriteLine($"    {index++}) {staff.Name}");
                        }

						int convertedSelection = convertInt("\nSelect the number of the Staff member for this booking:");

                        if (convertedSelection > 0 && convertedSelection < index)
                        {
                            selectStaff = staffs[convertedSelection - 1];
                            break;
                        }
                        else
                        {
							tags();
                            Console.WriteLine($"\n**ERROR**\nPlease enter a valid staff number between 1 and {index}.");
                            confirm();
                        }
                    }

					selectedBooking.Date = selectDate;
					selectedBooking.Vehicle = selectVehicle;
					selectedBooking.Customer = selectCustomer;
					selectedBooking.Staff = selectStaff;

                    Console.WriteLine("\nBooking successfully updated.");
                    confirm();
                } else
				{
					break;
				}
			}
		}
		else if (convertedSelected == index)
		{
			break;
		}
		else
		{
			tags();
			Console.WriteLine($"\nPlease enter a valid option between 1 and {index}");
            confirm();
        }
	}

}

// Loop through menu dictionary and output all options to the user, wait for their input and run the associated function
while (true)
{
	try
	{
		tags();
		Console.WriteLine($"\n**MENU**");
		int index = 1;

		foreach (string item in menuItems.Keys)
		{
			Console.WriteLine($"    {index++}) {item}");
		}

		Console.WriteLine($"    {index}) Exit");

		// Take in the users selection and convert it to an integer but catch and allow the user to re-try if they make a mistake

		int intSelection = convertInt("\nPlease pick an option:");

		// Check to see if the user selected an item in that exists in the dictionary (excluding the additional exit)
		if (intSelection > 0 && intSelection < index)
		{
			// Find the selected key from the dictionary to use its value pair function
			List<string> keys = new List<string>(menuItems.Keys);
			string key = keys[intSelection - 1];

			// Run the function
			menuItems[key]();
		} else if (intSelection == index) // If the user selected exit
		{
			break;
		}
		else
		{
			tags();
			Console.WriteLine($"\nPlease pick a valid option between 1 and {index}");
            confirm();
        }


	} catch (Exception e)
	{
		tags();
        Console.WriteLine($"\n**ERROR**\n    - {e.Message}");
        confirm();
    }
}