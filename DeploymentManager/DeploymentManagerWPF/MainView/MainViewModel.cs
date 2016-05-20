using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Contracts;
using DeploymentFlow;
using DeploymentFlow.Interfaces;
using DeploymentManager.Annotations;

namespace DeploymentManager.MainView
{
    internal class MainViewModel:INotifyPropertyChanged
    {
        private readonly IWorkFlowProviderFactory _workFlowProviderFactory;
        private readonly IMessageProvider _messageProvider;
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

        public string RequiredVersion { get; set; }
        public string InitialMayorVersion { get; set; }
        public string InitialMinorVersion { get; set; }
        public string InitialBuildVersion { get; set; }

        public MainViewModel(IWorkFlowProviderFactory workFlowProviderFactory, IMessageProvider messageProvider, string databaseProjectPath)
        {
            _workFlowProviderFactory = workFlowProviderFactory;
            _messageProvider = messageProvider;
            //FlowProvider = _workFlowProviderFactory.CreateWorkFlow();
            //FlowStepsVm = FlowProvider.AllSteps.Select(flowStep => new FlowStepVm(flowStep)).ToList();
            StartCommand =new RelayCommand(async o=> { await FlowProvider.StartWorkFlow(); });
            CreateWorkflowCommand = new RelayCommand(CreateWorkflow);
            RequiredVersion = "1.1.0.0";
            InitialMayorVersion = "1";
            InitialMinorVersion = "1";
            InitialBuildVersion = "20";
        }

        private void CreateWorkflow(object obj)
        {
            int initialMayorVersionInt, initialMinorVersionInt, initialBuildVersionInt;

            if (!int.TryParse(InitialMayorVersion, out initialMayorVersionInt))
            {
                _messageProvider.ShowMessage("It was not possible to Parse Initial Mayor Version");
                return;
            }
            if (!int.TryParse(InitialMinorVersion, out initialMinorVersionInt))
            {
                _messageProvider.ShowMessage("It was not possible to Parse Initial minor Version");
                return;
            }
            if (!int.TryParse(InitialBuildVersion, out initialBuildVersionInt))
            {
                _messageProvider.ShowMessage("It was not possible to Parse Initial build Version");
                return;
            }

            FlowProvider = _workFlowProviderFactory.CreateWorkFlow(RequiredVersion, initialMayorVersionInt, initialMinorVersionInt, initialBuildVersionInt);
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