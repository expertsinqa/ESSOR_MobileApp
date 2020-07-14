using ESORR.Models;
using ESORR.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ESORR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupPayoutDetails : ContentPage
    {
        GroupPayoutDetailsViewModel vm;
        public GroupPayoutDetails()
        {
            InitializeComponent();
            vm = BindingContext as GroupPayoutDetailsViewModel;
        }
        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            //var s = sender as Switch;
            //var item = s.Parent.BindingContext as UserPayInDetails;
            //if (item != null && item.isPaymentCompleted == false)
            //{
            //    if (e.Value)
            //        item.isPaymentCompleted = true;
            //    else
            //        item.isPaymentCompleted = false;
            //    if (vm != null)
            //    {
            //        vm.UpdatePayment(item);
            //    }
            //    else
            //    {
            //        vm = BindingContext as GroupContributionDetailPageViewModel;
            //        vm.UpdatePayment(item);
            //    }
            }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var s = sender as Image;
            var item = s.Parent.BindingContext as UserPayOutDetails;
            if (item != null && !item.isPaymentCompleted && item.IsEnabled)
            {
                item.isPaymentCompleted = true;
                vm.UpdatePayment(item);
            }
        }

        //}
    }
}