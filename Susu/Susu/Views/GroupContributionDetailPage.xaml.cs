using ESORR.Models;
using ESORR.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace ESORR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupContributionDetailPage : ContentPage
    {
        GroupContributionDetailPageViewModel vm;
        public GroupContributionDetailPage()
        {
            InitializeComponent();
            vm = BindingContext as GroupContributionDetailPageViewModel;
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var s= sender as Switch;
            var item = s.Parent.BindingContext as UserPayInDetails;
            if (item!=null && item.isPaymentCompleted == false)
            {
                if (e.Value)
                    item.isPaymentCompleted = true;
                else
                    item.isPaymentCompleted = false;
                if (vm != null)
                {
                    vm.UpdatePayment(item);
                }
                else
                {
                    vm = BindingContext as GroupContributionDetailPageViewModel;
                    vm.UpdatePayment(item);
                }
            }

        }

        private void TapedSpecificUser(object sender, EventArgs e)
        {
            var s = sender as Image;
            var item = s.Parent.BindingContext as UserPayInDetails;
            if(item!=null && !item.isPaymentCompleted)
            {
                item.isPaymentCompleted = true;
                vm.UpdatePayment(item);
            }
          
        }

        private void AllUsersTaped(object sender, EventArgs e)
        {
            var s = sender as Image;
            var selectedImage = s.Source as FileImageSource;

            if (selectedImage.File == "check_box.png")
            {
                DisplayAlert("", "All payments are done", "OK");
            }
            else
            {
                var item = s.Parent.BindingContext as UserPayInDetails;
                vm.UpdateAllpayments();
            }
          
        }
    }
}