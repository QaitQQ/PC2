
using System.Diagnostics;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {

        Process proc = new Process();
        var programPath = ".\\net6.0-windows\\MGSol.exe";
        proc.StartInfo.FileName = programPath;
        proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(programPath);
        proc.Start();

    }
}