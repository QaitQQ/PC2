using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LoaderMGSol
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        ProcessStartInfo start = new ProcessStartInfo();
        
        //start.Arguments = arguments;
// Enter the executable to run, including the complete path
start.FileName = ExeName;
// Do you want to show a console window?
start.WindowStyle = ProcessWindowStyle.Hidden;
start.CreateNoWindow = true;
int exitCode;
// Run the external process & wait for it to finish
using (Process proc = Process.Start(start))
{
     proc.WaitForExit();
     // Retrieve the app's exit code
     exitCode = proc.ExitCode;
}


    }
}
