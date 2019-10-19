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
            FileWatcher fileWatcher = new FileWatcher("1.txt", _loker);
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

        private static void Change()
        {
            Console.WriteLine("Call event");
            string str = string.Empty;

            lock (_loker)
            {
                str = File.ReadAllText("1.txt");
            }

            if (str == "1")
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
