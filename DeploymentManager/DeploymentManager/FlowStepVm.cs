using System.Windows.Media;
using DeploymentFlow;

namespace DeploymentManager
{
    public class FlowStepVm
    {
        private readonly FlowStep _flowStep;

        public FlowStepVm(FlowStep flowStep)
        {
            _flowStep = flowStep;
            ExecuteCommand = new RelayCommand(async o => { await _flowStep.Execute(); }, o => _flowStep.IsCurrent);
        }

        public Brush StepBackgroundColor
        {
            get
            {
                if (_flowStep.IsCurrent) return Brushes.Green;
                if (_flowStep.IsDone)
                {
                    return Brushes.DarkOliveGreen;
                }
                return Brushes.DimGray;
            }
        }

        public RelayCommand ExecuteCommand { get; }
    }
}