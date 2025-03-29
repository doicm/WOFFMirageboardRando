using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBRando
{
    public class Uninstall
    {
        private static (string, string) defineAndCheckFile(string basepath, string name)
        {
            string sourceCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + name + ".csh");
            string backupCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + name + "_original.csh");

            if (!File.Exists(backupCSH))
            {
                Console.WriteLine("Cannot locate file /resource/finalizedCommon/mithril/system/csv/" + name + "_original.csh.");
                Console.WriteLine("It appears the randomizer may already be uninstalled. If you are still having issues, \ngo to Steam and verify your files.");
                Console.WriteLine("\nPress enter to exit.\n");
                Console.ReadLine();
                Environment.Exit(0);
            }
            return (sourceCSH, backupCSH);
        }

        private static void copyAndRemoveFile(string source, string backup)
        {
            File.Copy(backup, source, true);
            File.Delete(backup);
        }

        private static void clearLogs(string basepath)
        {
            System.IO.File.WriteAllText(basepath + "/MBRando/logs/seed.txt", "");
        }
        public static void Run(string basepath)
        {
            (string sourceMBCSH, string backupMBCSH) = defineAndCheckFile(basepath, "mirageboard_data");

            Console.WriteLine("This will uninstall the mirageboard randomizer.\nPress enter to uninstall.\n");
            Console.ReadLine();

            copyAndRemoveFile(sourceMBCSH, backupMBCSH);
            clearLogs(basepath);

            Console.WriteLine("\nUninstallation complete. Press enter to exit.\n");
            Console.ReadLine();
            
            Environment.Exit(0);
        }
    }
}
