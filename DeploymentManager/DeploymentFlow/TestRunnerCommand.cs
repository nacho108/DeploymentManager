using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Contracts;
using DeploymentFlow.Annotations;
using System.IO;

namespace DeploymentFlow
{
    public class TestRunnerCommand : ICommand
    {
        private readonly string _repositoryPath;

        public TestRunnerCommand([NotNull] string repositoryPath)
        {
            if (repositoryPath == null) throw new ArgumentNullException(nameof(repositoryPath));
            _repositoryPath = repositoryPath;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<CommandResult> Execute()
        {
            string[] testsAssemblies = { };
            await Task.Run(() =>
            {
                testsAssemblies = Directory.GetFiles(_repositoryPath, "*.Tests.dll",SearchOption.AllDirectories);
                foreach (var ts in testsAssemblies)
                {
                    Output = Output + ts + "\n";
                }
            });
            return new CommandResult(0, _output);
        }

        public string CommandDescription { get; }

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