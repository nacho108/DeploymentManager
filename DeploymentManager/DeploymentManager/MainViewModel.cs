using System.ComponentModel;
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
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}