using System.Diagnostics;


namespace ProcessManager.models
{
    public class ProcessInfo
    {
        public string ProcessName { get; set; } = null;
        public string FilePath { get; set; } = null;
        public Process Process { get; set; } = null;

        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        public ProcessStatus Status { get; set; }
    }

    public enum ProcessStatus
    {
        Running, Pending, Finished, Failed
    }
}
