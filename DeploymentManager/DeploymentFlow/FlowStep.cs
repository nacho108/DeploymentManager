using System;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class FlowStep
    {
        public string Description { get; }
        public int Order { get; }
        public bool IsCurrent { get; set; }

        private readonly ICommand _command;

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
        }

    }
}