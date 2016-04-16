using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class FlowStep : INotifyPropertyChanged
    {
        public string Description { get; }

        public string State
        {
            get { return _state; }
            private set { _state = value; }
        }

        public string OutputResults { get; private set; }

        public int Order { get; }

        private readonly ICommand _command;
        private string _state;

        public FlowStep(ICommand command, string description, int order)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (description == null) throw new ArgumentNullException(nameof(description));

            _command = command;
            Description = description;
            Order = order;
        }

        public async Task Execute()
        {
            State = "Running...";
            var result=await _command.Execute();
            State = "Done.";
            OutputResults = result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}