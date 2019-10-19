using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ControlWork4
{
    public class Program
    {
        private static object _loker = new object();

        public static void Main(string[] args)
        {
            File.Create("1.txt").Dispose();
            FileWatcher fileWatcher = new FileWatcher("1.txt");
            fileWatcher.Change += Change;
            Task.Run(() => fileWatcher.Start());

            while (true)
            {
                Console.WriteLine("Do you want 1?");
                string answer = Console.ReadLine();

                if (answer.ToLower() == "yes")
                {
                    lock (_loker)
                    {
                        File.WriteAllText("1.txt", "1");
                    }
                }
            }
        }

        private static void Change(DateTime changeTime)
        {
            Console.WriteLine($"Last ghange {changeTime}");
            string fileContent = string.Empty;

            lock (_loker)
            {
                fileContent = File.ReadAllText("1.txt");
            }

            Console.WriteLine($"File content {fileContent}");

            if (fileContent == "1")
            {
                lock (_loker)
                {
                    File.WriteAllText("1.txt", "0");
                }

                Thread.Sleep(10000);
                Console.WriteLine("File content changed");
            }
        }
    }
}
