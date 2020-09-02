using Prism;
using Prism.Ioc;
using Susu.ViewModels;
using Susu.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using Prism.Mvvm;
using ESORR.Views;
using ESORR.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Susu
{
    [AutoRegisterForNavigation]
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }
        public static string AccessToken { get; set; }
        public static long UserId { get; set; }

        public static long GroupId { get; set; }

        public static bool IsGroupAdmin { get; set; } = false;

        public static int contributionId { get; set; }

        public static int GroupNumber { get; set; }

        public static bool IsAggreementAccepted { get; set; }

        public static bool IsProfilePhotoUploaded { get; set; }

        public static string Amount { get; set; }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            //await NavigationService.NavigateAsync("ResetPasswordPage");
            if (App.Current.Properties.ContainsKey("Access_token"))
            {
                if (!string.IsNullOrEmpty(App.Current.Properties["Access_token"].ToString()))
                {
                    App.AccessToken = App.Current.Properties["Access_token"].ToString();
                    if (App.Current.Properties.ContainsKey("UserId"))
                        App.UserId = long.Parse(App.Current.Properties["UserId"].ToString());
                    if (App.Current.Properties.ContainsKey("GroupId"))
                        App.GroupId = long.Parse(App.Current.Properties["GroupId"].ToString());
                    if (App.Current.Properties.ContainsKey("GroupAdmin"))
                        App.IsGroupAdmin = bool.Parse(App.Current.Properties["GroupAdmin"].ToString());
                    if (App.Current.Properties.ContainsKey("IsAggrementAccepted"))
                        App.IsAggreementAccepted = bool.Parse(App.Current.Properties["IsAggrementAccepted"].ToString());
                    if (App.Current.Properties.ContainsKey("IsProfileUpdated"))
                        App.IsProfilePhotoUploaded = bool.Parse(App.Current.Properties["IsProfileUpdated"].ToString());
                    if(!App.IsAggreementAccepted)
                    {
                        await NavigationService.NavigateAsync("ServiceAggrement");
                        return;
                    }
                    if(!App.IsProfilePhotoUploaded)
                    {
                        await NavigationService.NavigateAsync("UploadIdProof");
                        return;
                    }
                    if(App.GroupId>0)
                    {
                        await NavigationService.NavigateAsync("HomePage");
                    }
                    else
                    {
                        await NavigationService.NavigateAsync("LandingPage");
                    }
                   
                }
            }
            else
            {
                await NavigationService.NavigateAsync("LoginPage");
            }


        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<LoadContactsFromMobilePage, LoadContactsFromMobilePageViewModel>();
            containerRegistry.RegisterForNavigation<SamplePage, SamplePageViewModel>();
            containerRegistry.RegisterForNavigation<InviteScreenPage, InviteScreenPageViewModel>();
            ViewModelLocationProvider.Register<PaymentsPage, PaymentsPageViewModel>();
            containerRegistry.RegisterForNavigation<ResetPasswordPage, ResetPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupContributionDetailPage, GroupContributionDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage,HomePageViewModel>();
            containerRegistry.RegisterForNavigation<MorePage, MorePageViewModel>();
            //containerRegistry.RegisterForNavigation<PaymentsPage, PaymentsPageViewModel>
        }
    }
}
