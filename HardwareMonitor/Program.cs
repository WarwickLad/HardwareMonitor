using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

public class HardwareMonitor
{
    public PerformanceCounter cpuCounter;
    public PerformanceCounter ramCounter;

    public void setCurrentCpuUsage()
    {
        this.cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    }
    public void setramCounter()
    {
        this.ramCounter = new PerformanceCounter("Memory", "Available MBytes");
    }

    public string getCurrentCpuUsage()
    {
        return cpuCounter.NextValue() + "%";
    }

    public string getAvailableRAM()
    {
        return ramCounter.NextValue() + "MB";
    }

    private static void RunTest()
    {
        HardwareMonitor hardwareMonitorInstance = new HardwareMonitor();
        String consoleOutput;
        while (true)
        {
            hardwareMonitorInstance.setramCounter();
            consoleOutput = hardwareMonitorInstance.getAvailableRAM();
            Console.WriteLine(consoleOutput);
            Console.ReadLine();
        }
        
    }

    static void Main()
    {
        RunTest();
    }
}
