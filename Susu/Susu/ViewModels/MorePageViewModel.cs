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
        #region Properties
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
        #endregion
       

        #region Constructor
        public MorePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Fuctions
        /// <summary>
        /// This method hit when user click on Back button
        /// </summary>
        /// <param name="obj"></param>
        private void Back(object obj)
        {
            NavigationService.GoBackAsync();
        }
        /// <summary>
        /// This method hit when user click on Reset Password
        /// </summary>
        private async void ResetPassword()
        {
                await NavigationService.NavigateAsync("ResetPasswordPage");
        }

        /// <summary>
        /// This method hit when user click on help button
        /// </summary>
        private void Help()
        {
            IsHelppopupVisible = true;
        }

        /// <summary>
        /// This method hit when user click on Logout button
        /// </summary>
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

        /// <summary>
        /// This method hit when user click on close help popup
        /// </summary>
        private void CloseHelp()
        {
            IsHelppopupVisible = false;
        }
        #endregion
    }
}
