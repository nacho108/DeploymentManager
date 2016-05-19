using System.Windows;
using DeploymentFlow;
using DeploymentManager.MainView;

namespace DeploymentManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WorkFlowProviderFactory workFlowProviderFactory = new WorkFlowProviderFactory();
            MainViewModel mainviewModel = new MainViewModel(workFlowProviderFactory, new WPFMessageProvider());
            MainWindow mw = new MainWindow() { DataContext = mainviewModel };
            mw.Show();
        }
    }
}
