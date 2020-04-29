using System;
using System.Diagnostics;
using System.IO;

namespace HardwareMonitor
{
    public class HardwareMonitor
    {
        private static void ReadValuesOfPerformanceCounters()
        {
            //Initialise performance counter objects
            PerformanceCounter processorTimeCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter memoryUsage = new PerformanceCounter("Memory", "Available MBytes");
            PerformanceCounter memoryPercentage = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            PerformanceCounter processorC1 = new PerformanceCounter("Processor", "% C1 Time", "_Total");
            PerformanceCounter processorC2 = new PerformanceCounter("Processor", "% C2 Time", "_Total");
            PerformanceCounter processorC3 = new PerformanceCounter("Processor", "% C3 Time", "_Total");

            //Initialise timestampts (date/time and unix time formats)
            DateTime dateTime = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();

            String[] textOutput = new string[9];
            string textLine = "";

            while (true)
            {
                //Timestamps updated
                dateTime = DateTime.UtcNow;
                unixTime = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();

                /*

                //Performance counter values are updated
                Console.WriteLine("Unix timestamp: {0}", unixTime.ToString());
                Console.Write("Date: {0}", dateTime);
                Console.WriteLine("");
                Console.WriteLine("CPU value: {0}", processorTimeCounter.NextValue());
                Console.Write("Memory value: {0}", memoryUsage.NextValue());
                Console.WriteLine(" Memory usage: {0}", memoryPercentage.NextValue());
                Console.Write("C1 time: {0}", processorC1.NextValue());
                Console.Write(" C2 time: {0}", processorC2.NextValue());
                Console.WriteLine(" C3 time: {0}", processorC3.NextValue());
                Console.WriteLine("------------------------------");

                */

                textLine = unixTime.ToString();
                textOutput[0] = textLine;
                Console.WriteLine(textLine);

                textLine = dateTime.ToString();
                textOutput[1] = textLine;
                Console.WriteLine(textLine);

                textLine = processorTimeCounter.NextValue().ToString();
                textOutput[2] = textLine;
                Console.WriteLine(textLine);

                textLine = memoryUsage.NextValue().ToString();
                textOutput[3] = textLine;
                Console.WriteLine(textLine);

                textLine = memoryPercentage.NextValue().ToString();
                textOutput[4] = textLine;
                Console.WriteLine(textLine);

                textLine = processorC1.NextValue().ToString();
                textOutput[5] = textLine;
                Console.WriteLine(textLine);

                textLine = processorC2.NextValue().ToString();
                textOutput[6] = textLine;
                Console.WriteLine(textLine);

                textLine = processorC3.NextValue().ToString();
                textOutput[7] = textLine;
                Console.WriteLine(textLine);

                textOutput[8] = "";

                Console.WriteLine("-----------------------------");

                /*

                textOutput[0] = dateTime.ToString();
                textOutput[1] = unixTime.ToString();
                textOutput[2] = processorTimeCounter.NextValue().ToString();
                textOutput[3] = memoryUsage.NextValue().ToString();
                textOutput[4] = memoryPercentage.NextValue().ToString();
                textOutput[5] = processorC1.NextValue().ToString();
                textOutput[6] = processorC2.NextValue().ToString();
                textOutput[7] = processorC3.NextValue().ToString();
                textOutput[8] = "";

                */

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Zachary\Documents\Visual Studio 2017\Projects\HardwareMonitor\HardwareMonitor\WriteLines.txt", true))
                {
                    foreach (string line in textOutput)
                    {
                        file.WriteLine(line);
                    }
                }

                //Thread sleeps for 10s
                System.Threading.Thread.Sleep(20000);
            }
        }

        public static void Main()
        {
            ReadValuesOfPerformanceCounters();
        }
    }
}