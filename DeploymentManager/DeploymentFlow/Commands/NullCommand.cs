
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DeploymentFlow.Annotations;
using DeploymentFlow.Interfaces;

namespace DeploymentFlow
{
    public class NullCommand : ICommand
    {
        private readonly string _name;

        public NullCommand(string name)
        {
            _name = name;
        }

        public async Task<CommandResult> Execute()
        {
            await Task.Delay(200);
            Output = "Ok";
            return new CommandResult(0, "Ok");
        }

        public string CommandDescription => "NullComand";

        public string Output { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}