using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DeploymentFlow;
using DeploymentFlow.Interfaces;
using DeploymentManager.Annotations;

namespace DeploymentManager.MainView
{
    internal class MainViewModel:INotifyPropertyChanged
    {
        private readonly IWorkFlowProviderFactory _workFlowProviderFactory;
        private FlowStepVm _stepSelected;
        private string _selectedStepOutput;
        private string _selectedDescription;
        private WorkFlowProvider _flowProvider;
        private List<FlowStepVm> _flowStepsVm;

        public WorkFlowProvider FlowProvider
        {
            get { return _flowProvider; }
            private set
            {
                if (_flowProvider == value) return;
                _flowProvider = value;
                OnPropertyChanged();
                FlowStepsVm= FlowProvider.AllSteps.Select(flowStep => new FlowStepVm(flowStep)).ToList();
            }
        }

        public MainViewModel(IWorkFlowProviderFactory workFlowProviderFactory)
        {
            _workFlowProviderFactory = workFlowProviderFactory;
            FlowProvider = _workFlowProviderFactory.CreateWorkFlow();
            FlowStepsVm = FlowProvider.AllSteps.Select(flowStep => new FlowStepVm(flowStep)).ToList();
            StartCommand =new RelayCommand(async o=> { await FlowProvider.StartWorkFlow(); });
            CreateWorkflowCommand = new RelayCommand(CreateWorkflow);
        }

        private void CreateWorkflow(object obj)
        {
            FlowProvider = _workFlowProviderFactory.CreateWorkFlow();
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

        public List<FlowStepVm> FlowStepsVm
        {
            get { return _flowStepsVm; }
            private set
            {
                if (Equals(value, _flowStepsVm)) return;
                _flowStepsVm = value;
                OnPropertyChanged();
            }
        }

        public FlowStepVm StepSelected
        {
            get { return _stepSelected; }
            set
            {
                if (value == _stepSelected) return;
                _stepSelected = value;
                OnPropertyChanged();
                SelectedStepOutput = _stepSelected?.FlowStep.OutputResults;
                SelectedDescription = StepSelected?.FlowStep.CommandDescription;
            }
        }

        public RelayCommand StartCommand { get; }
        public RelayCommand CreateWorkflowCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}