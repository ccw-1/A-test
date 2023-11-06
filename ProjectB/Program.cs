using System;
using System.IO;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        string logFile = "output.log";
        string lastOutput = null;

        while (true)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c query session /mode /flow /connect /counter /vm");
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;

            Process p = Process.Start(psi);
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            if (output != lastOutput)
            {
                lastOutput = output;
                // if the output changes, record
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ":\n" + output);
                }
            }

            System.Threading.Thread.Sleep(30000); // <--- poll every 30 secs
        }
    }
}

