using Communication.Bus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Config;
using Config.Model;
using GBServerTest.Service;
using HJ212_Server;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace GBServerTest.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private Connection _connection;
        private IGB_Server? _gb;

        [ObservableProperty]
        private IEnumerable<HJ212_Server.Version> _Versions;
        [ObservableProperty]
        private HJ212_Server.Version _Version = HJ212_Server.Version.HJT212_2017;

        [ObservableProperty]
        private bool _isListened;
        [ObservableProperty]
        private string? _status;

        public ObservableCollection<ClientViewModel> TabItems { get; set; } = [];

        public MainViewModel(ConfigManager configManager)
        {
            _connection = configManager.Connection;
            Status = _connection.ToString();
            Versions = Enum.GetValues(typeof(HJ212_Server.Version)).Cast<HJ212_Server.Version>();
            WeakReferenceMessenger.Default.Register<StatusMessage>(this, (r, m) =>
            {
                _connection = m.Value;
                Status = m.Value.ToString();
            });
            WeakReferenceMessenger.Default.Register<CloseTabItemMessage>(this, async (r, m) =>
            {
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    TabItems.Remove(TabItems.Where(_ => _.Guid == m.Value).First());
                });
            });
        }

        [RelayCommand]
        private void ConnectionConfig()
        {
            var popupService = App.Current.Services.GetRequiredService<IPopupService>();
            popupService.ShowConnectionConfig();
        }

        [RelayCommand]
        private async Task ConnectAsync()
        {
            if (IsListened)
            {
                await _gb!.StopAsync();
                IsListened = false;
            }
            else
            {
                _gb = new GB_Server(new TcpServer(_connection.HostName == "Any" ? IPAddress.Any.ToString() : _connection.HostName, _connection.Port), Version);
                _gb.OnSentData += Gb_OnSentData;
                _gb.OnReceivedData += Gb_OnReceivedData;
                _gb.OnClientConnect += Gb_OnClientConnect;
                _gb.OnClientDisconnect += Gb_OnClientDisconnect;
                try
                {
                    await _gb.StartAsync();
                    IsListened = true;
                }
                catch
                {
                    //ExceptionStr = "连接失败，检查链路";
                }
            }
        }

        private async Task Gb_OnClientDisconnect(int clientId)
        {
            var client = TabItems.Where(x => x.ClientId == clientId).First();
            client.IsConnect = false;
            client.ClientId = null;
            await Task.CompletedTask;
        }

        private async Task Gb_OnClientConnect(int clientId)
        {
            string info = (await _gb!.GetClientInfos(clientId))!;
            var clientInfo = new ClientViewModel(_gb, clientId, info) { IsConnect = true };
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                TabItems.Add(clientInfo);
            });
        }

        private async Task Gb_OnReceivedData(int clientId, byte[] data)
        {
            TabItems.Where(x => x.ClientId == clientId).First().Log += $"{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff} GB Rec:<-- {Encoding.UTF8.GetString(data)}";
            await Task.CompletedTask;
        }

        private async Task Gb_OnSentData(int clientId, byte[] data)
        {
            TabItems.Where(x => x.ClientId == clientId).First().Log += $"{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff} GB Send:<-- {Encoding.UTF8.GetString(data)}";
            await Task.CompletedTask;
        }
    }

    internal class StatusMessage(Connection value) : ValueChangedMessage<Connection>(value) { }

    internal class CloseTabItemMessage(Guid clientId) : ValueChangedMessage<Guid>(clientId) { }
}