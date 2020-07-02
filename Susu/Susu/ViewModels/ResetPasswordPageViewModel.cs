using Prism.Navigation;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Susu.Views
{
    public class ResetPasswordPageViewModel:ViewModelBase
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

        public Color _NewPasswordPlaceholder;
        public Color NewPasswordPlaceholder { get { return _NewPasswordPlaceholder; } set { SetProperty(ref _NewPasswordPlaceholder, value); } }

        public Color _ConfirmPlaceholder;
        public Color ConfirmPlaceholder { get { return _ConfirmPlaceholder; } set { SetProperty(ref _ConfirmPlaceholder, value); } }

        public Color _oldPasswordPlaceholder = Color.Gray;
        public Color oldPasswordPlaceholder { get { return _oldPasswordPlaceholder; } set { SetProperty(ref _oldPasswordPlaceholder, value); } }

        #endregion
        public ResetPasswordPageViewModel(INavigationService navigationService):base(navigationService)
        {
            NavigationService = navigationService;
        }

        public void Back()
        {
            NavigationService.GoBackAsync();
        }

        public async void Update()
        {
            if(string.IsNullOrEmpty(OldPassword))
            {
                oldPasswordPlaceholder = Color.Red;
            }
            else if (string.IsNullOrEmpty(NewPassword))
            {
                NewPasswordPlaceholder = Color.Red;
            }
            else if (string.IsNullOrEmpty(ConfirmPassword))
            {
                ConfirmPlaceholder = Color.Red;
            }
            else if(!string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword))
            {
                if(NewPassword != ConfirmPassword)
                {
                  await  App.Current.MainPage.DisplayAlert("", "Password and confirm password mismatch", "OK");
                }
                else
                {
                    bool ispasswordReset = await ServiceBase.ResetPassword(App.UserId,OldPassword, NewPassword);
                    if(ispasswordReset)
                    {
                        if(await App.Current.MainPage.DisplayAlert("", "Password updated Successfully", "OK"," "))
                        {
                            await NavigationService.NavigateAsync("SamplePage");
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }
}
