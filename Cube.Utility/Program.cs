#define V_1
using System.Diagnostics;

namespace Cube.Utility;

public class Program
{
    static async Task Main(string[] args)
    {
        string logDirectory = Environment.CurrentDirectory;
        string logFile = "test_a.log";        
        int maxBackupFiles = 10;
        int loopLimit = 10;

        // Check if log directory exists
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

#if V_1
        Console.WriteLine("Start");
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < loopLimit; i++)
        {
            new FileRotate()
                .FileRotateType1(logDirectory, logFile, maxBackupFiles);
        }
        sw.Stop();
        Console.WriteLine($"End : Time (ElapsedMilliseconds) : {sw.ElapsedMilliseconds}");
        sw.Reset();

#elif V_2

        Console.WriteLine("Start");
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        for (int i = 0; i < loopLimit; i++)
        {
            new FileRotate()
                .FileRotateType2(logDirectory, logFile, maxBackupFiles);
        }
        sw.Stop();
        Console.WriteLine($"End : Time (ElapsedMilliseconds) : {sw.ElapsedMilliseconds}");
        sw.Reset();

#elif V_3

        Console.WriteLine("Start");
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        for (int i = 0; i < loopLimit; i++)
        {            
            await new FileRotate()
                .FileRotateAsync(logDirectory, logFile, maxBackupFiles);
        }        
        sw.Stop();
        Console.WriteLine($"End : Time (ElapsedMilliseconds) : {sw.ElapsedMilliseconds}");

#endif

    }

}