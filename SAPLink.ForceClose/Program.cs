
using System.Diagnostics;

TryKillProcess(KillSapLinkProcesses);


void TryKillProcess(ActionDelegate action)
{
    try
    {
        action();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}

void KillSapLinkProcesses()
{
    foreach (Process process in Process.GetProcessesByName("SAPLink"))
    {
        process.Kill();
    }
}

public delegate void ActionDelegate();