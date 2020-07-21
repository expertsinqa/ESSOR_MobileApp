
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Susu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InviteScreenPage : ContentPage
    {
        public InviteScreenPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}