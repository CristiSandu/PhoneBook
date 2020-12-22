using AgendaTelefonica.Models;
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

        private void favoriteListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}