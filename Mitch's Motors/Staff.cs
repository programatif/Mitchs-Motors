using System;
using System.Collections.Generic;
using System.Text;

namespace Mitch_s_Motors
{
    internal class Staff
    {
        // Attributes
        public string Name {  get; set; }
        public bool Currently_available { get; set; } = true;

        // Constructor
        public Staff (string name)
        {
            Name = name;
        }

        // Behaviours

        // Output a full summary of everything in the class
        public void summary()
        {
            Console.WriteLine($"\n**Staff**\n    - Name: {Name}\n    - Available Now: {Currently_available}\n\n");
        }
    }
}
