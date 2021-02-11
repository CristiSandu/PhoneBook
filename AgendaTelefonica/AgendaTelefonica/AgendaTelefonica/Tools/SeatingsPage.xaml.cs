using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AgendaTelefonica.Tools
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeatingsPage : ContentPage
    {
        private readonly ChartEntry[] entries = new[]
        {
             new ChartEntry(212)
             {
                 Label = "UWP",
                 ValueLabel = "212",
                 Color = SKColor.Parse("#2c3e50")
             },
             new ChartEntry(248)
             {
                 Label = "Android",
                 ValueLabel = "248",
                 Color = SKColor.Parse("#77d065")
             },
             new ChartEntry(128)
             {
                 Label = "iOS",
                 ValueLabel = "128",
                 Color = SKColor.Parse("#b455b6")
             },
             new ChartEntry(514)
             {
                 Label = "Shared",
                 ValueLabel = "514",
                 Color = SKColor.Parse("#3498db")
             }
        };
        
        public SeatingsPage()
        {
            InitializeComponent();
            chartView.Chart = new RadialGaugeChart { Entries = entries, LabelTextSize = 40 };
        }
    }
}