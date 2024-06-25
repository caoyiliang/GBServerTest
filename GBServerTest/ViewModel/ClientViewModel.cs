using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HJ212_Server;
using System.Windows;

namespace GBServerTest.ViewModel
{
    public partial class ClientViewModel(IGB_Server gb, int clientId, string ip) : ObservableObject
    {
        private readonly IGB_Server _gb = gb;
        public Guid Guid { get; } = Guid.NewGuid();

        [ObservableProperty]
        private int? _clientId = clientId;

        [ObservableProperty]
        private string _ip = ip;

        [ObservableProperty]
        private string? _log;

        [ObservableProperty]
        private string? _MN;

        [ObservableProperty]
        private string? _PW;

        [ObservableProperty]
        private IEnumerable<ST> _STs = Enum.GetValues(typeof(ST)).Cast<ST>();

        [ObservableProperty]
        private ST _ST;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private bool _isConnect;

        [ObservableProperty]
        private int _OverTime = 5;
        [ObservableProperty]
        private int _ReCount = 3;

        partial void OnIsConnectChanged(bool value)
        {
            Title = value ? Ip : $"{Ip}连接断开";
        }

        [RelayCommand]
        private void Close()
        {
            WeakReferenceMessenger.Default.Send(new CloseTabItemMessage(Guid));
        }

        #region C1
        [RelayCommand]
        private async Task C1TestAsync()
        {
            if (MN == null)
            {
                MessageBox.Show("MN空");
                return;
            }
            if (PW == null)
            {
                MessageBox.Show("PW空");
                return;
            }
            try
            {
                await _gb.SetTimeoutAndRetryAsync((int)ClientId!, MN, PW, ST, OverTime, ReCount);
            }
            catch (TimeoutException)
            {
                MessageBox.Show("请求超时");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
