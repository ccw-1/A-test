#define EVENT_LOG

using System;
using System.Diagnostics;
using System.Threading;

public class Program
{
	public static void Main()
	{
		using (var mutex = new Mutex(false, "bc4fc9fc-76da-11ee-bfbe-00155d27750a")) // <-- random and unique string, we use uuid here
		{
			
			bool another = !mutex.WaitOne(TimeSpan.Zero);
			if (another)
			{
				WriteLog("Only one instance of this app is allowed.");
				return;
			}
			// just one copy should be running
			singleton();
			mutex.ReleaseMutex();
		}
	}
	private static void singleton()
	{
		Process child = Respawn();
		Stopwatch sw = new Stopwatch();
		sw.Start();
		int lastChildPid = -1;
		while (true)
		{
			try
			{
				child = Process.GetProcessById(child.Id);
				if (child.HasExited)
				{
					WriteLog("Child has exited, respawn");
					child = Respawn();
					lastChildPid = -1;
					sw.Restart();
				}
				else if (child.Id != lastChildPid)
				{
					WriteLog("child pid " + child.Id + " is alive");
					lastChildPid = child.Id ;
				}
			}
			catch (ArgumentException)
			{
				WriteLog("Exception: Child has died, respawn");

				child = Respawn();
				lastChildPid = -1;
				sw.Restart();
			}
			if (sw.ElapsedMilliseconds > 30000)
			{ // kill if it runs more than 30 seconds
				WriteLog("Child has run for too long, kill ");
				child.Kill();
				Thread.Sleep(1000); // wait 1 sec
			}
			else
			{
				Thread.Sleep(5000);
			}
		}
	}

	private static void WriteLog(string msg)
	{
#if EVENT_LOG
        EventLog.WriteEntry("Application", msg);
#else
        Console.WriteLine(msg);
#endif
	}
		 
	private static Process Respawn()
	{
		Process p = Process.Start(@"..\\Sleep\\sleep.exe", "600"); // <--- this is the path of the program to be monitored and restart
		return p;
	}
}
