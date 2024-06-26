using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HJ212_Server;
using System.Collections.ObjectModel;
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
        private string? _MN = "88888888";

        [ObservableProperty]
        private string? _PW = "123456";

        [ObservableProperty]
        private IEnumerable<ST> _STs = Enum.GetValues(typeof(ST)).Cast<ST>();

        [ObservableProperty]
        private ST _ST = ST.大气环境污染源;

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
        [ObservableProperty]
        private int _timeOut_C1 = 5000;
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
                await _gb.SetTimeoutAndRetryAsync((int)ClientId!, MN, PW, ST, OverTime, ReCount, TimeOut_C1);
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

        #region C2
        [ObservableProperty]
        private int _timeOut_C2 = 5000;
        [ObservableProperty]
        private string _c2PolId = "w01018";
        [ObservableProperty]
        private string? _systemTime;
        [RelayCommand]
        private async Task C2TestAsync()
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
                SystemTime = (await _gb.GetSystemTimeAsync((int)ClientId!, MN, PW, ST, C2PolId, TimeOut_C2)).ToString("yyyy-MM-dd HH:mm:ss");
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

        #region C3
        [ObservableProperty]
        private int _timeOut_C3 = 5000;
        [ObservableProperty]
        private string _c3PolId = "w01018";
        [ObservableProperty]
        private string _sendTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        [RelayCommand]
        private async Task C3TestAsync()
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
            if (!DateTime.TryParseExact(SendTime, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var systemTime))
            {
                MessageBox.Show("发送时间有误");
                return;
            }
            try
            {
                await _gb.SetSystemTimeAsync((int)ClientId!, MN, PW, ST, C3PolId, systemTime, TimeOut_C3);
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

        #region C4
        [ObservableProperty]
        private bool _C4;
        partial void OnC4Changed(bool value)
        {
            if (value)
            {
                _gb!.OnAskSetSystemTime -= ClientViewModel_OnAskSetSystemTime;
                _gb!.OnAskSetSystemTime += ClientViewModel_OnAskSetSystemTime;
            }
            else
            {
                _gb!.OnAskSetSystemTime -= ClientViewModel_OnAskSetSystemTime;
            }
        }

        private async Task ClientViewModel_OnAskSetSystemTime(int clientId, (string PolId, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            MessageBox.Show($"PolId:{objects.PolId} 请求设置系统时间");
            await Task.CompletedTask;
        }
        #endregion

        #region C5
        [ObservableProperty]
        private int _timeOut_C5 = 5000;
        [ObservableProperty]
        private int? _rtdInterval;
        [RelayCommand]
        private async Task C5TestAsync()
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
                RtdInterval = await _gb.GetRealTimeDataIntervalAsync((int)ClientId!, MN, PW, ST, TimeOut_C5);
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

        #region C6
        [ObservableProperty]
        private int _timeOut_C6 = 5000;
        [RelayCommand]
        private async Task C6TestAsync()
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
                await _gb.SetRealTimeDataIntervalAsync((int)ClientId!, MN, PW, ST, RtdInterval ?? 0, TimeOut_C6);
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

        #region C7
        [ObservableProperty]
        private int _timeOut_C7 = 5000;
        [ObservableProperty]
        private int? _minInterval;
        [RelayCommand]
        private async Task C7TestAsync()
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
                MinInterval = await _gb.GetMinuteDataIntervalAsync((int)ClientId!, MN, PW, ST, TimeOut_C7);
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

        #region C8
        [ObservableProperty]
        private int _timeOut_C8 = 5000;
        [RelayCommand]
        private async Task C8TestAsync()
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
                await _gb.SetMinuteDataIntervalAsync((int)ClientId!, MN, PW, ST, MinInterval ?? 0, TimeOut_C8);
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

        #region C9
        [ObservableProperty]
        private int _timeOut_C9 = 5000;
        [ObservableProperty]
        private string _newPW = "123456";
        [RelayCommand]
        private async Task C9TestAsync()
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
                await _gb.SetNewPWAsync((int)ClientId!, MN, PW, ST, NewPW, TimeOut_C9);
                PW = NewPW;
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

        #region C10
        [ObservableProperty]
        private int _timeOut_C10 = 5000;
        [RelayCommand]
        private async Task C10TestAsync()
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
                await _gb.StartRealTimeDataAsync((int)ClientId!, MN, PW, ST, TimeOut_C10);
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

        #region C11
        [ObservableProperty]
        private int _timeOut_C11 = 5000;
        [RelayCommand]
        private async Task C11TestAsync()
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
                await _gb.StopRealTimeDataAsync((int)ClientId!, MN, PW, ST, TimeOut_C11);
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

        #region C12
        [ObservableProperty]
        private int _timeOut_C12 = 5000;
        [RelayCommand]
        private async Task C12TestAsync()
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
                await _gb.StartRunningStateDataAsync((int)ClientId!, MN, PW, ST, TimeOut_C12);
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

        #region C13
        [ObservableProperty]
        private int _timeOut_C13 = 5000;
        [RelayCommand]
        private async Task C13TestAsync()
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
                await _gb.StopRunningStateDataAsync((int)ClientId!, MN, PW, ST, TimeOut_C13);
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

        #region C14
        [ObservableProperty]
        private ObservableCollection<HJ212_Server.Model.RealTimeData> _RealTimeDatas = [];
        [ObservableProperty]
        private bool _C14;
        partial void OnC14Changed(bool value)
        {
            if (value)
            {
                _gb!.OnUploadRealTimeData -= ClientViewModel_OnUploadRealTimeData;
                _gb!.OnUploadRealTimeData += ClientViewModel_OnUploadRealTimeData;
            }
            else
            {
                _gb!.OnUploadRealTimeData -= ClientViewModel_OnUploadRealTimeData;
            }
        }

        private async Task ClientViewModel_OnUploadRealTimeData(int clientId, (DateTime DataTime, List<HJ212_Server.Model.RealTimeData> Data, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                RealTimeDatas.Clear();
                foreach (var item in objects.Data)
                {
                    RealTimeDatas.Add(item);
                }
            });
        }
        #endregion

        #region C15
        [ObservableProperty]
        private ObservableCollection<HJ212_Server.Model.RunningStateData> _RunningStateDatas = [];
        [ObservableProperty]
        private bool _C15;
        partial void OnC15Changed(bool value)
        {
            if (value)
            {
                _gb!.OnUploadRunningStateData -= ClientViewModel_OnUploadRunningStateData;
                _gb!.OnUploadRunningStateData += ClientViewModel_OnUploadRunningStateData; ;
            }
            else
            {
                _gb!.OnUploadRunningStateData -= ClientViewModel_OnUploadRunningStateData;
            }
        }

        private async Task ClientViewModel_OnUploadRunningStateData(int clientId, (DateTime DataTime, List<HJ212_Server.Model.RunningStateData> Data, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                RunningStateDatas.Clear();
                foreach (var item in objects.Data)
                {
                    RunningStateDatas.Add(item);
                }
            });
        }
        #endregion
    }
}
