namespace DeploymentManager.MainView
{
    internal interface IMessageProvider
    {
        void ShowMessage(string message);
    }

    class MessageProvider : IMessageProvider
    {
        public void ShowMessage(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}