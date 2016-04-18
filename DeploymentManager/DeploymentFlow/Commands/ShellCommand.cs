using System.Diagnostics;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class ShellCommand : ICommand
    {
        public async Task<CommandResult> Execute()
        {
            string output = "";
            Process p = new Process();
            await Task.Run(() =>
            {
                // Start the child process.
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
            return new CommandResult(p.ExitCode,output);
        }
    }
}