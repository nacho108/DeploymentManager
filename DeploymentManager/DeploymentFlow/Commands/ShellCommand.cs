using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DeploymentFlow.Annotations;

namespace DeploymentFlow.Commands
{
    public class ShellCommand : ICommand, INotifyPropertyChanged
    {
        private string _output;
        public string Output
        {
            get { return _output; }
            set
            {
                if (value == _output) return;
                _output = value;
                OnPropertyChanged();
            }
        }

        public async Task<CommandResult> Execute()
        {
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
                    process.OutputDataReceived += Process_OutputDataReceived;
                    process.Start();
                    process.BeginOutputReadLine();

                    using (Task processWaiter = Task.Run(() => process.WaitForExit()))
                    {
                        Task.WaitAll(processWaiter);
                        exitCode = process.ExitCode;
                    }
                }
            });
            return new CommandResult(exitCode, _output);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _output += e.Data+"\n";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}