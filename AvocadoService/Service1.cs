using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace AvocadoService
{
    public partial class Service1 : ServiceBase
    {
        private EventLog eventLog;
        private System.Timers.Timer timer;
        private Process child;
        private Stopwatch sw;
        private int lastChildPid = -1;

        public Service1()
        {
            InitializeComponent();
            this.ServiceName = "AvocadoService";
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.eventLog = new EventLog();
            this.eventLog.Source = this.ServiceName;
            this.eventLog.Log = "Application";
            // Create event source if it doesn't exist
            if (!EventLog.SourceExists(this.eventLog.Source))
            {
                EventLog.CreateEventSource(this.eventLog.Source, this.eventLog.Log);
            }
            // Initialize timer
            this.timer = new System.Timers.Timer();
            this.timer.Interval = 5000; // 10 seconds 
            this.timer.Elapsed += TimerElapsed;

        }

        protected override void OnStart(string[] args)
        {
            string startArgs = string.Join(", ", args); // Combine the arguments
            this.eventLog.WriteEntry($"Avocado service started with arguments: {startArgs}", EventLogEntryType.Information);
            this.timer.Start(); // Start the timer
            this.child = Respawn();
            this.sw = new Stopwatch();
        }

        protected override void OnStop()
        {
            this.child.Kill();
            this.eventLog.WriteEntry("Avocado service stopped.", EventLogEntryType.Information);
            this.timer.Stop(); // Stop the timer
        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Business logic here
            try
            {
                this.child = Process.GetProcessById(this.child.Id);
                if (this.child.HasExited)
                {
                    this.eventLog.WriteEntry("Child has exited, respawn.", EventLogEntryType.Information);
                    this.child = Respawn();
                    this.lastChildPid = -1;
                    this.sw.Restart();
                }
                else if (this.child.Id != this.lastChildPid)
                {
                    this.eventLog.WriteEntry("Child pid " + this.child.Id + " is alive.", EventLogEntryType.Information);
                    this.lastChildPid = this.child.Id;
                }
            }
            catch (ArgumentException)
            {
                this.eventLog.WriteEntry("Exception: Child has died, respawn.", EventLogEntryType.Information);
                this.child = Respawn();
                this.lastChildPid = -1;
                this.sw.Restart();
            }
            if (sw.ElapsedMilliseconds > 61000) // <=== change this for a different interval
            { // kill if it runs more than 61 seconds 
                this.eventLog.WriteEntry("Child has been running for too long, kill.", EventLogEntryType.Information);
                this.child.Kill();
            }
        }
        private static Process Respawn()
        {
            Process p = Process.Start(@"C:\\tools\\sleep.exe", "600"); // <--- change this for a different program 
            return p;
        }
    }
}
