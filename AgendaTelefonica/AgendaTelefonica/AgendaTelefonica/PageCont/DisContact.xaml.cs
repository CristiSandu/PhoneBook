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
        private const bool V = true;
        Models.Contact _cont;
        SQLiteConnection _conn;
        public DisContact()
        {
            InitializeComponent();
            _cont = (Models.Contact)BindingContext;

        }

        public DisContact(Models.Contact contact)
        {
            InitializeComponent();
            BindingContext = contact;
            _cont = contact;
          
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            byte[] b = _cont.profilPicture;
            if (b != null)
            {
                Stream ms = new MemoryStream(b);
                contactPhoto.Source = ImageSource.FromStream(() => ms);
            }
            else
            {
                contactPhoto.Source = "contact_default.png";
                contactPhoto.Aspect = Aspect.AspectFit;
            }

            if (_cont.favorite == true)
            {
                starImg.Source = "favorite.svg";
            }
        }

        private async void backBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void callBtn_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn.Text == "Call")
            {
                if (Call(phoneNumber.Text))
                {
                    AddToHistory(false);
                }
            }else
            {
                if ( await SendEmail(email.Text))
                {
                    AddToHistory(true);
                }
            }

            await Navigation.PopToRootAsync();
        }

        public async void AddToHistory(bool isEmail)
        {
            _conn = new SQLiteConnection(App.DataBaseLocation);
            _conn.CreateTable<HistoryElem>();
            DateTime dateTime = DateTime.Now;
            HistoryElem historyElem = new HistoryElem
            {
                id_Contact = _cont.id,
                IsEmail = isEmail,
                date = dateTime
            };

            _conn.Insert(historyElem);
            _conn.Close();
        }


        public async Task<bool> SendEmail(string email)
        {
            var list_emails = new List<string> { email };
            try
            {
                var message = new EmailMessage
                {
                    To = list_emails,
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
                return false;
            }
            catch (Exception ex)
            {
                // Some other exception occurred
                return false;
            }

            return true;
        }

        public bool Call(string phoneNumber)
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

        private async void delete_Clicked(object sender, EventArgs e)
        {
            var contact = (Models.Contact)BindingContext;
            SQLiteConnection conn = contact.getConnection();
            if (contact != null)
                conn.Delete(contact);
            conn.Delete(contact);

            conn.Close();

            await Navigation.PopAsync();

        }
    }
}