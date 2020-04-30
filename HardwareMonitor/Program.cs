using System;
using System.Diagnostics;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Threading.Tasks;

namespace HardwareMonitor
{
    public class HardwareMonitor
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUWest1;
        private static IAmazonS3 s3Client;

        private static async Task UploadFileAsync(string filePath, string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);
                // Option 1. Upload a file. The file name is used as the object key name.
                await fileTransferUtility.UploadAsync(filePath, bucketName);
                Console.WriteLine("Upload to S3 completed");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

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

            //Initiliase string array to write to console and text file
            String[] textOutput = new string[9];
            string textLine = "";

            //Initialise loop counter to track file write/API push cycles
            int loopCounter = 0;
            int loopTotal = 6;

            //Initialise text file and clear previous entries
            File.WriteAllText(@"C:\Users\Zachary\Documents\Visual Studio 2017\Projects\HardwareMonitor\HardwareMonitor\WriteLines.txt", String.Empty);

            string bucketNameS3 = "hardware-monitor-staging";

            while (true)
            {
                //Clear text file after loop counter reachers cycle length
                if(loopCounter == loopTotal)
                {
                    loopCounter = 0;
                    UploadFileAsync(@"C:\Users\Zachary\Documents\Visual Studio 2017\Projects\HardwareMonitor\HardwareMonitor\WriteLines.txt", bucketNameS3).Wait();
                    File.WriteAllText(@"C:\Users\Zachary\Documents\Visual Studio 2017\Projects\HardwareMonitor\HardwareMonitor\WriteLines.txt", String.Empty);
                }
                loopCounter++;

                //Timestamps updated
                dateTime = DateTime.UtcNow;
                unixTime = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();

                //Writing to console
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

                //Writing to file
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Zachary\Documents\Visual Studio 2017\Projects\HardwareMonitor\HardwareMonitor\WriteLines.txt", true))
                {
                    foreach (string line in textOutput)
                    {
                        file.WriteLine(line);
                    }
                }

                //Thread sleeps for 10s
                System.Threading.Thread.Sleep(10000);
            }
        }

        public static void Main()
        {
            s3Client = new AmazonS3Client(bucketRegion);

            //Main code executed
            ReadValuesOfPerformanceCounters();
        }
    }
}