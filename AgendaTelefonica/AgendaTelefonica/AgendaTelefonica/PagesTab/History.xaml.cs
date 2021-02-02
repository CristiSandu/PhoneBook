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
            _historyList = new ObservableCollection<HistoryDispMod>();
            foreach (HistoryElem h in historyElements)
            {
                var getContactFromTel = _conn.Query<Models.Contact>("SELECT * FROM Contact WHERE id = ?", h.id_Contact);
                var elem = getContactFromTel.ToList<Models.Contact>();

                if (h.id_Contact == -1)
                    nameContact = $"Call {h.phoneNumber}";
                else
                    nameContact = $"Call {elem[0].firstName} - {elem[0].secondName}";

                HistoryDispMod hdm = new HistoryDispMod
                    {
                        Name = nameContact,
                        Date = h.date.ToString()
                    };

                _historyList.Add(hdm);
            }

            historyListView.ItemsSource = _historyList;
        }

        private async void dispContact_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Tools.SeatingsPage());
        }

        private void historyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void callNumber_Clicked(object sender, EventArgs e)
        {

        }

        private void searchBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void addContBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void settingsBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}