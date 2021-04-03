using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AgendaTelefonica.Convertors
{
    public class CountNumbersOfHistoryCalls : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                var data = value.ToString();
                var parseDate = DateTime.Parse(data);
                return parseDate.ToString("d/M/yyy, HH:mm", CultureInfo.CreateSpecificCulture("en-US"));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return null;
        }
    }
}
