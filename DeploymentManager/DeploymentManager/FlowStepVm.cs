using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using DeploymentFlow;
using DeploymentManager.Annotations;

namespace DeploymentManager
{
    public class FlowStepVm : INotifyPropertyChanged
    {
        public FlowStep FlowStep { get; }

        public FlowStepVm(FlowStep flowStep)
        {
            FlowStep = flowStep;
            FlowStep.PropertyChanged += FlowStep_PropertyChanged;
        }

        private void FlowStep_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                OnPropertyChanged("StepBackgroundColor");
            }
        }

        public Brush StepBackgroundColor
        {
            get
            {
                Console.WriteLine("pidio back");
                switch (FlowStep.State)
                {
                    case StepState.Running:
                        return Brushes.Green;
                    case StepState.Done:
                        return Brushes.DarkOliveGreen;
                    case StepState.Error:
                        return Brushes.Red;
                }
                return Brushes.DimGray;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}