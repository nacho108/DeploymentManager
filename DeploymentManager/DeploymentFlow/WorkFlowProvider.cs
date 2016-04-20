using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DeploymentFlow.Annotations;
using DeploymentFlow.Interfaces;

namespace DeploymentFlow
{
    public class WorkFlowProvider : IWorkFlowProvider, INotifyPropertyChanged
    {
        private readonly List<FlowStep> _flowSteps;
        private WorkFlowState _state;

        public WorkFlowProvider(IEnumerable<FlowStep> flowSteps)
        {
            _flowSteps = new List<FlowStep>(flowSteps.OrderBy(o => o.Order).ToList());
        }

        public async Task StartWorkFlow()
        {
            if (State==WorkFlowState.Created)
            {
                State = WorkFlowState.Running;
                for (int i = 0; i < _flowSteps.Count; i++)
                {
                    await _flowSteps[i].Execute();
                    if (_flowSteps[i].State == StepState.Error)
                    {
                        State = WorkFlowState.FinishedWithError;
                        break;
                    }
                }
                if (State == WorkFlowState.Running)
                {
                    State = WorkFlowState.FinishedSuccesfuly;
                }
            }
        }

        public WorkFlowState State
        {
            get { return _state; }
            private set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<FlowStep> AllSteps => _flowSteps;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}