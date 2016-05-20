using System.Windows;
using DeploymentFlow;
using DeploymentManager.MainView;
using System.Configuration;

namespace DeploymentManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var repositoryPath = ConfigurationManager.AppSettings["RepositoryPath"];
            string solutionPath=ConfigurationManager.AppSettings["SolutionPath"];
            string databaseProjectPath= ConfigurationManager.AppSettings["DatabaseProjectPath"];

            if (repositoryPath == null || solutionPath == null || databaseProjectPath == null)
            {
                throw new ConfigurationErrorsException(
                    "The config.app should have repositoryPath, solutionPath and databaseProjectPath settings to be able to work.");
            }

            WorkFlowProviderFactory workFlowProviderFactory = new WorkFlowProviderFactory(repositoryPath, solutionPath, databaseProjectPath);
            MainViewModel mainviewModel = new MainViewModel(workFlowProviderFactory, new WPFMessageProvider(), databaseProjectPath);
            MainWindow mw = new MainWindow() { DataContext = mainviewModel };
            mw.Show();
        }
    }
}
