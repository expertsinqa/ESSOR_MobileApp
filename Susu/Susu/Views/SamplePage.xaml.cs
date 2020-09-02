using Susu.CustomControl;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Susu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SamplePage : ContentPage
    {
        SamplePageViewModel viewModel;
        public SamplePage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            viewModel = BindingContext as SamplePageViewModel;
        }

        private void CustomPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (viewModel != null)
            {
                var picker = sender as Xamarin.Forms.Picker;
                string val = picker.SelectedItem.ToString();
                if (viewModel != null && viewModel.groupDto != null && val == "Daily")
                {
                    viewModel.groupDto.Contrbution_peroid = "Daily";
                    viewModel.groupDto.ContributionPeriod = "Daily";
                    viewModel.IsContributionDayVisible = false;
                    viewModel.IsContributionDateVisible = false;
                    viewModel.IsGroupStartDateVisible = false;
                    viewModel.IsGroupPayOutDayVisible = false;
                    //viewModel.IsPayoutDayVisible = false;
                }
                else if (viewModel != null && viewModel.groupDto != null && val == "Weekly")
                {
                    viewModel.groupDto.Contrbution_peroid = "Weekly";
                    viewModel.groupDto.ContributionPeriod = "Weekly";
                    viewModel.IsContributionDayVisible = true;
                    viewModel.IsContributionDateVisible = false;
                    viewModel.IsGroupStartDateVisible = true;
                    viewModel.IsGroupPayOutDayVisible = true;
                    viewModel.IsGroupPayOutDateVisible = false;
                    //viewModel.IsPayoutDayVisible = true;
                }
                else if (viewModel != null && viewModel.groupDto != null && val == "Bi-Weekly")
                {
                    viewModel.groupDto.Contrbution_peroid = "biweekly";
                    viewModel.groupDto.ContributionPeriod = "biweekly";
                    viewModel.IsContributionDayVisible = true;
                    viewModel.IsContributionDateVisible = false;
                    viewModel.IsGroupStartDateVisible = true;
                    viewModel.IsGroupPayOutDayVisible = true;
                    viewModel.IsGroupPayOutDateVisible = false;
                    //viewModel.IsPayoutDayVisible = true;
                }
                else
                {
                    if(val == "Monthly")
                    {
                        viewModel.groupDto.ContributionPeriod = "Monthly";
                    }
                    else if(val == "Semi-Monthly") {
                        viewModel.groupDto.ContributionPeriod = "semimonthly";
                    }
                    else if(val == "Yearly") {
                        viewModel.groupDto.ContributionPeriod = "Yearly";
                    }
                    else if(val == "Semi-Yearly")
                    {
                        viewModel.groupDto.ContributionPeriod = "semiyearly";
                    }
                    viewModel.IsContributionDateVisible = true;
                    viewModel.IsContributionDayVisible = false;
                    viewModel.IsGroupPayOutDayVisible = false;
                    viewModel.IsGroupPayOutDateVisible = true;
                    viewModel.IsGroupStartDateVisible = false;
                    //  viewModel.IsPayoutDayVisible = false;
                }
                //if (viewModel != null && viewModel.groupDto!=null && viewModel.groupDto.Contrbution_peroid != null && viewModel.groupDto.Contrbution_peroid == "Daily")
                //{
                //    viewModel.IsContributionDayVisible = false;
                //    viewModel.IsContributionDateVisible = false;
                //    viewModel.IsGroupStartDateVisible = false;
                //    viewModel.IsGroupPayOutDayVisible = false;
                //    //viewModel.IsPayoutDayVisible = false;
                //}
                //else if (viewModel!= null && viewModel.groupDto != null && viewModel.groupDto.Contrbution_peroid != null && viewModel.groupDto.Contrbution_peroid == "Weekly")
                //{
                //    viewModel.IsContributionDayVisible = true;
                //    viewModel.IsContributionDateVisible = false;
                //    viewModel.IsGroupStartDateVisible = true;
                //    viewModel.IsGroupPayOutDayVisible = true;
                //    viewModel.IsGroupPayOutDateVisible = false;
                //    //viewModel.IsPayoutDayVisible = true;
                //}
                //else if (viewModel != null && viewModel.groupDto != null && viewModel.groupDto.Contrbution_peroid != null && viewModel.groupDto.Contrbution_peroid == "Bi-Weekly")
                //{
                //    viewModel.IsContributionDayVisible = true;
                //    viewModel.IsContributionDateVisible = false;
                //    viewModel.IsGroupStartDateVisible = true;
                //    viewModel.IsGroupPayOutDayVisible = true;
                //    viewModel.IsGroupPayOutDateVisible = false;
                //    //viewModel.IsPayoutDayVisible = true;
                //}
                //else
                //{
                //    viewModel.IsContributionDateVisible = true;
                //    viewModel.IsContributionDayVisible = false;
                //    viewModel.IsGroupPayOutDayVisible = false;
                //    viewModel.IsGroupPayOutDateVisible = true;
                //    viewModel.IsGroupStartDateVisible = false;
                //    //  viewModel.IsPayoutDayVisible = false;
                //}
            }
        }

        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue)) return;

            if (!double.TryParse(e.NewTextValue, out double value))
            {
                ((CustomEntry)sender).Text = e.OldTextValue;
            }
        }
    }
}