using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Contracts;
using DeploymentFlow.Annotations;
using DeploymentFlow.Interfaces;

namespace DeploymentFlow.Commands
{
    public class ShellCommand : ICommand
    {
        private readonly string _filename;
        private readonly string _arguments;

        public ShellCommand([NotNull] string filename, [NotNull] string arguments)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));
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
            int exitCode=0;
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
            return new CommandResult(exitCode, _output);
        }

        public string CommandDescription => _filename + " " + _arguments;
            

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