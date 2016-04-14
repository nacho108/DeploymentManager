using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentFlow
{
    public class WorkFlowProvider : IWorkFlowProvider
    {
        private readonly List<FlowStep> _flowSteps;
        private FlowStep _currentStep;
        private int _currentStepindex;
        private bool _workFlowFinished;

        public WorkFlowProvider(IEnumerable<FlowStep> flowSteps)
        {
            _flowSteps = new List<FlowStep>(flowSteps.OrderBy(o => o.Order).ToList());
            _currentStepindex = 0;
            _currentStep = _flowSteps[_currentStepindex];
            _currentStep.IsCurrent = true;
        }

        public bool WorkFlowFinished => _workFlowFinished;

        public IEnumerable<FlowStep> AllSteps => _flowSteps;

        public FlowStep GetCurrentStep()
        {
            return _currentStep;
        }

        async public Task ExecuteCurrentStep()
        {
            if (_workFlowFinished) return;
            await _currentStep.Execute();
            _currentStepindex++;
            if (_currentStepindex <= _flowSteps.Count - 1)
            {
                _currentStep = _flowSteps[_currentStepindex];
                _flowSteps[_currentStepindex].IsCurrent = true;
            }
            else
            {
                _workFlowFinished = true;
            }
        }
    }
}