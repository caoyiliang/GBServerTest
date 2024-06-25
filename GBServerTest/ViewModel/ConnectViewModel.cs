using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Config;
using Config.Model;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace GBServerTest.ViewModel
{
    public partial class ConnectViewModel : ObservableObject
    {
        [ObservableProperty]
        Connection _connection;
        [ObservableProperty]
        private List<string> _hostName = ["Any"];

        public ConnectViewModel(ConfigManager configManager)
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in ipHostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    HostName.Add(ip.ToString());
                }
            }
            Connection = configManager.Connection;
            Connection.PropertyChanged += OnConnectionChanged;
        }

        private async void OnConnectionChanged(object? sender, PropertyChangedEventArgs e)
        {
            await Connection.TrySaveChangeAsync();
            WeakReferenceMessenger.Default.Send(new StatusMessage(Connection));
        }
    }
}