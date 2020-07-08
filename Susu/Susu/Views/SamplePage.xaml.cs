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
                if (viewModel != null && viewModel.groupDto!=null && viewModel.groupDto.ContributionPeriod!=null && viewModel.groupDto.ContributionPeriod == "Daily")
                {
                    viewModel.IsContributionDayVisible = false;
                    viewModel.IsContributionDateVisible = false;
                    viewModel.IsGroupStartDateVisible = false;
                    viewModel.IsGroupPayOutDayVisible = false;
                    //viewModel.IsPayoutDayVisible = false;
                }
                else if (viewModel!= null && viewModel.groupDto != null && viewModel.groupDto.ContributionPeriod != null && viewModel.groupDto.ContributionPeriod == "Weekly")
                {
                    viewModel.IsContributionDayVisible = true;
                    viewModel.IsContributionDateVisible = false;
                    viewModel.IsGroupStartDateVisible = true;
                    viewModel.IsGroupPayOutDayVisible = true;
                    viewModel.IsGroupPayOutDateVisible = false;
                    //viewModel.IsPayoutDayVisible = true;
                }
                else
                {
                    viewModel.IsContributionDateVisible = true;
                    viewModel.IsContributionDayVisible = false;
                    viewModel.IsGroupPayOutDayVisible = false;
                    viewModel.IsGroupPayOutDateVisible = true;
                    viewModel.IsGroupStartDateVisible = false;
                    //  viewModel.IsPayoutDayVisible = false;
                }
            }
        }
    }
}