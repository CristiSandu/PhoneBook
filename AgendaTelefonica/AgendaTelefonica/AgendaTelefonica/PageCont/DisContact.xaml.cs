using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AgendaTelefonica.PageCont;

namespace AgendaTelefonica.PageCont
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisContact : ContentPage
    {
        public DisContact()
        {
            InitializeComponent();
        }

        private async void backBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}