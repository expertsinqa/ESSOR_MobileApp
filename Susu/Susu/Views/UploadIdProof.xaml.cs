
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Susu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadIdProof : ContentPage
    {
        public UploadIdProof()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}