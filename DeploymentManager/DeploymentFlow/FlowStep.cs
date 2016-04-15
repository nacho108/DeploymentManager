using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class FlowStep : INotifyPropertyChanged
    {
        public string Description { get; }
        public int Order { get; }

        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                if (value == _isCurrent) return;
                _isCurrent = value;
                OnPropertyChanged();
            }
        }

        public bool IsDone
        {
            get { return _isDone; }
            private set
            {
                if (value == _isDone) return;
                _isDone = value;
                OnPropertyChanged();
            }
        }

        private readonly ICommand _command;
        private bool _isDone;
        private bool _isCurrent;

        public FlowStep(ICommand command, string description, int order)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (description == null) throw new ArgumentNullException(nameof(description));

            _command = command;
            Description = description;
            Order = order;
        }

        async public Task Execute()
        {
            await _command.Execute();
            IsDone = true;
            IsCurrent = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}