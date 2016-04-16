using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public interface IWorkFlowProvider
    {
        IEnumerable<FlowStep> AllSteps { get; }
        Task StartWorkFlow();
        bool Running { get; }
        bool WorkFlowFinished { get; }
    }
}