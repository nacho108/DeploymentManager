using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DeploymentFlow;
using DeploymentManager.Annotations;

namespace DeploymentManager
{
    internal class MainViewModel:INotifyPropertyChanged
    {
        private FlowStepVm _stepSelected;
        private string _selectedStepOutput;
        private string _selectedDescription;

        public WorkFlowProvider FlowProvider { get; }

        public MainViewModel(WorkFlowProvider workFlowProvider)
        {
            FlowProvider = workFlowProvider;
            FlowStepsVm = FlowProvider.AllSteps.Select(flowStep => new FlowStepVm(flowStep)).ToList();
            StartCommand =new RelayCommand(async o=> { await workFlowProvider.StartWorkFlow(); });
        }

        public string SelectedDescription
        {
            get { return _selectedDescription; }
            private set
            {
                if (value == _selectedDescription) return;
                _selectedDescription = value;
                OnPropertyChanged();
            }
        }

        public string SelectedStepOutput
        {
            get { return _selectedStepOutput; }
            private set
            {
                if (value == _selectedStepOutput) return;
                _selectedStepOutput = value;
                OnPropertyChanged();
            }
        }

        public List<FlowStepVm> FlowStepsVm { get; }

        public FlowStepVm StepSelected
        {
            get { return _stepSelected; }
            set
            {
                if (value == _stepSelected) return;
                _stepSelected = value;
                SelectedStepOutput = _stepSelected.FlowStep.OutputResults;
                SelectedDescription = StepSelected.FlowStep.CommandDecription;
            }
        }

        public RelayCommand StartCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}