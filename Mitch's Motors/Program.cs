using Mitch_s_Motors;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

// Initialise lists to store all user-created objects in memory to be called and used in the menu
List<Vehicle> vehicles = new List<Vehicle>();
List<Customer> customers = new List<Customer>();
List<Staff> staffs = new List<Staff>();
List<Booking> bookings = new List<Booking>();


// Dictionary to store all menu options and their attached functions
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
	{ "Remove Customer From System\n", deleteCustomer }
};


// Functions

// Output a long list of tags to break up the terminal content
static void tags()
{
    Console.WriteLine("\n######################################################\n");
} 

// Create a new vehicle and save it in the vehicles list
void addVehicle()
{
	try
	{
		tags();
        Console.WriteLine($"\n**Register New Vehicle**\n");

        Console.WriteLine("Enter Registration: ");
        string Registration = Console.ReadLine();

        Console.WriteLine("Enter Type (e.g., Sedan, SUV): ");
        string Type = Console.ReadLine();

        Console.WriteLine("Enter Price: ");
        double Price = double.Parse(Console.ReadLine());

        Console.WriteLine("Enter Colour: ");
        string Colour = Console.ReadLine();

        Console.WriteLine("Enter Number of Seats: ");
        int Seats = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter Boot Space (Liters): ");
        double Boot_space = double.Parse(Console.ReadLine());

        Console.WriteLine("Enter Brand: ");
        string Brand = Console.ReadLine();

        Console.WriteLine("Enter Year: ");
        int Year = int.Parse(Console.ReadLine());

        Vehicle new_vehicle = new Vehicle(Registration, Type, Price, Colour, Seats, Boot_space, Brand, Year);
		vehicles.Add( new_vehicle );

        Console.WriteLine("Vehicle Successfully Registered");
    }
	catch (Exception)
	{

		throw;
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

			Console.WriteLine($"\nIf you wish to view details about a single car, enter its associated number - or press \"{index}\" to exit");

			string input = Console.ReadLine();
			int convertedInput = int.Parse(input);

			if (convertedInput > 0 && convertedInput < index)
			{
				Vehicle selectedVehicle = vehicles[convertedInput - 1];

				index = 1;

				Console.WriteLine("\n");
				tags();
				List<string> keys = selectedVehicle.summary_dict().Keys.ToList();

				foreach (string info in keys)
				{
					Console.WriteLine($"    {index++}) {info}: {selectedVehicle.summary_dict()[info]}");
				}


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
				Console.WriteLine($"\n**ERROR**\n    - Please enter a valid number between 1 and {index}.");
			}
		}
		catch (Exception)
		{

			throw;
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

        Console.WriteLine("Enter the name of the staff member");
		string name = Console.ReadLine();

		Staff newStaff = new Staff(name);
		staffs.Add(newStaff);

        Console.WriteLine($"Successfully added {name}");
    }
    catch (Exception)
    {

        throw;
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

    Console.WriteLine("\nPress any key to continue...");
	Console.ReadLine();
}

// Create a new customer and add to customer list
void addCustomer()
{
    try
    {
        tags();
        Console.WriteLine($"\n**Register New Customer**\n");

        Console.WriteLine("Enter the name of the customer");
        string name = Console.ReadLine();

        Console.WriteLine("Enter the home address of the customer");
		string address = Console.ReadLine();

        Console.WriteLine("Enter the email address of the customer");
		string email = Console.ReadLine();

        Console.WriteLine("Enter the phone number of the customer");
		string phone = Console.ReadLine();

		Customer newCustomer = new Customer(name, address, email, phone);
		customers.Add(newCustomer);

        Console.WriteLine($"Successfully added {name}");
    }
    catch (Exception)
    {

        throw;
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
		Console.WriteLine($"If you wish to view more in detail, enter the associated number or press \"{index}\" to exit");

		string selection = Console.ReadLine();
		int convertedSelection = int.Parse(selection);

		tags();
		if ( convertedSelection > 0 && convertedSelection < index)
		{
			Customer selectedCustomer = customers[convertedSelection - 1];
			selectedCustomer.summary();

            Console.WriteLine($"\nIf you wish to edit {selectedCustomer.Name}'s profile then press \"1\", else press any key");
			selection = Console.ReadLine();
			
			if ( selection == "1")
			{
                Console.WriteLine("Enter the name of the customer");
                selectedCustomer.Name = Console.ReadLine();

                Console.WriteLine("Enter the home address of the customer");
                selectedCustomer.Address = Console.ReadLine();

                Console.WriteLine("Enter the email address of the customer");
                selectedCustomer.Email = Console.ReadLine();

                Console.WriteLine("Enter the phone number of the customer");
                selectedCustomer.Phone = Console.ReadLine();

                Console.WriteLine($"Successfully updated {selectedCustomer.Name}");
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
		Console.WriteLine($"\nSelect a vehicle that you want to remove, or press \"{index}\" to cancel");

		string selection = Console.ReadLine();
		int convertedSelection = int.Parse(selection);

		if (convertedSelection > 0 && convertedSelection < index)
		{
			vehicles.RemoveAt(convertedSelection - 1);
			Console.WriteLine("\nVehicle successfully removed from the system");
			break;
		}
		else if (convertedSelection == index)
		{
			Console.WriteLine("\nCancelled");
			break;
		}
		else
		{
			Console.WriteLine($"\n**ERROR**\n    - Please enter a valid vehicle or press \"{index}\" to cancel");
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
        Console.WriteLine($"\nSelect a staff that you want to remove, or press \"{index}\" to cancel");

        string selection = Console.ReadLine();
        int convertedSelection = int.Parse(selection);

        if (convertedSelection > 0 && convertedSelection < index)
        {
            staffs.RemoveAt(convertedSelection - 1);
            Console.WriteLine("\nStaff member successfully removed from the system");
            break;
        }
        else if (convertedSelection == index)
        {
            Console.WriteLine("\nCancelled");
            break;
        }
        else
        {
            Console.WriteLine($"\n**ERROR**\n    - Please enter a valid staff member or press \"{index}\" to cancel");
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
        Console.WriteLine($"\nSelect a customer that you want to remove, or press \"{index}\" to cancel");

        string selection = Console.ReadLine();
        int convertedSelection = int.Parse(selection);

        if (convertedSelection > 0 && convertedSelection < index)
        {
            staffs.RemoveAt(convertedSelection - 1);
            Console.WriteLine("\nCustomer successfully removed from the system");
            break;
        }
        else if (convertedSelection == index)
        {
            Console.WriteLine("\nCancelled");
            break;
        }
        else
        {
            Console.WriteLine($"\n**ERROR**\n    - Please enter a valid customer or press \"{index}\" to cancel");
        }
    }
}

// Loop through menu dictionary and output all options to the user, wait for their input and run the associated function
while (true)
{
	try
	{
		tags();
		Console.WriteLine($"\n**MENU**\nPlease pick an option:\n");
		int index = 1;

		foreach (string item in menuItems.Keys)
		{
			Console.WriteLine($"    {index++}) {item}");
		}

		Console.WriteLine($"    {index}) Exit");

		// Take in the users selection
		string selection = Console.ReadLine();
		try
		{
			// Attempt to convert it to an integer but catch and allow the user to re-try if they make a mistake
			int intSelection = int.Parse( selection );

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
                Console.WriteLine($"\n**INFO**\nBye Bye!\n\n");
				break;
			}
			else
			{
				Console.WriteLine($"\nPlease pick a valid option between 1 and {index}");
			}
		} catch (Exception e)
		{
			throw;
		}

	} catch (Exception)
	{
		throw;
	}
}