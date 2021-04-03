using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AgendaTelefonica.Convertors
{
    public class ColorConvertorFavorite : IValueConverter
    {
        public static List<Color> _colorFav = new List<Color> { 
            Color.FromHex("#827717"), 
            Color.FromHex("#f57f17"), 
            Color.FromHex("#ff6f00"), 
            Color.FromHex("#e65100"),
            Color.FromHex("#76ff03"),
            Color.FromHex("#ffd54f"),
            Color.FromHex("#a1887f") };
        Random _rand = new Random();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = _rand.Next(_colorFav.Count);
            //return _colorFav[index];
            return Color.FromRgb(_rand.Next(250), _rand.Next(250), _rand.Next(250));
        }

        public static void populateListOfColors()
        {
            _colorFav.Add(Color.FromHex("#827717"));
            _colorFav.Add(Color.FromHex("#f57f17"));
            _colorFav.Add(Color.FromHex("#ff6f00"));
            _colorFav.Add(Color.FromHex("#e65100"));
            _colorFav.Add(Color.FromHex("#76ff03"));
            _colorFav.Add(Color.FromHex("#ffd54f"));
            _colorFav.Add(Color.FromHex("#a1887f"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
