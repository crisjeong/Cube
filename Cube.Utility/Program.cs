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
            BackupRotate_1(logDirectory, "test_a.log", maxBackupFiles);
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
            BackupRotate_2(logDirectory, "test_b.log", maxBackupFiles);
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
            await BackupRotateAsync(logDirectory, logFile, maxBackupFiles);
        }        
        sw.Stop();
        Console.WriteLine($"End : Time (ElapsedMilliseconds) : {sw.ElapsedMilliseconds}");

#endif

    }

    static async Task BackupRotateAsync(string logDirectory, string logFile, int maxBackupFiles)
    {
        // Get the full path of the log file
        string logFilePath = Path.Combine(logDirectory, logFile);
        string backupFilePath = string.Empty;

        // Check if the log file exists and is larger than the maximum file size
        if (File.Exists(logFilePath))
        {
            // Rename the current log file
            for (int i = maxBackupFiles - 1; i >= 0; i--)
            {
                backupFilePath = Path.Combine(logDirectory, $"{logFile}.{i}");
                if (File.Exists(backupFilePath))
                {
                    if (i == maxBackupFiles - 1)
                    {
                        //File.Delete(backupFilePath);
                        await Task.Run(() => File.Delete(backupFilePath));
                    }
                    else
                    {
                        string newBackupFilePath = Path.Combine(logDirectory, $"{logFile}.{i + 1}");
                        //File.Move(backupFilePath, newBackupFilePath);
                        await Task.Run(() => File.Move(backupFilePath, newBackupFilePath));
                    }
                }
            }

            backupFilePath = Path.Combine(logDirectory, $"{logFile}.0");
            //File.Move(logFilePath, backupFilePath);
            await Task.Run(() => File.Move(logFilePath, backupFilePath));
        }

        // Create or append to the log file
        using (StreamWriter sw = File.AppendText(logFilePath))
        {
            //sw.WriteLine($"[{DateTime.Now.ToString()}] This is a log message.");
            await sw.WriteLineAsync($"[{DateTime.Now.ToString()}] This is a log message.");
        }
    }

    //private static async Task RenameBackupFilesAsync(string logDirectory, string logFile, int maxBackupFiles)
    //{
    //    var backupFilePaths = Enumerable.Range(0, maxBackupFiles - 1)
    //                                    .Select(i => Path.Combine(logDirectory, $"{logFile}.{i}"))
    //                                    .OrderByDescending(path => path)
    //                                    .Skip(1);

    //    foreach (var backupFilePath in backupFilePaths)
    //    {
    //        string newBackupFilePath = Path.Combine(logDirectory, $"{logFile}.{int.Parse(Path.GetExtension(backupFilePath).TrimStart('.')) + 1}");
    //        await MoveLogFileAsync(backupFilePath, newBackupFilePath);
    //    }

    //    string lastBackupFilePath = Path.Combine(logDirectory, $"{logFile}.{maxBackupFiles - 1}");
    //    if (File.Exists(lastBackupFilePath))
    //    {
    //        await Task.Run(() => File.Delete(lastBackupFilePath));
    //    }
    //}

    //private static async Task MoveLogFileAsync(string sourceFilePath, string destFilePath)
    //{
    //    await Task.Run(() => File.Move(sourceFilePath, destFilePath));
    //}

    static void BackupRotate_1(string logDirectory, string logFile, int maxBackupFiles)
    {
        // Get the full path of the log file
        string logFilePath = Path.Combine(logDirectory, logFile);
        string backupFilePath = string.Empty;

        // Check if the log file exists
        if (File.Exists(logFilePath))
        {
            // Rename the current log file
            for (int i = maxBackupFiles - 2; i >= 0; i--)
            {
                backupFilePath = Path.Combine(logDirectory, $"{logFile}.{i}");
                if (File.Exists(backupFilePath))
                {
                    string newBackupFilePath = Path.Combine(logDirectory, $"{logFile}.{i + 1}");
                    File.Move(backupFilePath, newBackupFilePath, true);
                }
            }
            
            string lastBackupFilePath = Path.Combine(logDirectory, $"{logFile}.{maxBackupFiles - 1}");
            if (File.Exists(lastBackupFilePath))
            {
                File.Delete(lastBackupFilePath);
            }
            
            backupFilePath = Path.Combine(logDirectory, $"{logFile}.0");
            File.Move(logFilePath, backupFilePath, true);
        }

        // Create or append to the log file
        using (StreamWriter sw = File.AppendText(logFilePath))
        {
            sw.WriteLine($"[{DateTime.Now.ToString()}] This is a log message.");
        }
    }

    static void BackupRotate_2(string logDirectory, string logFile, int maxBackupFiles)
    {
        // Get the full path of the log file
        string logFilePath = Path.Combine(logDirectory, logFile);
        string backupFilePath = string.Empty;

        // Check if the log file exists and is larger than the maximum file size
        if (File.Exists(logFilePath))
        {
            // Rename the current log file
            for (int i = maxBackupFiles - 1; i >= 0; i--)
            {
                backupFilePath = Path.Combine(logDirectory, $"{logFile}.{i}");
                if (File.Exists(backupFilePath))
                {
                    if (i == maxBackupFiles - 1)
                    {
                        File.Delete(backupFilePath);
                    }
                    else
                    {
                        string newBackupFilePath = Path.Combine(logDirectory, $"{logFile}.{i + 1}");
                        File.Move(backupFilePath, newBackupFilePath, true);
                    }
                }
            }

            backupFilePath = Path.Combine(logDirectory, $"{logFile}.0");
            File.Move(logFilePath, backupFilePath, true);
        }

        // Create or append to the log file
        using (StreamWriter sw = File.AppendText(logFilePath))
        {
            sw.WriteLine($"[{DateTime.Now.ToString()}] This is a log message.");
        }
    }

}