using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DeploymentFlow.Interfaces;

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
                Debug.WriteLine("{0} {1}", Description, State);
                OnPropertyChanged();
            }
        }

        public string CommandDescription => _command.CommandDescription;

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
            _command.PropertyChanged += _command_PropertyChanged;
            var result = await _command.Execute();
            OutputResults = _command.Output;
            if (result.ExitCode == 0)
            {
                State = StepState.Done;
            }
            else
            {
                State = StepState.Error;
            }
        }

        private void _command_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Output")
            {
                OnPropertyChanged("OutputResults");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}