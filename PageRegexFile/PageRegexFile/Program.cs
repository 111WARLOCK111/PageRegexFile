using System;

using System.IO;

using System.Xml;

using System.Net;

using System.Text;
using System.Text.RegularExpressions;

using System.Collections.Generic;

namespace PageRegexFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the web page (NOTE: THIS MUST HAS HTTP or HTTPS):");
            Console.WriteLine();

            GetData();
        }

        static void GetData()
        {
            var web = Console.ReadLine();

            byte[] row;

            try
            {
                using (var db = new WebClient())
                {
                    row = db.DownloadData(web);
                }
            }
            catch
            {
                Console.WriteLine("Web page was incorrect. Please enter the web page again:");
                GetData();
                return;
            }

            if (row == null)
            {
                Console.WriteLine("There was no data received from the web page. Please enter a new page:");
                GetData();
                return;
            }

            var ut = new UTF8Encoding();

            var page = ut.GetString(row);

            if (page == null)
            {
                Console.WriteLine("There was no data received from the web page. Please enter a new page:");
                GetData();
                return;
            }

            Console.WriteLine("Data sucessfully received from the server.");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Please enter a valid regex string in order to get the data:");

            Console.WriteLine();

            var rgx = Console.ReadLine();

            var regex = new Regex(rgx);

            var match = regex.Matches(page);

            var print = false;

            var file = "default.txt";

            if (match.Count > 0)
            {
                Console.WriteLine("There are " + match.Count + " matches that were found, Would you like to see them in console?");

                Console.WriteLine();

                var read = Console.ReadKey(true);

                if (read.Key == ConsoleKey.Enter || read.Key == ConsoleKey.Y)
                    print = true;
                else
                    print = false;

                Console.WriteLine();

                Console.WriteLine("Alright, Enter a name for file you're looking to write them:");

                Console.WriteLine("NOTE: This removes any existing file, so watch out!!");

                Console.WriteLine();

                file = Console.ReadLine();
            }

            using (var sw = new StreamWriter(file))
            {
                foreach (var m in match)
                {
                    sw.WriteLine(m);
                    if (!print) continue;
                    Console.WriteLine(m);
                }
            }

            Console.WriteLine("Finished! Check the existing file called " + file + " ;)");

            Console.WriteLine("If you're not going to continue, just close this application.");

            Console.WriteLine("Enter the web page (NOTE: THIS MUST HAS HTTP or HTTPS):");

            Console.WriteLine();
            Console.WriteLine();

            GetData();
        }
    }
}
