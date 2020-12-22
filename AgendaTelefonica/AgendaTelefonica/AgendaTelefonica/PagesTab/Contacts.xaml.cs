using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AgendaTelefonica.PageCont;

namespace AgendaTelefonica.PagesTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Contacts : ContentPage
    {
        public Contacts()
        {
            InitializeComponent();
        }

        private async void addContact_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddContact());

        }
    }
}