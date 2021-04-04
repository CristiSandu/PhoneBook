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
using System.Collections;
using System.Globalization;
using AgendaTelefonica.Models;

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

        private IEnumerable GetPhoneNumber(string newTextValue = null)
        {
            SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation);
            var contacts = conn.Table<Models.Contact>().ToList();
            conn.Close();
            if (String.IsNullOrWhiteSpace(newTextValue))
                return contacts;

            return contacts.Where(c => c.phoneNumber.StartsWith(RemoveDiacritics(newTextValue), true, null));
        }

        public string RemoveDiacritics(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        private void cotactsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cont = e.CurrentSelection.FirstOrDefault() as Models.Contact;
            if (e.CurrentSelection != null)
            {
                dialModel.Number = cont.phoneNumber;
                string value_pnone = "";
                int i = 0;
                while (value_pnone.Length < 12)
                {
                    if (value_pnone.Length == 4 ||
                        value_pnone.Length == 8)
                    {
                        value_pnone += " ";
                    }
                    value_pnone += cont.phoneNumber[i];
                    i++;
                }
                dialModel.Number_Printer = value_pnone;
            }
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
                    dialModel.Number.Length == 7)
                {
                    dialModel.Number_Printer += " ";
                }
                dialModel.Number += btn.Text;
                dialModel.Number_Printer += btn.Text;
            }


            cotactsList.ItemsSource = GetPhoneNumber(dialModel.Number);
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