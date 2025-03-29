
using System.Drawing.Text;
using LocateFile;

namespace MBRando
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("#################################################################");
            Console.WriteLine("##                  Mirage Board Randomizer                    ##");
            Console.WriteLine("#################################################################\n");
            Console.WriteLine("Welcome to the Mirage board randomizer. This randomizer shuffles");
            Console.WriteLine("most every node in the game. It is highly recommended that you");
            Console.WriteLine("make a backup of your save before continuing.\n---\n");
            // Let the user know that a window will open to find WOFF.exe
            Console.WriteLine("To get started, you will need to locate the executable for WOFF, or WOFF.exe.");
            Console.WriteLine("You can find it by right-clicking the game in Steam and going to");
            Console.WriteLine("Manage -> Browse local files.... Copy the filepath including exe \nfor this step.\n");
            Console.WriteLine("Press enter when you're ready.");
            Console.ReadLine();

            string WOFFfilepath = SharedMethods.SetPath("");

            if (!WOFFfilepath.EndsWith("WOFF.exe"))
            {
                Console.WriteLine("Please specify WOFF.exe file. Exiting. Please try again.");
                System.Threading.Thread.Sleep(2000);
                Environment.Exit(0);
            }

            Console.WriteLine("Success!\n");

            string basepath = Path.GetDirectoryName(WOFFfilepath);
            Console.WriteLine(basepath);
            while (true)
            {               
                Console.WriteLine("Please type one of the following numbers and press enter:\n");
                Console.WriteLine("[1] Install");
                Console.WriteLine("[2] Uninstall");
                Console.WriteLine("[3] Exit\n");

                string option = Console.ReadLine();

                if (option == "1") Install.Run(basepath);
                else if (option == "2") Uninstall.Run(basepath);
                else if (option == "3") Environment.Exit(0);
                Console.Clear();
            }            
        }
    }
    class SharedMethods
    {
        public static string SetPath(string fileToFind)
        {
            var foundPath = "";
            var t = new Thread(() =>
            {
                foundPath = Locator.FindFilePath(fileToFind);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return foundPath;
        }

    }
}

