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
        public WorkFlowProvider FlowProvider { get; }

        public MainViewModel(WorkFlowProvider workFlowProvider)
        {
            FlowProvider = workFlowProvider;
            FlowStepsVm = FlowProvider.AllSteps.Select(flowStep => new FlowStepVm(flowStep)).ToList();
            StartCommand =new RelayCommand(async o=> { await workFlowProvider.StartWorkFlow(); });
        }

        public List<FlowStepVm> FlowStepsVm { get; }

        public RelayCommand StartCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}