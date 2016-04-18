using System.Diagnostics;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class ShellCommand : ICommand
    {
        public async Task<string> Execute()
        {
            string output = "";
            await Task.Run(() =>
            {
                // Start the child process.
                Process p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "test.bat";
                p.Start();
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
            });
            return output;
        }
    }
}