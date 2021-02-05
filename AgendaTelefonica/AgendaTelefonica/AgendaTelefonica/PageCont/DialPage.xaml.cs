using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AgendaTelefonica.Models;
using Xamarin.Essentials;
using SQLite;

namespace AgendaTelefonica.PageCont
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialPage : ContentPage
    {
        DialModeliNotifay dialModel = new DialModeliNotifay();
        public DialPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            labelNumber.BindingContext = dialModel;
        }

        private void btnNr_Clicked(object sender, EventArgs e)
        {
            HapticFeedback.Perform(HapticFeedbackType.LongPress);
            Button btn = (Button)sender;
            if (dialModel.Number == null)
            {
                dialModel.Number += btn.Text;
                dialModel.Number_Printer += btn.Text;
            }
            else if (btn.Text == "C")
            {
                dialModel.Number = "";
                dialModel.Number_Printer = "";
            }
            else if (dialModel.Number.Length < 10)
            {
                if (dialModel.Number.Length == 4 ||
                    dialModel.Number.Length == 7 )
                {
                    dialModel.Number_Printer += " ";
                }
                    dialModel.Number += btn.Text;
                    dialModel.Number_Printer += btn.Text;
                
            }
        }

        private async void btnDial_Clicked(object sender, EventArgs e)
        {
            HapticFeedback.Perform(HapticFeedbackType.LongPress);
            if (call(dialModel.Number))
            {
                SQLiteConnection _conn = new SQLiteConnection(App.DataBaseLocation);
                _conn.CreateTable<HistoryElem>();
                DateTime dateTime = DateTime.Now;
                HistoryElem historyElem = new HistoryElem
                {
                    id_Contact = -1,
                    phoneNumber = dialModel.Number,
                    date = dateTime
                };

                _conn.Insert(historyElem);
                _conn.Close();
            }
            await Navigation.PopToRootAsync();
        }

        public bool call(string phoneNumber)
        {
            try
            {
                PhoneDialer.Open(phoneNumber);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                return false;
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                return false;
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                return false;
            }
            return true;
        }
    }
}