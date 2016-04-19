using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DeploymentFlow.Annotations;

namespace DeploymentFlow.Commands
{
    public class ShellCommand : ICommand
    {
        private readonly string _filename;
        private readonly string _arguments;

        public ShellCommand(string filename, string arguments)
        {
            _filename = filename;
            _arguments = arguments;
        }

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
                    process.StartInfo.Arguments = _arguments;
                    process.StartInfo.FileName = _filename;
                    process.OutputDataReceived += Process_OutputDataReceived;
                    process.ErrorDataReceived += Process_ErrorDataReceived;
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                    exitCode = process.ExitCode;
                }
            });
            await Task.Delay(3000);
            return new CommandResult(exitCode, _output);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output += e.Data + "\n";
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output += e.Data+"\n";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}