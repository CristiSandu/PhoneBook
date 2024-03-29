﻿using AgendaTelefonica.PageCont;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgendaTelefonica.Models;
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

            searchBarContacts.Focus();
            SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation);
            var contacts = conn.Table<Models.Contact>().ToList();
            searchBarContacts.Placeholder = $"Search in {contacts.Count()} contacts";
            _contacts = new ObservableCollection<Models.Contact>(contacts);

            conn.Close();
        }

        private async void contactsSearchListView_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            var cont = e.CurrentSelection.FirstOrDefault() as Contact;
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new DisContact(cont));
            }
        }

        private void searchBarContacts_TextChanged(object sender, TextChangedEventArgs e)
        {
            contactsSearchListView.ItemsSource = GetArtists(e.NewTextValue);
        }

        private IEnumerable GetArtists(string newTextValue = null)
        {
            SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation);
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}