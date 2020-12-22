using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using AgendaTelefonica.Models;
using System.Text.RegularExpressions;
using SQLite;
using System.Net.Mail;

namespace AgendaTelefonica.PageCont
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddContact : ContentPage
    {
        SQLiteConnection conn;
        public AddContact()
        {
            InitializeComponent();
            contactPhoto.Source = "contact_default.svg";

        }


        private async void addPhoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result_photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Pick a photo!"
                });
                var stream = await result_photo.OpenReadAsync();
                contactPhoto.Source = ImageSource.FromStream(() => stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        private async void saveBtn_Clicked(object sender, EventArgs e)
        {
            Models.Contact cont = new Models.Contact
            {
                firstName = numeContact.Text,
                secondName = prenumeContact.Text,
                phoneNumber = nrTelefon.Text,
                email = email.Text,
                favorite = addFavorite.IsEnabled,
            };
            string patternPhoneNumber = "(0[0-9]{9})$";
            //string patternEmailAddress = "^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$";

            Match matchPhoneNumber = Regex.Match(nrTelefon.Text, patternPhoneNumber);
            

            if (!matchPhoneNumber.Success || !IsValid(email.Text))
            {
                if (!matchPhoneNumber.Success)
                    await DisplayAlert("Atentie!", "Numarul introdus nu este valid!", "OK");
                if (!IsValid(email.Text))
                    await DisplayAlert("Atentie!", "Email-ul introdus nu este valid!", "OK");

            }
            else
            {
                conn = cont.getConnection();
                conn.CreateTable<Models.Contact>();
                int rows;
                rows = conn.Insert(cont);

                if (rows > 0)
                    await DisplayAlert("Success", "Contact successfully added", "OK");
                else
                    await DisplayAlert("Error", "Failed to add the contact", "OK");

                conn.Close();

                await Navigation.PopAsync();
            }
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void deleteBtn_Clicked(object sender, EventArgs e)
        {

        }

        private async void cancelBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}