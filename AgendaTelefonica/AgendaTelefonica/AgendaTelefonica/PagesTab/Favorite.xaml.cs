using AgendaTelefonica.Models;
using AgendaTelefonica.PageCont;
using AgendaTelefonica.Tools;
using Microcharts;
using SkiaSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AgendaTelefonica.PagesTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Favorite : ContentPage
    {
        Contact contact = new Contact();
        SQLiteConnection conn;
        private ObservableCollection<Models.Contact> _contacts;

        public Favorite()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            conn = contact.getConnection();
            conn.CreateTable<Models.Contact>();
            var favoriteContacts = conn.Query<Models.Contact>("SELECT * FROM Contact WHERE favorite = ?", true);
            var conts = favoriteContacts.ToList();

            _contacts = new ObservableCollection<Contact>(conts);

            favoriteListView.ItemsSource = _contacts;
            conn.Close();
        }

        private async void favoriteListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cont = e.CurrentSelection.FirstOrDefault() as Contact;
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new DisContact(cont));
            }
        }

        private async void callNumber_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DialPage());
        }

        private async void settingsBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SeatingsPage());
        }

        private async void addContBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddContact());
        }

        private async void searchBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

       
    }



}