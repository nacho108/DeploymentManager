using System;
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

        public string Description => _flowStep.Description;
        public string State => _flowStep.State.ToString();


        public Brush StepBackgroundColor
        {
            get
            {
                Console.WriteLine("pidio back");
                switch (_flowStep.State)
                {
                    case StepState.Running:
                        return Brushes.Green;
                    case StepState.Done:
                        return Brushes.DarkOliveGreen;
                }
                return Brushes.DimGray;
            }
        }
    }
}