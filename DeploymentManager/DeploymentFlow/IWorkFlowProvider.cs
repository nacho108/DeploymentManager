using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public interface IWorkFlowProvider
    {
        Task StartWorkFlow();
        Task<IEnumerable<FlowStep>> GetAllSteps();
        Task<FlowStep> GetCurrentStep();



    }

    public class FlowStep
    {
        public string Description { get; set; }
        public int Order { get; set; }

        private readonly ICommand _command;
        private readonly string _description;
        private readonly int _order;

        public FlowStep(ICommand command, string description, int order)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (description == null) throw new ArgumentNullException(nameof(description));

            _command = command;
            _description = description;
            _order = order;
        }

        public Task Execute()
        {
            return _command.Execute();
        }

    }
}