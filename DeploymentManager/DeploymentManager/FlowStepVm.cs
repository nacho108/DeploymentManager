using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using DeploymentFlow;
using DeploymentManager.Annotations;

namespace DeploymentManager
{
    public class FlowStepVm 
    {
        private readonly FlowStep _flowStep;

        public FlowStepVm(FlowStep flowStep)
        {
            _flowStep = flowStep;
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
    }
}