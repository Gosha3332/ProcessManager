using ProcessManager.models;
using System.Diagnostics;

string logsDir = "";
string[] worcersPath = new string[] { };

if (!Directory.Exists(logsDir))
{
    Console.WriteLine($"Папка не найдена: {logsDir}");
    return;
}

string[] logFiles = Directory.GetFiles(logsDir, "*.log");
string[] mode = new string[] { "fast", "slow", "slow", "fast", "slow" };

List<ProcessInfo> processes = new List<ProcessInfo>();

for (int i = 0; i < logFiles.Length; i++)
{
    ProcessStartInfo startInfo = new ProcessStartInfo
    {
        FileName = worcersPath[i],
        UseShellExecute = false,
        Arguments = $"{logFiles[i]} 5 {mode[i]}"
    };

    processes.Add(new ProcessInfo
    {
        ProcessName = worcersPath[i],
        FilePath = logFiles[i],
        Process = new Process { StartInfo = startInfo },
        Status = ProcessStatus.Pending
    });
}

Console.WriteLine("Выберите сценарий:\n1 — дождатся\n2 — прервать");
string command = Console.ReadLine();

void RunProcess(ProcessInfo p)
{
    try
    {
        p.Process.Start();
        p.StartAt = DateTime.Now;
        p.Status = ProcessStatus.Running;
        Console.WriteLine($"{p.ProcessName} запущен, id: {p.Process.Id}");

        p.Process.WaitForExit();

        p.EndAt = DateTime.Now;
        p.Status = ProcessStatus.Finished;
        Console.WriteLine($"{p.ProcessName} завершён, exit code: {p.Process.ExitCode}");
    }
    catch (Exception ex)
    {
        p.Status = ProcessStatus.Failed;
        Console.WriteLine($"{p.ProcessName}: ошибка — {ex.Message}");
    }
}


List<Task> tasks = new List<Task>();
foreach (var p in processes)
{
    tasks.Add(Task.Run(() => RunProcess(p)));
}




if (command == "2")
{
    await Task.Delay(1000);
    foreach (var p in processes)
    {
        try
        {
            if (p.Status == ProcessStatus.Running)
                p.Process.Kill();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{p.ProcessName}: не удалось остановить — {ex.Message}");
        }
    }
}


await Task.WhenAll(tasks);

foreach (var p in processes)
{
    Console.WriteLine($"{p.ProcessName}: {p.Status}");
}
Console.WriteLine("Work finished");


