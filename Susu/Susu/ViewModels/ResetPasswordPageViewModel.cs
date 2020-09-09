using Prism.Navigation;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Susu.Views
{
    public class ResetPasswordPageViewModel : ViewModelBase
    {
        #region Properties
        public string _NewPassword;
        public string NewPassword { get { return _NewPassword; } set { SetProperty(ref _NewPassword, value); } }

        public string _ConfirmPassword;
        public string ConfirmPassword { get { return _ConfirmPassword; } set { SetProperty(ref _ConfirmPassword, value); } }

        public string _OldPassword;

        public string OldPassword { get { return _OldPassword; } set { SetProperty(ref _OldPassword, value); } }

        public ICommand BackClicked { get { return new Command(Back); } }

        public ICommand UpdateClicked { get { return new Command(Update); } }

        public Color _NewPasswordPlaceholder = Color.FromHex("#083b66");
        public Color NewPasswordPlaceholder { get { return _NewPasswordPlaceholder; } set { SetProperty(ref _NewPasswordPlaceholder, value); } }

        public Color _ConfirmPlaceholder = Color.FromHex("#083b66");
        public Color ConfirmPlaceholder { get { return _ConfirmPlaceholder; } set { SetProperty(ref _ConfirmPlaceholder, value); } }

        public Color _oldPasswordPlaceholder = Color.FromHex("#083b66");
        public Color oldPasswordPlaceholder { get { return _oldPasswordPlaceholder; } set { SetProperty(ref _oldPasswordPlaceholder, value); } }

        #endregion

        #region Constructor
        public ResetPasswordPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Functions
        /// <summary>
        /// This method hit when user click on Back button
        /// </summary>
        public void Back()
        {
            NavigationService.GoBackAsync();
        }

        /// <summary>
        /// This method hit when user click on update button
        /// </summary>
        public async void Update()
        {
            if (Validation())
            {
                if (!string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword))
                {
                    if (NewPassword != ConfirmPassword)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Password and confirm password mismatch", "OK");
                    }
                    else
                    {
                        bool ispasswordReset = await ServiceBase.ResetPassword(App.UserId, OldPassword, NewPassword);
                        if (ispasswordReset)
                        {
                            if (await App.Current.MainPage.DisplayAlert("", "Password updated Successfully.\n Please login again with new password", "OK", " "))
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
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("", "Something went wrong,please contact admin", "OK");
                        }
                    }
                }
            }
            else
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Method to validate required fields
        /// </summary>
        /// <returns></returns>
        public bool Validation()
        {
            int count = 0;
            if (string.IsNullOrEmpty(OldPassword))
            {
                oldPasswordPlaceholder = Color.Red;
                count++;
            }
            else
            {
                oldPasswordPlaceholder = Color.FromHex("#083b66");
            }
            if (string.IsNullOrEmpty(NewPassword))
            {
                NewPasswordPlaceholder = Color.Red;
                count++;
            }
            else
            {
                NewPasswordPlaceholder = Color.FromHex("#083b66");
            }
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                ConfirmPlaceholder = Color.Red;
                count++;
            }
            else
            {
                ConfirmPlaceholder = Color.FromHex("#083b66");
            }
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
