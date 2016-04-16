using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DeploymentFlow.Annotations;

namespace DeploymentFlow
{
    public class WorkFlowProvider : IWorkFlowProvider, INotifyPropertyChanged
    {
        private readonly List<FlowStep> _flowSteps;
        private bool _workFlowFinished;
        private bool _running;

        public WorkFlowProvider(IEnumerable<FlowStep> flowSteps)
        {
            _flowSteps = new List<FlowStep>(flowSteps.OrderBy(o => o.Order).ToList());
        }

        public async Task StartWorkFlow()
        {
            if (!_running && !_workFlowFinished)
            {
                _running = true;
                OnPropertyChanged(nameof(Running));
                for (int i = 0; i < _flowSteps.Count; i++)
                {
                    await _flowSteps[i].Execute();
                }
                _running = false;
                OnPropertyChanged(nameof(Running));
                _workFlowFinished = true;
                OnPropertyChanged(nameof(WorkFlowFinished));
            }
        }

        public bool WorkFlowFinished => _workFlowFinished;
        public bool Running => _running;
        public IEnumerable<FlowStep> AllSteps => _flowSteps;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}