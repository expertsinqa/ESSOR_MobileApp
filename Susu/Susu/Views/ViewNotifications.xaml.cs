using Susu.Models;
using Susu.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Susu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewNotifications : ContentPage
    {
        ViewNotificationsViewModel vm;
        public ViewNotifications()
        {
            vm = BindingContext as ViewNotificationsViewModel;
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            EmailNotificatinDetailsDto emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
            emailNotificatinDetailsDto = (EmailNotificatinDetailsDto)e.Item;
            if (emailNotificatinDetailsDto != null)
            {
                if (vm != null)
                {
                    vm.ViewNotification(emailNotificatinDetailsDto);
                }
                else
                {
                    vm = BindingContext as ViewNotificationsViewModel;
                    vm.ViewNotification(emailNotificatinDetailsDto);
                }
            }
           
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}