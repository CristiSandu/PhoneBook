using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AgendaTelefonica.PageCont;
using AgendaTelefonica.Models;
using SQLite;
using System.Collections.ObjectModel;
using AgendaTelefonica.Tools;

namespace AgendaTelefonica.PagesTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Contacts : ContentPage
    {
        Contact contact = new Contact();
        SQLiteConnection conn;
        private ObservableCollection<Models.Contact> _contacts;
        public Contacts()
        {
            InitializeComponent();
        }

        private async void addContact_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddContact());

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            conn = contact.getConnection();
            conn.CreateTable<Models.Contact>();
            var conts = conn.Table<Models.Contact>().ToList();
            _contacts = new ObservableCollection<Contact>(conts);

            contactsListView.ItemsSource = _contacts;
            conn.Close();
        }

        private async void contactsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Models.Contact cont = e.SelectedItem as Models.Contact;
                await Navigation.PushAsync(new DisContact(cont));
            }
        }

        private async void deleteAllCont_Clicked(object sender, EventArgs e)
        {
            /*conn = contact.getConnection();
            conn.CreateTable<Models.Contact>();
            conn.DeleteAll<Models.Contact>();
            conn.DeleteAll<HistoryElem>();
            conn.Close();*/

            await Navigation.PushAsync(new SearchPage());

        }
    }
}