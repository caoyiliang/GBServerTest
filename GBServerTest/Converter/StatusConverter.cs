using System.Globalization;
using System.Windows.Data;

namespace GBServerTest.Converter
{
    class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool v)
            {
                return v ? "停止监听" : "监听";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
