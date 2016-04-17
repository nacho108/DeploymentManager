using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class FlowStep : INotifyPropertyChanged
    {
        public string Description { get; }

        public StepState State
        {
            get { return _state; }
            private set
            {
                if (_state == value) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public string OutputResults
        {
            get { return _outputResults; }
            private set
            {
                if (_outputResults == value) return;
                _outputResults = value;
                OnPropertyChanged();
            }
        }

        public int Order { get; }

        private readonly ICommand _command;
        private StepState _state;
        private string _outputResults;

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
            State = StepState.Running;
            var result=await _command.Execute();
            OutputResults = result;
            State = StepState.Done;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}