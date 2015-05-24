using System;
using System.Globalization;
using System.Threading;

namespace AssignmentOne
    {
    class ShellHeighDistance
        {
        static void Main()
            {
            // the welcome messages for user friendly interface
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Soldier!");
            Console.WriteLine();
            Console.WriteLine("Prepare for battle!");
            Console.WriteLine();
            Console.WriteLine("Let's calculate the maximum height of the shell of your tank\nand the distance it will travel along the ground...");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Provide the initial firing angle: ");

            //as we are students from all over the world an unification of the culture is a good idea
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            // initial angle we ask the user to provide
            float theta = float.Parse(Console.ReadLine());

            // converting to radians using the simple formula  Math.PI * degrees / 180.0;
            double radians = (Math.PI * theta) / 180.0;

            // initial speed we ask the user to provide
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Now, provide the initial speed: ");
            float speed = float.Parse(Console.ReadLine());

            // declaring the second
            const int second = 60;

            // acceleration due to gravity
            const double acceleration = 9.8d / second * second;

            // x component of the velocity at start
            float vox = speed * (float)Math.Cos(radians);

            // y component of the velocity at start
            float voy = speed * (float)Math.Sin(radians);

            // time until shell reaches apex
            double time = voy / acceleration;


            // height of shell at apex
            double height = voy * voy / (2 * acceleration);

            // distance shell travels horizontally (assuming launch and target elevations are equal)
            double distance = vox * 2 * time;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Well done! The height of the shell is {0:F3}!", height);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The distance of the shell is {0:F3}!", distance);
            Console.WriteLine();
            }
        }
    }
