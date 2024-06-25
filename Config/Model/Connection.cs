using CommunityToolkit.Mvvm.ComponentModel;
using DataPairs.Interfaces;
using DataPairs;

namespace Config.Model
{
    public partial class Connection : ObservableObject
    {
        [ObservableProperty]
        private string _HostName = "127.0.0.1";
        [ObservableProperty]
        private int _Port = 2756;

        private static readonly IDataPair<Connection> _pair = new DataPair<Connection>(nameof(Connection), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PairsDB.dll"));

        public async Task InitAsync()
        {
            var data = await _pair.TryGetValueAsync();
            foreach (var item in data.GetType().GetProperties())
            {
                GetType().GetProperty(item.Name)!.SetValue(this, item.GetValue(data));
            }
        }

        public async Task TrySaveChangeAsync()
        {
            await _pair.TryInitOrUpdateAsync(this);
        }

        public override string ToString()
        {
            return $"当前[连接方式:TcpServer] [远端Ip:{HostName}][端口:{Port}]";
        }
    }
}
