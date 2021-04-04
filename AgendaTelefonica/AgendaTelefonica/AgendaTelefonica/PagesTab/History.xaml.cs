using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AgendaTelefonica.PageCont;
using AgendaTelefonica.PageCont;
using AgendaTelefonica.Models;
using SQLite;
using System.Collections.ObjectModel;
using AgendaTelefonica.Tools;

namespace AgendaTelefonica.PagesTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class History : ContentPage
    {
        Models.Contact _cont = new Models.Contact();
        SQLiteConnection _conn;
        ObservableCollection<HistoryDispMod> _historyList;

        public History()
        {
            InitializeComponent();
            _historyList = new ObservableCollection<HistoryDispMod>();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            string nameContact;
            _conn = _cont.getConnection();
            _conn.CreateTable<Models.Contact>();
            _conn.CreateTable<HistoryElem>();
            var historyElements = _conn.Table<HistoryElem>().ToList();
            var operation = "Call";
            _historyList = new ObservableCollection<HistoryDispMod>();
            foreach (HistoryElem h in historyElements)
            {
                var getContactFromTel = _conn.Query<Models.Contact>("SELECT * FROM Contact WHERE id = ?", h.id_Contact);
                var elem = getContactFromTel.ToList<Models.Contact>();

                operation = h.IsEmail ? "Email" : "Call";
                nameContact = h.id_Contact == -1 ? $"{operation} {h.phoneNumber}" : $"{operation} {elem[0].firstName} - {elem[0].secondName}";

                HistoryDispMod hdm = new HistoryDispMod
                {
                    Name = nameContact,
                    Date = h.date.ToString()
                };

                _historyList.Add(hdm);
            }

            historyListView.ItemsSource = _historyList;
            nrOfCalls.Text = $"Number of Calls: {_historyList.Count}";
        }

        private async void dispContact_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Tools.SeatingsPage());
        }

        private void historyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void callNumber_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DialPage());
        }

        private async void searchBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        private async void addContBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddContact());
        }

        private async void settingsBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SeatingsPage());
        }
    }
}