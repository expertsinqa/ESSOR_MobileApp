﻿
using Susu.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Susu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateGroupPage : ContentPage
    {
        CreateGroupPageViewModel viewModel;
        public CreateGroupPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            viewModel = BindingContext as CreateGroupPageViewModel;

        }

        private void CustomPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (viewModel != null)
            {
                if (viewModel != null && viewModel.selectedPeriod == "Daily")
                {
                    viewModel.IsContributionDayVisible = false;
                    viewModel.IsContributionDateVisible = false;
                    viewModel.IsGroupStartDateVisible = false;
                    viewModel.IsPayoutDayVisible = false;
                    //viewModel.IsPayoutDayVisible = false;
                }
                else if (viewModel.selectedPeriod != null && viewModel.selectedPeriod == "Weekly")
                {
                    viewModel.IsContributionDayVisible = true;
                    viewModel.IsContributionDateVisible = false;
                    viewModel.IsGroupStartDateVisible = true;
                    viewModel.IsPayoutDayVisible = true;
                    //viewModel.IsPayoutDayVisible = false;
                    //viewModel.IsPayoutDayVisible = true;
                }
                else
                {
                    viewModel.IsContributionDateVisible = true;
                    viewModel.IsContributionDayVisible = false;
                    viewModel.IsPayoutDayVisible = false;
                    viewModel.IsPayoutDateVisible = true;
                    viewModel.IsGroupStartDateVisible = false;
                  //  viewModel.IsPayoutDayVisible = false;
                }
            }
        }

        private void CustomDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.SelectedDate = e.NewDate;
        }

        private void GroupSartdate(object sender, DateChangedEventArgs e)
        {
            viewModel.GroupStartDate = e.NewDate;
        }

        private void PayoutDate(object sender, DateChangedEventArgs e)
        {
            viewModel.payoutDate = e.NewDate;
        }

        //private void PayoutDay(object sender, DateChangedEventArgs e)
        //{

        //}
    }
}