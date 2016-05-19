using System.Windows;
using DeploymentManager.MainView;

namespace DeploymentManager
{
    public class WPFMessageProvider: IMessageProvider
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}