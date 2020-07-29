using Prism.Navigation;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ESORR.ViewModels
{
    public class HomePageViewModel: ViewModelBase
    {
        #region Properties
        public ICommand GroupInfoClicked { get { return new Command(GroupInfo); } }
        public ICommand GroupUsersClicked { get { return new Command(GroupUsers); } }
        public ICommand PaymentsClicked { get { return new Command(Payment); } }
        public ICommand ProfileClicked { get { return new Command(Profile); } }
        public ICommand MoreClicked { get { return new Command(More); } }

        



        #endregion
        #region Constructor
        public HomePageViewModel(INavigationService navigationService): base(navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion
        #region Functions
        private void GroupInfo()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "GroupInfo");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }
        private void GroupUsers()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "GroupUsers");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }
        private void More()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "More");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }

        private void Profile()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "Profile");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }

        private void Payment()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "Payment");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }

        #endregion
    }
}
