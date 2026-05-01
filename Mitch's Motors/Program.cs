using Mitch_s_Motors;
using System.Collections;

// Initialise lists to store all user-created objects in memory to be called and used in the menu
List<Vehicle> vehicles = new List<Vehicle>();
List<Customer> customer = new List<Customer>();
List<Staff> staffs = new List<Staff>();
List<Booking> bookings = new List<Booking>();


// Dictionary to store all menu options and their attached functions
Dictionary<string, Action> menuItems = new Dictionary<string, Action>
{
	{ "Register New Vehicle", addVehicle },
	{ "View All Vehicles", viewVehicles }
};


// Functions
// Create a new vehicle and save it in the vehicles list
void addVehicle()
{
	try
	{
        Console.WriteLine($"\n**Register New Vehicle**\n ");
	}
	catch (Exception)
	{

		throw;
	}
}

// Loop through and output all vehicles on the vehicles list
void viewVehicles()
{
	try
	{
        Console.WriteLine($"\n**All Vehicles**\n");
        
	}
	catch (Exception)
	{

		throw;
	}
}



// Loop through menu dictionary and output all options to the user, wait for their input and run the associated function
while (true)
{
	try
	{
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
			if (intSelection > 0 && intSelection < index - 1)
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