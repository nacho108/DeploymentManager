using System;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class FlowStep
    {
        public string Description { get; }
        public int Order { get; }

        public bool IsDone => _isDone;

        public bool IsCurrent { get; set; }

        private readonly ICommand _command;
        private bool _isDone;

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
            if (IsCurrent)
            {
                await _command.Execute();
                _isDone = true;
            }
        }

    }
}