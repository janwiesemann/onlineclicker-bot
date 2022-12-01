using System;
using System.Globalization;
using System.Windows.Data;

namespace OnlineClicker_bot
{
    internal class ToPercentageConverter : IMultiValueConverter
    {
        public static readonly ToPercentageConverter Instance = new ToPercentageConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                throw new ArgumentNullException();

            if (values.Length != 2)
                throw new ArgumentException("Values array does not have a length of 2!", nameof(values));

            if (values[0] == null)
                throw new ArgumentNullException($"{nameof(values)}[0]");

            if (values[1] == null)
                throw new ArgumentNullException($"{nameof(values)}[1]");

            if ((double)values[1] == 0)
                return 0.0;

            return (double)values[0] / (double)values[1] * 100.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}