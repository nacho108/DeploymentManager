using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public interface IWorkFlowProvider
    {
        IEnumerable<FlowStep> GetAllSteps();
        FlowStep GetCurrentStep();
        Task ExecuteCurrentStep();
        bool WorkFlowFinished { get; } 
    }
}