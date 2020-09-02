using Prism.Navigation;
using Susu;
using Susu.ViewModels;
using Susu.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ESORR.ViewModels
{
    public class MorePageViewModel: ViewModelBase
    {
        public bool _IsHelppopupVisible = false;
        public bool IsHelppopupVisible { get { return _IsHelppopupVisible; } set { SetProperty(ref _IsHelppopupVisible, value); } }

        public ICommand ResetPasswordCliked
        {
            get { return new Command(ResetPassword); }
        }
        public ICommand HelpClicked
        {
            get { return new Command(Help); }
        }

       
        public ICommand LogoutClicked
        {
            get { return new Command(LogOut); }
        }

        public ICommand HelpCloseClicked
        {
            get{ return new Command(CloseHelp); }
        }
        public ICommand BackClicked
        {
            get { return new Command(Back); }
        }

        private void Back(object obj)
        {
            NavigationService.GoBackAsync();
        }

        public MorePageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
        private async void ResetPassword()
        {
                await NavigationService.NavigateAsync("ResetPasswordPage");
        }
        private void Help()
        {
            IsHelppopupVisible = true;
        }

        private void LogOut()
        {
            App.Current.Properties.Clear();
            App.IsGroupAdmin = false;
            App.UserId = 0;
            App.GroupId = 0;
            App.AccessToken = null;
            App.GroupNumber = 0;
            App.contributionId = 0;
            App.IsAggreementAccepted = false;
            App.IsProfilePhotoUploaded = false;
            App.GroupNumber = 0;
            App.Current.MainPage = new LoginPage();
            //DependencyService.Get<ICloseApplication>().closeApplication();
        }

        private void CloseHelp()
        {
            IsHelppopupVisible = false;
        }
        
    }
}
