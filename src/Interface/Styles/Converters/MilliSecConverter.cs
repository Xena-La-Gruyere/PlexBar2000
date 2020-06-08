using System;
using System.Globalization;
using System.Windows.Data;

namespace Interface.Styles.Converters
{
    public class MilliSecConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long millisecs)
            {
                var time = TimeSpan.FromMilliseconds(millisecs);

                if(time.TotalHours > 1) 
                    return time.ToString(@"h\:mm\:ss");
                return time.ToString(@"m\:ss");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
