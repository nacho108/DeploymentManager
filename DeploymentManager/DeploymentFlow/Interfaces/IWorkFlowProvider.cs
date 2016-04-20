using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeploymentFlow.Interfaces
{
    public interface IWorkFlowProvider
    {
        IEnumerable<FlowStep> AllSteps { get; }
        Task StartWorkFlow();
        WorkFlowState State { get; }
    }
}