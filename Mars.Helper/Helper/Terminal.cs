using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mars
{
    public static class Terminal
    {
        public static string password = "12345678";
        private static List<(string, Process)> _activeProcesses = new List<(string, Process)>();

        public static string ReadTerminal(Process proc)
        {
            string result = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            proc.Kill();
            return result;
        }
        
        public static void SendTerminal(string command, bool hidden = true)
        {
            Logging.Log.Information(command);
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    //Arguments = $"-c \" echo -e '{password}\n' | {command} \"",
                    Arguments = $"-c \"{command}\" ",
                    RedirectStandardOutput = false,
                    UseShellExecute = true,
                };

                Process proc = new Process()
                {
                    StartInfo = startInfo,
                    EnableRaisingEvents = true,
                };

                KillDuplicateProcess(command);
                _activeProcesses.Add((command, proc));

                proc.Exited += new EventHandler(Exit);
                proc.Start();

                Logging.Log.Information($"Process Start: {command}");
            }
            catch (Exception e)
            {
                Logging.Log.Error($"{command}-> {e.Message}");
                throw;
            }
        }

        private static void Exit(object? sender, System.EventArgs e)
        {
            if(sender is not null)
            {
                var item = (Process)sender;
                Logging.Log.Information(
                    $"Exit process id:[{item.Id}]:\n" +
                    $"Exit time    : {item.ExitTime}\n" +
                    $"Exit code    : {item.ExitCode}\n");

                _activeProcesses.Remove(_activeProcesses.Where(x => x.Item2.Id == item.Id).FirstOrDefault());
            }
        }

        private static void KillDuplicateProcess(string command)
        {
            foreach (var item in _activeProcesses.ToList())
            {
                if (item.Item1 == command)
                {
                    item.Item2.Kill(true);
                    item.Item2.Dispose();
                }
            }
        }

        public static void KillProcess(string command)
        {
            var item = _activeProcesses.Where(x => x.Item1 == command).FirstOrDefault();
            if(item.Item2 is not null)
            {
                item.Item2.Kill(true);
                item.Item2.Dispose();
            }
        }
    }
}