using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Contracts;
using DeploymentFlow.Annotations;
using System.IO;

namespace DeploymentFlow
{
    public class TestRunnerCommand : ICommand
    {
        private readonly string _testsPath;

        public TestRunnerCommand([NotNull] string testsPath)
        {
            if (testsPath == null) throw new ArgumentNullException(nameof(testsPath));
            _testsPath = testsPath;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<CommandResult> Execute()
        {
            int exitCode = 0;
            string[] testsAssemblies = { };
            await Task.Run(() =>
            {
                testsAssemblies = Directory.GetFiles(_testsPath, "*.Tests.dll",SearchOption.AllDirectories);
                string assembliesList = "";
                foreach (var ts in testsAssemblies)
                {
                    assembliesList = assembliesList+ ts+" ";
                }

                using (var process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WorkingDirectory = _testsPath;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.FileName = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe";
                    process.StartInfo.Arguments = assembliesList;
                    CommandDescription = process.StartInfo.FileName+" "+ process.StartInfo.Arguments;
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
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output += e.Data + "\n";
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output += e.Data + "\n";
        }


        public string CommandDescription { get; private set; }

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


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}