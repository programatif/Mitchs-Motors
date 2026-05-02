using Mitch_s_Motors;
using System.Collections;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
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
	// Loop to allow multiple attempts if the user makes a mistake
	while (true)
	{
		Console.WriteLine(title);
		string input = Console.ReadLine();

		if (string.IsNullOrWhiteSpace(input))
		{
			// Runs tags for user spacing, error message output and then confirm waits for the user to acknowledge the issue
			tags();
            Console.WriteLine($"\n**ERROR**\n    - Input cannot be blank, please try again");
			confirm();
		} else
		{
			// return the user input after validation has been completed
			return input;
		}
	}
}

// Commonly used validation to confirm that an input is not null, and can be converted to an integer and gracefully handle any errors
static int convertInt(string title)
{
	while (true)
	{
		// Take the user input, confirm its not left blank
		string input = notNullInput(title);

		// Attempt to convert string to integer then return it, or alert the user of the issue and gracefully let them try again without crashing
		try
		{
			int converted = int.Parse(input);
			return converted;
		}
		catch (Exception)
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

        // Attempt to convert string to real number then return it, or alert the user of the issue and gracefully let them try again without crashing
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

		// Get the registration from the user
		string Registration = notNullInput("Enter Registration: ");

		// Confirm the registration isn't already present in another vehicle
        foreach (Vehicle vehicle in vehicles)
        {
            if (vehicle.Registration == Registration)
            {
                throw new Exception($"A vehicle with registration {Registration} already exists.");
            }
        }

		// Get all other attributes for a vehicle with validation
        string Type = notNullInput("Enter Type (e.g., Van, SUV, Hatchback...): ");
		double Price = convertDouble("Enter Price: ");
		string Colour = notNullInput("Enter Colour: ");
		int Seats = convertInt("Enter Number of Seats: ");
		double Boot_space = convertDouble("Enter Boot Size (Liters): ");
		string Brand = notNullInput("Enter Brand: ");
		int Year = convertInt("Enter Year: ");

		// create the vehicle object using the collected inputs
        Vehicle new_vehicle = new Vehicle(Registration, Type, Price, Colour, Seats, Boot_space, Brand, Year);

		// save the vehicle to the vehicles list to be findable later
		vehicles.Add( new_vehicle );

        Console.WriteLine("\nVehicle Successfully Registered");
        confirm();
    }
	catch (Exception e)
	{
		// Output any errors
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

			// Initilise a variable to count the number of vehicles
			int index = 1;
			// Check to make sure there are vehicles saved
			if (vehicles.Count > 0)
			{
				// Loop through all vehicles and output them to the user with the associated number to create an interactive menu
				foreach (Vehicle vehicle in vehicles)
				{
					// Only output the vehicle registration to easily identify the vehicle and take up the least amount of screen space
					Console.WriteLine($"    {index++}) {vehicle.Registration}");
				}
			} else
			{
				// Warn user of a lack of vehicles and stop the function from continuing
				Console.WriteLine("\n**WARNING**\n    - There are no saved vehicles");
				confirm();
				return;
			}

			// Get the users menu selection
			int convertedInput = convertInt($"\nIf you wish to view details about a single car, enter its associated number - or press \"{index}\" to exit.");

			// Check the user has entered a valid input that is in the menu (excluding exit)
			if (convertedInput > 0 && convertedInput < index)
			{
				// Get the user selected vehicle object from the list
				Vehicle selectedVehicle = vehicles[convertedInput - 1];

				// Rest the index to be re-used
				index = 1;

				tags();

				// Output information about the vehicle
				selectedVehicle.summary();

				string usrSelection = notNullInput("\nEnter \"1\" to edit this vehicle, enter \"2\" to exit");
				
				// Edit vehicle information
				if (usrSelection == "1")
				{
					try
					{
                        tags();
                        Console.WriteLine($"**Update Vehicle {selectedVehicle.Registration}**\n");
						
						// Get the vehicle registration, filter through all vehicles and ensure the vehicle with that plate doesn't already exist in the system (and isn't itself)
                        string Registration = notNullInput($"Enter Registration (currently {selectedVehicle.Registration}): ");
                        foreach (Vehicle vehicle in vehicles)
                        {
                            if (vehicle.Registration == Registration && vehicle != selectedVehicle)
                            {
                                throw new Exception($"A vehicle with registration {Registration} already exists.");
                            }
                        }
                        selectedVehicle.Registration = Registration;

						// Get all vehicle fields and save to the vehicle attribute

                        selectedVehicle.Type = notNullInput($"Enter Type (currently {selectedVehicle.Type}): ");
                        selectedVehicle.Price = convertDouble($"Enter Price (currently £{selectedVehicle.Price}): ");
                        selectedVehicle.Colour = notNullInput($"Enter Colour (currently {selectedVehicle.Colour}): ");
                        selectedVehicle.Seats = convertInt($"Enter Number of Seats (currently {selectedVehicle.Seats}): ");
                        selectedVehicle.Boot_space = convertDouble($"Enter Boot Size (currently {selectedVehicle.Boot_space}l): ");
                        selectedVehicle.Brand = notNullInput($"Enter Brand (currently {selectedVehicle.Brand}): ");
                        selectedVehicle.Year = convertInt($"Enter Year (currently {selectedVehicle.Year}): ");

                        Console.WriteLine($"\nSuccessfully updated vehicle {selectedVehicle.Registration}");
                        confirm();
                    }
					catch (Exception e)
					{
						// Catch and warn user of any errors
						tags();
						Console.WriteLine($"\n**ERROR**\n    - {e.Message}");
						confirm();
					}
					
                }
                
            }
			// If the user selects to exit the menu
			else if (convertedInput == index)
			{
				// Exit the loop 
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

		// Get the name of the new staff member

		string name = notNullInput("Enter the name of the staff member");

		// Create a new staff object, save the name and add them to the saved list
		Staff newStaff = new Staff(name);
		staffs.Add(newStaff);

		// Let the user know it was successful
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

	// Loop through all staff in the list and output all of their information
	foreach (Staff staff in staffs)
	{
		staff.summary();
	}

	// Await user acknowledgement before continuing
    confirm();
}

// Create a new customer and add to customer list
void addCustomer()
{
    try
    {
        tags();
        Console.WriteLine($"\n**Register New Customer**\n");

		// Get all customer information

		string name = notNullInput("Enter the name of the customer: ");
		string address = notNullInput("Enter the home address of the customer: ");
		string email = notNullInput("Enter the email address of the customer: ");
		string phone = notNullInput("Enter the phone number of the customer: ");


		// Create new customer object with the collected information and save to the customers list
		Customer newCustomer = new Customer(name, address, email, phone);
		customers.Add(newCustomer);

		// Tell the user it was successful
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

		// Loop over all customer objects in the list
		foreach (Customer customer in customers)
		{
            // initialise a variable to hold the warning if there is one (re-made for each user to keep a blank slate)
            string warning = "";
            // If the customer is not allowed to make a booking
            if (!customer.isEligible())
			{
				// Save a warning
				warning = " - WARNING: This customer is not allowed to book any test drives";
			} 

			// Output the menu code (and itterate it on one so they next one is higher and correct), customer's name and the warning if there is one
			Console.WriteLine($"    {index++}) {customer.Name}{warning}");
		}

		// Get the user menu selection
		int convertedSelection = convertInt($"If you wish to view more in detail, enter the associated number or press \"{index}\" to exit");

		tags();

		// Confirm the user's selection is valid
		if ( convertedSelection > 0 && convertedSelection < index)
		{
			// Find the customer that the user has selected from the list
			Customer selectedCustomer = customers[convertedSelection - 1];
			// Output all information about the selected customer
			selectedCustomer.summary();

			// As if the user wishes to edit the customer object attributes
            Console.WriteLine($"\nIf you wish to edit {selectedCustomer.Name}'s profile then press \"1\", else press any key");
			
			// Taking input manually since it can be null
			string selection = Console.ReadLine();
			
			// If user selected to edit || if entered anything else then it counts as exit
			if ( selection == "1")
			{
				// Get and update all attributes for the selected customer object
				selectedCustomer.Name = notNullInput($"Enter the name of the customer (currently {selectedCustomer.Name}): ");
				selectedCustomer.Address = notNullInput($"Enter the home address of the customer (currently {selectedCustomer.Address}): ");
				selectedCustomer.Email = notNullInput($"Enter the email address of the customer (currently {selectedCustomer.Email}): ");
				selectedCustomer.Phone = notNullInput($"Enter the phone number of the customer (currently {selectedCustomer.Phone}): ");

				// Tell the user it was successful 
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

		// Loop through and output all vehicles to make an interactive user menu
		foreach (Vehicle vehicle in vehicles)
		{
			Console.WriteLine($"    {index++}) {vehicle.Registration}");
		}

		// Get users menu selection
		int convertedSelection = convertInt($"\nSelect a vehicle that you want to remove, or press \"{index}\" to cancel");

		// Confirm the user has made a valid selection and isn't cancel
		if (convertedSelection > 0 && convertedSelection < index)
		{
			// Remove vehicle object from the list
			vehicles.RemoveAt(convertedSelection - 1);
			Console.WriteLine("\nVehicle successfully removed from the system");
            confirm();
            break;
		}
		// If the user selected cancel
		else if (convertedSelection == index)
		{
			// Let the user know that they did cancel and didn't fulfil the deletion
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
		
		//Loop through and ouput all staff objects in the list to create an interactive menu
        foreach (Staff staff in staffs)
        {
            Console.WriteLine($"    {index++}) {staff.Name}");
        }

		// Get the users menu selection
		int convertedSelection = convertInt($"\nSelect a staff that you want to remove, or press \"{index}\" to cancel");

		// Confirm user made a valid selection that isn't cancel
        if (convertedSelection > 0 && convertedSelection < index)
        {
			// Remove the staff member object from the list
            staffs.RemoveAt(convertedSelection - 1);
            Console.WriteLine("\nStaff member successfully removed from the system");
            confirm();
            break;
        }
		// If the user selected cancel
        else if (convertedSelection == index)
        {
			// Inform the user that they cancelled rather than deleting the staff member from the system
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

		// Create an interactive user menu by looping through the list and outputting customer names
        foreach (Customer customer in customers)
        {
            Console.WriteLine($"    {index++}) {customer.Name}");
        }

		// Get the user's menu selection
		int convertedSelection = convertInt($"\nSelect a customer that you want to remove, or press \"{index}\" to cancel");

		// Confirm user selected a valid option that isn't cancel
        if (convertedSelection > 0 && convertedSelection < index)
        {
			// Remove the customer object from the customers list
            customers.RemoveAt(convertedSelection - 1);
            Console.WriteLine("\nCustomer successfully removed from the system");
            confirm();
            break;
        }
		// If the user selected cancel
        else if (convertedSelection == index)
        {
			// Inform the user that they cancelled rather than deleting the customer object
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

	// Make sure that there is at least one of each object that the booking class is dependant on
	bool runnable = vehicles.Count > 0 && customers.Count > 0 && staffs.Count > 0;
	if (!runnable)
	{
		// Inform the user that there isn't all the required objects to create a new booking
		tags();
        Console.WriteLine($"\nPlease make sure there is at least one Staff, Customer and Vehicle in the system before attempting to make a booking.");
        confirm();
        return;
	}

	// Initialise variables to store the attributes for the new booking object
	DateTime selectDate;
	Vehicle selectVehicle;
	Customer selectCustomer;
	Staff selectStaff;

	// Get the booking date
	// Loop until the date has been entered correctly
    while (true)
    {
		// Get the date from the user
        string date = notNullInput("What is the date and time of the booking? (YYYY-MM-DD HH:MM)");

		// try to convert into a DateTime format
        try
        {
            // Store the selected date into the previously initialised variable
            selectDate = DateTime.Parse(date);
            break;
        }
        catch (Exception)
        {
			// If there was an error (user entered an invalid date/format), inform them
            Console.WriteLine($"Failed to convert {date} to date, make sure you follow the format YYYY-MM-DD HH:MM");
        }
    }

	// Get the booking vehicle
    while (true)
	{
        index = 1;
        Console.WriteLine("\nVehicles:");

		// Make an interactive user menu out of the vehicles list
		foreach (Vehicle vehicle in vehicles)
		{
			Console.WriteLine($"    {index++}) {vehicle.Registration}");
		}

		// Get the users menu selection
		int convertedSelection = convertInt("\nEnter the number of the vehicle you wish to use: ");

		// Validate the selection
		if (convertedSelection > 0 && convertedSelection < index)
		{
			// Store the selected vehicle into the previously initialised variable
			selectVehicle = vehicles[convertedSelection - 1];
			break;
		}
		else
		{
			// Inform the user they made an invalid selection
			tags();
			Console.WriteLine($"\n**ERROR**\nPlease enter a valid vehicle number between 1 and {index}.");
            confirm();
        }
	}

	// Get the booking customer
    while (true)
    {
        index = 1;
        Console.WriteLine("\nEligible Customers for this booking (if 3 test drives are missed, then they become ineligible and will not be listed here):");
        // Create an interactive user menu with the customer objects in the list
		foreach (Customer customer in customers)
        {
			// Confirm that the customer is allowed to make a booking
			if (customer.isEligible())
			{
				Console.WriteLine($"    {index++}) {customer.Name}");
			}
        }

		// Get the user selection
		int convertedSelection = convertInt("\nEnter the number of the customer for the booking");

		// Validate the selection
		if (convertedSelection > 0 && convertedSelection < index)
        {
            // Store the selected customer into the previously initialised variable
            selectCustomer = customers[convertedSelection - 1];
			break;
        }
        else
        {
			// Inform the user they did not pick a valid option
			tags();
            Console.WriteLine($"\n**ERROR**\nPlease enter a valid customer number between 1 and {index}.");
            confirm();
        }
    }

    // Get the booking staff member
    while (true)
    {
        index = 1;
        Console.WriteLine("\nStaff Members");
		// Create an interactive menu out of staff objects in the list
        foreach (Staff staff in staffs)
        {
            Console.WriteLine($"    {index++}) {staff.Name}");
        }

		// Get the user selection
		int convertedSelection = convertInt("\nEnter the number of the Staff member for this booking:");

		// Validate the user selection
        if (convertedSelection > 0 && convertedSelection < index)
        {
            // Store the selected staff into the previously initialised variable
            selectStaff = staffs[convertedSelection - 1];
			break;
        }
        else
        {
			// Inform the user they did not pick a valid option
			tags();
            Console.WriteLine($"\n**ERROR**\nPlease enter a valid staff number between 1 and {index}.");
            confirm();
        }
    }

	// Create a blank list for bookings that are close enough to have potential conflicts of resource usage
	List<Booking> closeBookings = new List<Booking>();

	// Create a blank list for conflicting resource usages (a car being in two overlapping bookings)
	List<string> conflicts = new List<string>();

	// Filter through to find bookings within an hour either side of the selected date/time of the new booking object and store it to the blank list
	foreach (Booking booking in bookings)
	{
		// Confirm that the booking is within an hour and is has not been cancelled, missed or is finished
		if (booking.Status == "Booked" || booking.Status == "Active")
		{
			if ((booking.Date - selectDate).TotalMinutes < 60 && (selectDate - booking.Date).TotalMinutes < 60)
			{
				closeBookings.Add(booking);
			}
		}
    }

	// Check in each of the overlapping bookings to see for conflicting resources and save to the conflicts list
	foreach (Booking booking in closeBookings)
	{
		if (booking.Vehicle == selectVehicle)
		{
			conflicts.Add($"Vehicle is in use at {booking.Date}");
		}

		if (booking.Staff == selectStaff)
		{
			conflicts.Add($"Staff member is busy at {booking.Date}");
		}

		if (booking.Customer == selectCustomer)
		{
			conflicts.Add($"Customer is bsuy at {booking.Date}");
		}
	}

	// If there are not any conflicts in the list
	if (!conflicts.Any())
	{
		// Create a new booking object, save it to the global bookings list and inform the user that it was successful and give them an overview of the booking details
		Booking newBooking = new Booking(selectDate, selectVehicle, selectCustomer, selectStaff);
		bookings.Add(newBooking);

		tags();
		Console.WriteLine("Successfully added booking:");
		newBooking.summary();
		confirm();
	
	// If there were conflicts found
	} else
	{
		tags();
		// Warn the user that there were x amount of conflicts and what they were
        Console.WriteLine($"\n**WARNING**\n\nThere were {conflicts.Count} conflicts for this booking's time slot:");
		foreach (string conflict in conflicts)
		{
            Console.WriteLine($"    - {conflict}");
		}
		confirm();
	}

}

// View all bookings and go into detail with them
void viewBookings()
{
	while (true)
	{
		tags();
		int index = 1;
		Console.WriteLine("**View All Bookings**");
		// Create an interactive menu with bookings from the list
		foreach (Booking booking in bookings)
		{
			Console.WriteLine($"    {index++}) {booking.Customer.Name} - Status: {booking.Status}");
		}

		// Get the users menu selection
		int convertedSelected = convertInt($"\nIf you wish to view more details, edit or change a booking status, enter the number associated with the booking or press \"{index}\" to cancel");

		// Validate the users menu selection, and not exit
		if (convertedSelected > 0 && convertedSelected < index)
		{
			while (true)
			{
				tags();
				// Get the booking that the user selected from the bookings list and output all information about it
				Booking selectedBooking = bookings[convertedSelected - 1];
				selectedBooking.summary();

				// Create another interactive menu for the user 
				string choice = notNullInput($"\n\nSelect your desired action:\n    1) Update Status\n    2) Edit Booking\n    3) Exit");

				// Update booking status
				if (choice == "1")
				{
					tags();
                    Console.WriteLine($"**Change {selectedBooking.Customer.Name} | {selectedBooking.Date} Status**");

					// If the current status of the selected booking is "Booked" - indicating it is scheduled and no further information has been given bring up the available options
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

                    // If the current status of the selected booking is "Active" - indicating the test drive is currently ongoing, bring up the available options
                    }
                    else if (selectedBooking.Status == "Active")
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

					// If the current status is missed or cancelled then inform the user that nothing can be done further to this booking
					} else
					{
						tags();
                        Console.WriteLine($"\nThis booking is currently marked as {selectedBooking.Status}, therefore it's status cannot be updated");
						confirm();
					}

                    confirm();
				
				// Edit the attributes of this booking
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

					// Create the new attributes variables to update the existing information
                    DateTime selectDate;
                    Vehicle selectVehicle;
                    Customer selectCustomer;
                    Staff selectStaff;

					// Get the new date
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

                    
					// Get the new vehicle used
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

					// Get the new customer for the booking
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

					// Get the new staff member for the booking
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

					// Update all the information
					selectedBooking.Date = selectDate;
					selectedBooking.Vehicle = selectVehicle;
					selectedBooking.Customer = selectCustomer;
					selectedBooking.Staff = selectStaff;

					// Inform the user it was a success
                    Console.WriteLine("\nBooking successfully updated.");
                    confirm();
                } else
				{
					break;
				}
			}
		}
		// If user chose to exit then break out of the loop to go back to main menu
		else if (convertedSelected == index)
		{
			break;
		}
		else
		{
			// Warn the user that they entered an invalid selection
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

		// Create the menu using the menu dictionary listed at the top of the file
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
			// Inform the user they selected an invalid option
			tags();
			Console.WriteLine($"\nPlease pick a valid option between 1 and {index}");
            confirm();
        }


	} catch (Exception e)
	{
		// Inform the user that there was a generic error to handle gracefully 
		tags();
        Console.WriteLine($"\n**ERROR**\n    - {e.Message}");
        confirm();
    }
}