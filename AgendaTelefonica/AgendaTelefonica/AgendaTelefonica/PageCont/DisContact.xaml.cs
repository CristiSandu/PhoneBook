using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AgendaTelefonica.PageCont;
using Xamarin.Essentials;
using SQLite;
using AgendaTelefonica.Models;
using System.IO;

namespace AgendaTelefonica.PageCont
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisContact : ContentPage
    {
        Models.Contact _cont;
        SQLiteConnection _conn ;
        public DisContact()
        {
            InitializeComponent();
            _cont = (Models.Contact)BindingContext;
        }

        public DisContact(Models.Contact contact)
        {
            InitializeComponent();
            _cont = contact;
            firstName.Text = _cont.firstName;
            secondName.Text = _cont.secondName;
            phoneNumber.Text = _cont.phoneNumber;
            email.Text = _cont.email;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            byte[] b = _cont.profilPicture;
            if (b != null)
            {
                Stream ms = new MemoryStream(b);
                contactPhoto.Source = ImageSource.FromStream(() => ms);
            } else
            {
                contactPhoto.Source = "contact_default.png";
                contactPhoto.Aspect = Aspect.AspectFit;
            }
        }

        private async void backBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void callBtn_Clicked(object sender, EventArgs e)
        {
            if (call(phoneNumber.Text))
            {
                _conn = new SQLiteConnection(App.DataBaseLocation);
                _conn.CreateTable<HistoryElem>();
                DateTime dateTime = DateTime.Now;
                HistoryElem historyElem = new HistoryElem
                {
                    id_Contact = _cont.id,
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