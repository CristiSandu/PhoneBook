using AgendaTelefonica.PageCont;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AgendaTelefonica.Tools
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        ObservableCollection<Models.Contact> _contacts;
        public SearchPage()
        {
            InitializeComponent();
           
        }

         
        protected override void OnAppearing()
        {
            base.OnAppearing();
            focus();
            SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation);
            //conn.CreateTable<Artists>();
            var contacts = conn.Table<Models.Contact>().ToList();
            searchBarContacts.Placeholder = $"Search in {contacts.Count()} contacts";
            _contacts = new ObservableCollection<Models.Contact>(contacts);
            contactsSearchListView.ItemsSource = _contacts; 
             conn.Close();
        }

        private async void contactsSearchListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var cont = e.SelectedItem as Models.Contact;
                await Navigation.PushAsync(new DisContact(cont));
            }
        }

        public async void focus()
        {
            searchBarContacts.Focus();
        }


        private void searchBarContacts_TextChanged(object sender, TextChangedEventArgs e)
        {
            contactsSearchListView.ItemsSource = GetArtists(e.NewTextValue);
        }

        private IEnumerable GetArtists(string newTextValue = null)
        {
            // var artists = editeartist.createList();
            SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation);
            //conn.CreateTable<Artists>();
            var contacts = conn.Table<Models.Contact>().ToList();
            conn.Close();
            if (String.IsNullOrWhiteSpace(newTextValue))
                return contacts;

            return contacts.Where(c => c.firstName.StartsWith(RemoveDiacritics(newTextValue), true, null));
        }

        public string RemoveDiacritics(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        private void searchBarContacts_SearchButtonPressed(object sender, EventArgs e)
        {

        }
    }
}