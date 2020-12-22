using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AgendaTelefonica.PageCont;
using AgendaTelefonica.PageCont;

namespace AgendaTelefonica.PagesTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class History : ContentPage
    {
        public History()
        {
            InitializeComponent();
        }

        private async void dispContact_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DisContact());
        }
    }
}