using CshToolHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MBRando
{
    internal class Install
    {
        // Method for verifying CSH files, opening them and copying them to backup location for working on
        private static void verifyOpenAndCopy(string basepath, string cshName)
        {
            // Make backup of file. Verify the file is there first.
            string sourceCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + cshName + ".csh");
            string backupCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + cshName + "_original.csh");
            if (!File.Exists(sourceCSH))
            {
                Console.WriteLine("Cannot locate file /resource/finalizedCommon/mithril/system/csv/" + cshName + ".csh");
                Console.WriteLine("Press enter to exit installer.");
                Environment.Exit(0);
            }

            string destinationToCopyTo = Path.GetFullPath(basepath + "/MBRando/" + cshName + ".csh");
            // If original data exists already as a backup (if running the randomizer twice or more in a row), get the original data
            if (File.Exists(backupCSH))
            {
                // Create a copy locally for easy management
                File.Copy(backupCSH, destinationToCopyTo, true);
            }
            else
            {
                File.Copy(sourceCSH, backupCSH, true);
                // Create a copy locally for easy management
                File.Copy(sourceCSH, destinationToCopyTo, true);
            }
        }

        private static void copyBackAndDelete(string basepath, string name)
        {
            string sourceCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + name + ".csh");
            string csvThatWasEdited = Path.GetFullPath(basepath + "/MBRando/" + name + ".csv");
            string cshThatWasProduced = Path.GetFullPath(basepath + "/MBRando/" + name + ".csh");

            File.Copy(cshThatWasProduced, sourceCSH, true);
            File.Delete(csvThatWasEdited);
            File.Delete(cshThatWasProduced);
        }
        public static void Run(string basepath)
        {
            // Verify, open, and copy the files to the proper locations for installation
            verifyOpenAndCopy(basepath, "mirageboard_data");

            // Prompt the user for a seed name
            Console.WriteLine("Please type the value you wish to use for the seed (can leave blank),\nand press enter:\n");

            string sV = Console.ReadLine();

            //if input is blank, get current time in unix
            if (sV.Length == 0)
            {
                sV = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            }

            // Write to txt file with seed name in it for reference
            System.IO.File.WriteAllText(basepath + "/MBRando/logs/seed.txt", sV);

            // Run the WoFFCshTool by Surihia twice, one to decompress csh and convert to csv, and one to do the reverse after writing the values
            ConversionHelpers.ConvertToCsv(Path.Combine(basepath, "MBRando", "mirageboard_data.csh"));

            // Write the values
            Mirageboard.mirageboard_dataWriteCsv(basepath, sV);

            // Second WoFFCshTool run
            ConversionHelpers.ConvertToCsh(Path.Combine(basepath, "MBRando", "mirageboard_data.csv"));

            copyBackAndDelete(basepath, "mirageboard_data");

            Console.WriteLine("\nFinished generating! Press enter to exit.\n");
            Console.ReadLine();

            Environment.Exit(0);
        }
    }
}
