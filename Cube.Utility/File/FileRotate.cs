using System.Xml.Linq;

namespace Cube.Utility;

public class FileRotate
{
    public FileRotate()
    {
        //TODO
    }

    public void FileRotateType1(string logDirectory, string logFile, int maxBackupFiles)
    {
        
        if (string.IsNullOrWhiteSpace(logDirectory))
            throw new ArgumentNullException(nameof(logDirectory), $"{nameof(logDirectory)} cannot be null");
        if (string.IsNullOrWhiteSpace(logFile))
            throw new ArgumentNullException(nameof(logFile), $"{nameof(logFile)} cannot be null");
        if (maxBackupFiles <= 0)
            throw new ArgumentNullException(nameof(maxBackupFiles), $"{nameof(maxBackupFiles)} cannot be less than 0");

        // Get the full path of the log file
        string logFilePath = Path.Combine(logDirectory, logFile);
        string backupFilePath = string.Empty;

        try
        {
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }        
    }

    public void FileRotateType2(string logDirectory, string logFile, int maxBackupFiles)
    {
        if (string.IsNullOrWhiteSpace(logDirectory))
            throw new ArgumentNullException(nameof(logDirectory), $"{nameof(logDirectory)} cannot be null");
        if (string.IsNullOrWhiteSpace(logFile))
            throw new ArgumentNullException(nameof(logFile), $"{nameof(logFile)} cannot be null");
        if (maxBackupFiles <= 0)
            throw new ArgumentNullException(nameof(maxBackupFiles), $"{nameof(maxBackupFiles)} cannot be less than 0");

        // Get the full path of the log file
        string logFilePath = Path.Combine(logDirectory, logFile);
        string backupFilePath = string.Empty;

        try
        {
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task FileRotateAsync(string logDirectory, string logFile, int maxBackupFiles)
    {
        if (string.IsNullOrWhiteSpace(logDirectory))
            throw new ArgumentNullException(nameof(logDirectory), $"{nameof(logDirectory)} cannot be null");
        if (string.IsNullOrWhiteSpace(logFile))
            throw new ArgumentNullException(nameof(logFile), $"{nameof(logFile)} cannot be null");
        if (maxBackupFiles <= 0)
            throw new ArgumentNullException(nameof(maxBackupFiles), $"{nameof(maxBackupFiles)} cannot be less than 0");

        // Get the full path of the log file
        string logFilePath = Path.Combine(logDirectory, logFile);
        string backupFilePath = string.Empty;

        try
        {
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
