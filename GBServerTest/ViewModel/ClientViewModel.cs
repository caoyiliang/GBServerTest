using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HJ212_Server;
using System.Collections.ObjectModel;
using System.Windows;

namespace GBServerTest.ViewModel
{
    public partial class ClientViewModel(IGB_Server gb, Guid clientId, string ip) : ObservableObject
    {
        private readonly IGB_Server _gb = gb;

        [ObservableProperty]
        private Guid _clientId = clientId;

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
            WeakReferenceMessenger.Default.Send(new CloseTabItemMessage(ClientId));
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
                await _gb.SetTimeoutAndRetryAsync(ClientId, MN, PW, ST, OverTime, ReCount, TimeOut_C1);
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
                SystemTime = (await _gb.GetSystemTimeAsync(ClientId, MN, PW, ST, C2PolId, TimeOut_C2)).ToString("yyyy-MM-dd HH:mm:ss");
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
                await _gb.SetSystemTimeAsync(ClientId, MN, PW, ST, C3PolId, systemTime, TimeOut_C3);
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
                _gb!.OnAskSetSystemTime += ClientViewModel_OnAskSetSystemTime;
            }
            else
            {
                _gb!.OnAskSetSystemTime -= ClientViewModel_OnAskSetSystemTime;
            }
        }

        private async Task ClientViewModel_OnAskSetSystemTime(Guid clientId, (string PolId, HJ212_Server.Model.RspInfo RspInfo) objects)
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
                RtdInterval = await _gb.GetRealTimeDataIntervalAsync(ClientId, MN, PW, ST, TimeOut_C5);
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
                await _gb.SetRealTimeDataIntervalAsync(ClientId, MN, PW, ST, RtdInterval ?? 0, TimeOut_C6);
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
                MinInterval = await _gb.GetMinuteDataIntervalAsync(ClientId, MN, PW, ST, TimeOut_C7);
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
                await _gb.SetMinuteDataIntervalAsync(ClientId, MN, PW, ST, MinInterval ?? 0, TimeOut_C8);
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
                await _gb.SetNewPWAsync(ClientId, MN, PW, ST, NewPW, TimeOut_C9);
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
                await _gb.StartRealTimeDataAsync(ClientId, MN, PW, ST, TimeOut_C10);
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
                await _gb.StopRealTimeDataAsync(ClientId, MN, PW, ST, TimeOut_C11);
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
                await _gb.StartRunningStateDataAsync(ClientId, MN, PW, ST, TimeOut_C12);
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
                await _gb.StopRunningStateDataAsync(ClientId, MN, PW, ST, TimeOut_C13);
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
                _gb!.OnUploadRealTimeData += ClientViewModel_OnUploadRealTimeData;
            }
            else
            {
                _gb!.OnUploadRealTimeData -= ClientViewModel_OnUploadRealTimeData;
            }
        }

        private async Task ClientViewModel_OnUploadRealTimeData(Guid clientId, (DateTime DataTime, List<HJ212_Server.Model.RealTimeData> Data, HJ212_Server.Model.RspInfo RspInfo) objects)
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
                _gb!.OnUploadRunningStateData += ClientViewModel_OnUploadRunningStateData;
            }
            else
            {
                _gb!.OnUploadRunningStateData -= ClientViewModel_OnUploadRunningStateData;
            }
        }

        private async Task ClientViewModel_OnUploadRunningStateData(Guid clientId, (DateTime DataTime, List<HJ212_Server.Model.RunningStateData> Data, HJ212_Server.Model.RspInfo RspInfo) objects)
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

        #region C16
        [ObservableProperty]
        private ObservableCollection<HJ212_Server.Model.StatisticsData> _MinuteDatas = [];
        [ObservableProperty]
        private bool _C16;
        partial void OnC16Changed(bool value)
        {
            if (value)
            {
                _gb!.OnUploadMinuteData += ClientViewModel_OnUploadMinuteData;
            }
            else
            {
                _gb!.OnUploadMinuteData -= ClientViewModel_OnUploadMinuteData;
            }
        }

        private async Task ClientViewModel_OnUploadMinuteData(Guid clientId, (DateTime DataTime, List<HJ212_Server.Model.StatisticsData> Data, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                MinuteDatas.Clear();
                foreach (var item in objects.Data)
                {
                    MinuteDatas.Add(item);
                }
            });
        }
        #endregion

        #region C17
        [ObservableProperty]
        private ObservableCollection<HJ212_Server.Model.StatisticsData> _HourDatas = [];
        [ObservableProperty]
        private bool _C17;
        partial void OnC17Changed(bool value)
        {
            if (value)
            {
                _gb!.OnUploadHourData += ClientViewModel_OnUploadHourData;
            }
            else
            {
                _gb!.OnUploadHourData -= ClientViewModel_OnUploadHourData;
            }
        }

        private async Task ClientViewModel_OnUploadHourData(Guid clientId, (DateTime DataTime, List<HJ212_Server.Model.StatisticsData> Data, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                HourDatas.Clear();
                foreach (var item in objects.Data)
                {
                    HourDatas.Add(item);
                }
            });
        }
        #endregion

        #region C18
        [ObservableProperty]
        private ObservableCollection<HJ212_Server.Model.StatisticsData> _DayDatas = [];
        [ObservableProperty]
        private bool _C18;
        partial void OnC18Changed(bool value)
        {
            if (value)
            {
                _gb!.OnUploadDayData += ClientViewModel_OnUploadDayData;
            }
            else
            {
                _gb!.OnUploadDayData -= ClientViewModel_OnUploadDayData;
            }
        }

        private async Task ClientViewModel_OnUploadDayData(Guid clientId, (DateTime DataTime, List<HJ212_Server.Model.StatisticsData> Data, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                DayDatas.Clear();
                foreach (var item in objects.Data)
                {
                    DayDatas.Add(item);
                }
            });
        }
        #endregion

        #region C19
        [ObservableProperty]
        private ObservableCollection<HJ212_Server.Model.RunningTimeData> _RunningTimeDatas = [];
        [ObservableProperty]
        private bool _C19;
        partial void OnC19Changed(bool value)
        {
            if (value)
            {
                _gb!.OnUploadRunningTimeData += ClientViewModel_OnUploadRunningTimeData;
            }
            else
            {
                _gb!.OnUploadRunningTimeData -= ClientViewModel_OnUploadRunningTimeData;
            }
        }

        private async Task ClientViewModel_OnUploadRunningTimeData(Guid clientId, (DateTime DataTime, List<HJ212_Server.Model.RunningTimeData> Data, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                RunningTimeDatas.Clear();
                foreach (var item in objects.Data)
                {
                    RunningTimeDatas.Add(item);
                }
            });
        }
        #endregion

        #region C20
        [ObservableProperty]
        private int _timeOut_C20 = 5000;
        [ObservableProperty]
        private string _startTime_C20 = DateTime.Now.AddMinutes(-10).ToString("yyyyMMddHHmmss");
        [ObservableProperty]
        private string _endTime_C20 = DateTime.Now.ToString("yyyyMMddHHmmss");
        private List<HJ212_Server.Model.HistoryData> _MinuteHistoryDatas = [];
        [ObservableProperty]
        private string? _historyDateTime_C20;
        [ObservableProperty]
        private int _total_C20;
        [ObservableProperty]
        private int _index_C20;

        partial void OnIndex_C20Changed(int value)
        {
            if (value > Total_C20)
            {
                Index_C20 = value = Total_C20;
            }
            if (value < 1)
            {
                Index_C20 = value = 1;
            }
            MinuteDatas.Clear();
            foreach (var item in _MinuteHistoryDatas[value - 1].Data)
            {
                MinuteDatas.Add(item);
            }
            HistoryDateTime_C20 = _MinuteHistoryDatas[value - 1].DataTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        [RelayCommand]
        private async Task C20TestAsync()
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
            if (!DateTime.TryParseExact(StartTime_C20, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var startTime))
            {
                MessageBox.Show("开始时间有误");
                return;
            }
            if (!DateTime.TryParseExact(EndTime_C20, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var endTime))
            {
                MessageBox.Show("结束时间有误");
                return;
            }
            try
            {
                _MinuteHistoryDatas = await _gb.GetMinuteDataAsync(ClientId, MN, PW, ST, startTime, endTime, TimeOut_C20);
                Total_C20 = _MinuteHistoryDatas.Count;
                Index_C20 = 1;
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

        #region C21
        [ObservableProperty]
        private int _timeOut_C21 = 5000;
        [ObservableProperty]
        private string _startTime_C21 = DateTime.Now.AddHours(-10).ToString("yyyyMMddHHmmss");
        [ObservableProperty]
        private string _endTime_C21 = DateTime.Now.ToString("yyyyMMddHHmmss");
        private List<HJ212_Server.Model.HistoryData> _HourHistoryDatas = [];
        [ObservableProperty]
        private string? _historyDateTime_C21;
        [ObservableProperty]
        private int _total_C21;
        [ObservableProperty]
        private int _index_C21;
        partial void OnIndex_C21Changed(int value)
        {
            if (value > Total_C21)
            {
                Index_C21 = value = Total_C21;
            }
            if (value < 1)
            {
                Index_C21 = value = 1;
            }
            HourDatas.Clear();
            foreach (var item in _HourHistoryDatas[value - 1].Data)
            {
                HourDatas.Add(item);
            }
            HistoryDateTime_C21 = _HourHistoryDatas[value - 1].DataTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        [RelayCommand]
        private async Task C21TestAsync()
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
            if (!DateTime.TryParseExact(StartTime_C21, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var startTime))
            {
                MessageBox.Show("开始时间有误");
                return;
            }
            if (!DateTime.TryParseExact(EndTime_C21, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var endTime))
            {
                MessageBox.Show("结束时间有误");
                return;
            }
            try
            {
                _HourHistoryDatas = await _gb.GetHourDataAsync(ClientId, MN, PW, ST, startTime, endTime, TimeOut_C21);
                Total_C21 = _HourHistoryDatas.Count;
                Index_C21 = 1;
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

        #region C22
        [ObservableProperty]
        private int _timeOut_C22 = 5000;
        [ObservableProperty]
        private string _startTime_C22 = DateTime.Now.AddDays(-10).ToString("yyyyMMddHHmmss");
        [ObservableProperty]
        private string _endTime_C22 = DateTime.Now.ToString("yyyyMMddHHmmss");
        private List<HJ212_Server.Model.HistoryData> _DayHistoryDatas = [];
        [ObservableProperty]
        private string? _historyDateTime_C22;
        [ObservableProperty]
        private int _total_C22;
        [ObservableProperty]
        private int _index_C22;
        partial void OnIndex_C22Changed(int value)
        {
            if (value > Total_C22)
            {
                Index_C22 = value = Total_C22;
            }
            if (value < 1)
            {
                Index_C22 = value = 1;
            }
            DayDatas.Clear();
            foreach (var item in _DayHistoryDatas[value - 1].Data)
            {
                DayDatas.Add(item);
            }
            HistoryDateTime_C22 = _DayHistoryDatas[value - 1].DataTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        [RelayCommand]
        private async Task C22TestAsync()
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
            if (!DateTime.TryParseExact(StartTime_C22, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var startTime))
            {
                MessageBox.Show("开始时间有误");
                return;
            }
            if (!DateTime.TryParseExact(EndTime_C22, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var endTime))
            {
                MessageBox.Show("结束时间有误");
                return;
            }
            try
            {
                _DayHistoryDatas = await _gb.GetDayDataAsync(ClientId, MN, PW, ST, startTime, endTime, TimeOut_C22);
                Total_C22 = _DayHistoryDatas.Count;
                Index_C22 = 1;
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

        #region C23
        [ObservableProperty]
        private int _timeOut_C23 = 5000;
        [ObservableProperty]
        private string _startTime_C23 = DateTime.Now.AddDays(-10).ToString("yyyyMMddHHmmss");
        [ObservableProperty]
        private string _endTime_C23 = DateTime.Now.ToString("yyyyMMddHHmmss");
        private List<HJ212_Server.Model.RunningTimeHistory> _RunningTimeHistoryDatas = [];
        [ObservableProperty]
        private string? _historyDateTime_C23;
        [ObservableProperty]
        private int _total_C23;
        [ObservableProperty]
        private int _index_C23;
        partial void OnIndex_C23Changed(int value)
        {
            if (value > Total_C23)
            {
                Index_C23 = value = Total_C23;
            }
            if (value < 1)
            {
                Index_C23 = value = 1;
            }
            RunningTimeDatas.Clear();
            foreach (var item in _RunningTimeHistoryDatas[value - 1].Data)
            {
                RunningTimeDatas.Add(item);
            }
            HistoryDateTime_C23 = _RunningTimeHistoryDatas[value - 1].DataTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        [RelayCommand]
        private async Task C23TestAsync()
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
            if (!DateTime.TryParseExact(StartTime_C23, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var startTime))
            {
                MessageBox.Show("开始时间有误");
                return;
            }
            if (!DateTime.TryParseExact(EndTime_C23, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var endTime))
            {
                MessageBox.Show("结束时间有误");
                return;
            }
            try
            {
                _RunningTimeHistoryDatas = await _gb.GetRunningTimeDataAsync(ClientId, MN, PW, ST, startTime, endTime, TimeOut_C23);
                Total_C23 = _RunningTimeHistoryDatas.Count;
                Index_C23 = 1;
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

        #region C24
        [ObservableProperty]
        private string? _DataTime_C24;
        [ObservableProperty]
        private string? _RestartTime_C24;
        [ObservableProperty]
        private bool _C24;
        partial void OnC24Changed(bool value)
        {
            if (value)
            {
                _gb!.OnUploadAcquisitionDeviceRestartTime += ClientViewModel_OnUploadAcquisitionDeviceRestartTime;
            }
            else
            {
                _gb!.OnUploadAcquisitionDeviceRestartTime -= ClientViewModel_OnUploadAcquisitionDeviceRestartTime;
            }
        }

        private async Task ClientViewModel_OnUploadAcquisitionDeviceRestartTime(Guid clientId, (DateTime DataTime, DateTime RestartTime, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                DataTime_C24 = objects.DataTime.ToString("yyyy-MM-dd HH:mm:ss");
                RestartTime_C24 = objects.RestartTime.ToString("yyyy-MM-dd HH:mm:ss");
            });
        }
        #endregion

        #region C30
        [ObservableProperty]
        private int _timeOut_C30 = 5000;
        [ObservableProperty]
        private string _c30PolId = "w01018";
        [RelayCommand]
        private async Task C30TestAsync()
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
                await _gb.CalibrateAsync(ClientId, MN, PW, ST, C30PolId, TimeOut_C30);
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

        #region C31
        [ObservableProperty]
        private int _timeOut_C31 = 5000;
        [ObservableProperty]
        private string _c31PolId = "w01018";
        [RelayCommand]
        private async Task C31TestAsync()
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
                await _gb.RealTimeSamplingAsync(ClientId, MN, PW, ST, C31PolId, TimeOut_C31);
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

        #region C32
        [ObservableProperty]
        private int _timeOut_C32 = 5000;
        [ObservableProperty]
        private string _c32PolId = "w01018";
        [RelayCommand]
        private async Task C32TestAsync()
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
                await _gb.StartCleaningOrBlowbackAsync(ClientId, MN, PW, ST, C32PolId, TimeOut_C32);
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

        #region C33
        [ObservableProperty]
        private int _timeOut_C33 = 5000;
        [ObservableProperty]
        private string _c33PolId = "w01018";
        [RelayCommand]
        private async Task C33TestAsync()
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
                await _gb.ComparisonSamplingAsync(ClientId, MN, PW, ST, C33PolId, TimeOut_C33);
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

        #region C34
        [ObservableProperty]
        private int _timeOut_C34 = 5000;
        [ObservableProperty]
        private string? _DataTime_C34;
        [ObservableProperty]
        private string? _VaseNo;
        [RelayCommand]
        private async Task C34TestAsync()
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
                (DateTime DataTime, string VaseNo) rs = await _gb.OutOfStandardRetentionSampleAsync(ClientId, MN, PW, ST, TimeOut_C34);
                DataTime_C34 = rs.DataTime.ToString("yyyy-MM-dd HH:mm:ss");
                VaseNo = rs.VaseNo;
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

        #region C35
        [ObservableProperty]
        private int _timeOut_C35 = 5000;
        [ObservableProperty]
        private string _c35PolId = "w01018";
        [ObservableProperty]
        private string _cstartTime = "000000";
        [ObservableProperty]
        private int _ctime = 1;
        [RelayCommand]
        private async Task C35TestAsync()
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
            if (!TimeOnly.TryParseExact(CstartTime, "HHmmss", out var startTime))
            {
                MessageBox.Show("开始时间有误");
                return;
            }
            try
            {
                await _gb.SetSamplingPeriodAsync(ClientId, MN, PW, ST, C35PolId, startTime, Ctime, TimeOut_C35);
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

        #region C36
        [ObservableProperty]
        private int _timeOut_C36 = 5000;
        [ObservableProperty]
        private string _c36PolId = "w01018";
        [ObservableProperty]
        private string? _cstartTime_C36;
        [ObservableProperty]
        private int? _ctime_C36;
        [RelayCommand]
        private async Task C36TestAsync()
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
                var rs = await _gb.GetSamplingPeriodAsync(ClientId, MN, PW, ST, C36PolId, TimeOut_C36);
                CstartTime_C36 = rs.CstartTime.ToString("HHmmss");
                Ctime_C36 = rs.CTime;
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

        #region C37
        [ObservableProperty]
        private int _timeOut_C37 = 5000;
        [ObservableProperty]
        private string _c37PolId = "w01018";
        [ObservableProperty]
        private int? _stime_C37;
        [RelayCommand]
        private async Task C37TestAsync()
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
                Stime_C37 = await _gb.GetSampleExtractionTimeAsync(ClientId, MN, PW, ST, C37PolId, TimeOut_C37);
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

        #region C38
        [ObservableProperty]
        private int _timeOut_C38 = 5000;
        [ObservableProperty]
        private string _c38PolId = "w01018";
        [ObservableProperty]
        private string? _SN_C38;
        [RelayCommand]
        private async Task C38TestAsync()
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
                SN_C38 = await _gb.GetSNAsync(ClientId, MN, PW, ST, C38PolId, TimeOut_C38);
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

        #region C39
        [ObservableProperty]
        private string? _polId_C39;
        [ObservableProperty]
        private string? _SN_C39;
        [ObservableProperty]
        private bool _C39;
        partial void OnC39Changed(bool value)
        {
            if (value)
            {
                _gb!.OnUploadSN += ClientViewModel_OnUploadSN;
            }
            else
            {
                _gb!.OnUploadSN -= ClientViewModel_OnUploadSN;
            }
        }

        private async Task ClientViewModel_OnUploadSN(Guid clientId, (DateTime DataTime, string PolId, string SN, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                PolId_C39 = objects.PolId;
                SN_C39 = objects.SN;
            });
        }
        #endregion

        #region C40
        [ObservableProperty]
        private string? _DataTime_C40;
        [ObservableProperty]
        private string? _PolId_C40;
        [ObservableProperty]
        private string? _Info_C40;
        [ObservableProperty]
        private bool _C40;
        partial void OnC40Changed(bool value)
        {
            if (value)
            {
                _gb!.OnUploadLog += ClientViewModel_OnUploadLog;
            }
            else
            {
                _gb!.OnUploadLog -= ClientViewModel_OnUploadLog;
            }
        }

        private async Task ClientViewModel_OnUploadLog(Guid clientId, (DateTime DataTime, string? PolId, string Log, HJ212_Server.Model.RspInfo RspInfo) objects)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                DataTime_C40 = objects.DataTime.ToString("yyyy-MM-dd HH:mm:ss");
                PolId_C40 = objects.PolId;
                Info_C40 = objects.Log;
            });
        }
        #endregion
    }
}