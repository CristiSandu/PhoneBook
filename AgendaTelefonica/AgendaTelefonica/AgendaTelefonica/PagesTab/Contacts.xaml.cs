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
                await Navigation.PushAsync(new DisContact
                {
                    BindingContext = e.SelectedItem as Models.Contact
                });
            }
        }
    }
}