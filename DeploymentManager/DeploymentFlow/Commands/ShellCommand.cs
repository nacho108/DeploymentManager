using System.Diagnostics;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class ShellCommand : ICommand
    {
        public async Task<CommandResult> Execute()
        {
            string standardOutput ="";
            string standardError ;
            int exitCode=9;
            await Task.Run(() =>
            {
                using (var process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.FileName = "..\\..\\batchs\\test.bat";

                    process.Start();

                    //Thread.Sleep(100);

                    using (Task processWaiter = Task.Run(() => process.WaitForExit()))
                    using (Task<string> outputReader = Task.Run(() => process.StandardOutput.ReadToEnd()))
                    using (Task<string> errorReader = Task.Factory.StartNew(() => process.StandardError.ReadToEnd()))
                    {
                        Task.WaitAll(processWaiter, outputReader, errorReader);

                        standardOutput = outputReader.Result;
                        standardError = errorReader.Result;
                        exitCode = process.ExitCode;
                    }
                }
            });
            return new CommandResult(exitCode, standardOutput);
        }
    }
}