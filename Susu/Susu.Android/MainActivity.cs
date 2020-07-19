using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace Susu.Droid
{
    [Activity(Label = "ESORR", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            //base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);
            Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));
            //var config = new PayPalConfiguration(PayPalEnvironment.Sandbox, "AUSkwXXhdMQBLTjWwTBHilMz4SvfZ_I5AbOA6icmIhX6_IVNWzoghjBP0AUTffaT2R4-UGIyIRLFRR-D")
            //{
            //    //If you want to accept credit cards
            //    AcceptCreditCards = true,
            //    //Your business name
            //    MerchantName = "ESORR",
            //    //Your privacy policy Url
            //    MerchantPrivacyPolicyUri = "https://www.example.com/privacy",
            //    //Your user agreement Url
            //    MerchantUserAgreementUri = "https://www.example.com/legal",
            //    // OPTIONAL - ShippingAddressOption (Both, None, PayPal, Provided)
            //    ShippingAddressOption = ShippingAddressOption.Both,
            //    // OPTIONAL - Language: Default languege for PayPal Plug-In
            //    Language = "us",
            //    // OPTIONAL - PhoneCountryCode: Default phone country code for PayPal Plug-In
            //    PhoneCountryCode = "52",
            //};
            //CrossPayPalManager.Init(config, this);
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            //PayPalManagerImplementation.Manager.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
           // PayPalManagerImplementation.Manager.Destroy();
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

