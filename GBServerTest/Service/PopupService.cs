using GBServerTest.View;
using GBServerTest.ViewModel;

namespace GBServerTest.Service
{
    internal class PopupService(ConnectConfig connectConfig, ConnectViewModel connectConfigViewModel) : IPopupService
    {
        public void ShowConnectionConfig()
        {
            connectConfig.DataContext = connectConfigViewModel;
            connectConfig.Owner = App.Current.MainWindow;
            connectConfig.ShowDialog();
        }
    }
}
