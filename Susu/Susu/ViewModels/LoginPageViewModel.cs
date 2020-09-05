using ESORR.Interface;
using Newtonsoft.Json;
using Prism.Navigation;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Susu.Models.Enums;

namespace Susu.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Properties

        public string _Email;
        public string Email { get { return _Email; } set { SetProperty(ref _Email, value); } }

        public string _Password;
        public string Password { get { return _Password; } set { SetProperty(ref _Password, value); } }

        public Color _EmailPlaceholder = Color.FromHex("#083b66");
        public Color EmailPlaceholder { get { return _EmailPlaceholder; } set { SetProperty(ref _EmailPlaceholder, value); } }

        public Color _PasswordPlaceholder = Color.FromHex("#083b66");
        public Color PasswordPlaceholder { get { return _PasswordPlaceholder; } set { SetProperty(ref _PasswordPlaceholder, value); } }

        public ICommand ForgotpasswordClicked { get { return new Command(ForgotPassword); } }

        public ICommand SignUpClicked { get; set; }

        public ICommand LoginClicked
        {
            get { return new Command(Login); }
        }

        public bool _IsForgotPasswordVisible = false;
        public bool IsForgotPasswordVisible { get { return _IsForgotPasswordVisible; } set { SetProperty(ref _IsForgotPasswordVisible, value); } }

        public ICommand CloseForgetPasswordClicked
        {
            get { return new Command(CloseForgetPassword); }
        }

        public ICommand SubmitForgotPasswordClicked
        {
            get { return new Command(SubmitForgetPassword); }
        }

        public string _UserMail;
        public string UserMail { get { return _UserMail; } set { SetProperty(ref _UserMail, value); } }

        public Color _ForgotPasswordEmailPlaceholderColor = Color.FromHex("#083b66");
        public Color ForgotPasswordEmailPlaceholderColor { get { return _ForgotPasswordEmailPlaceholderColor; } set { SetProperty(ref _ForgotPasswordEmailPlaceholderColor, value); } }

        public ICommand UpdateAppClicked { get { return new Command(AppUpdate); } }
        public ICommand CancelAppClicked { get { return new Command(Cancel); } }

        public bool _IsAppUpdateVisible = false;
        public bool IsAppUpdateVisible { get { return _IsAppUpdateVisible; } set { SetProperty(ref _IsAppUpdateVisible, value); } }
        public string _AppUpdateText = "";
        public string AppUpdateText { get { return _AppUpdateText; } set { SetProperty(ref _AppUpdateText, value); } }
        IFirebaseAnalytics eventTracker;


        #endregion

        #region Constructor
        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SignUpClicked = new Command(SignUp);
            GetAppVersionDetails();
            eventTracker = DependencyService.Get<IFirebaseAnalytics>();

        }
        #endregion

        #region Functions
        public async void SignUp()
        {
            await NavigationService.NavigateAsync("SignUpPage");
        }

        public  async void  GetAppVersionDetails()
        {
            List<APPVersionDetails> appVersionDetails = new List<APPVersionDetails>();
            try
            {
                IsLoading = true;
                appVersionDetails = await ServiceBase.GetAppVesrion();
                if (appVersionDetails != null && appVersionDetails.Count > 0)
                {
                    var currentVersion = VersionTracking.CurrentVersion;
                    if (!string.IsNullOrEmpty(currentVersion))
                    {
                        double version = double.Parse(currentVersion);
                        if(Device.RuntimePlatform == Device.Android)
                        {
                            double androidVersion = 0;
                            if (appVersionDetails[0]!=null && !string.IsNullOrEmpty(appVersionDetails[0].VersionNUmber.ToString()))
                            {
                                androidVersion = appVersionDetails[0].VersionNUmber;
                            }
                            if (version < androidVersion)
                            {
                                AppUpdateText = "An updated version of app is available.";
                                IsAppUpdateVisible = true;
                            }
                            else
                            {
                                IsAppUpdateVisible = false;
                            }
                        }
                        else
                        {
                            double iosVersion = 0;
                            if (appVersionDetails[1] != null && !string.IsNullOrEmpty(appVersionDetails[1].VersionNUmber.ToString()))
                                 iosVersion = appVersionDetails[1].VersionNUmber;
                            if (version < iosVersion)
                            {
                                AppUpdateText = "An updated version of app is available.";
                                IsAppUpdateVisible = true;
                            }
                            else
                            {
                                IsAppUpdateVisible = false;
                            }
                        }
                        //if (version < appVersionDetails[0].VersionNUmber)
                        //{
                        //    if (Device.RuntimePlatform == Device.Android)
                        //    {
                        //        AppUpdateText = "An updated version of app is available.";
                        //        IsAppUpdateVisible = true;
                        //    }
                        //    else
                        //    {
                        //        //AppUpdateText = "An updated version of app is available.";
                        //        //IsAppUpdateVisible = true;
                        //    }
                        //    //IsAppUpdateVisible = true;
                        //}
                        //else
                        //{
                        //    IsAppUpdateVisible = false;
                        //}
                    }
                    IsLoading = false;
                }
                else
                {
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
            }
           
        }
        public async void Login()
        {
            UserDto userDto = null;
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailPlaceholder = Color.Red;
                return;
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordPlaceholder = Color.Red;
                return;
            }
            else if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {
                const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
                if ((Regex.IsMatch(Email, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))))
                {
                    IsLoading = true;
                    Dictionary<string, string> response = await ServiceBase.Login(Email, Password);
                    if (response != null && response.ContainsKey("access_token"))
                    {
                        App.AccessToken = response["access_token"];
                    }
                    if (response != null && response.ContainsKey("userDetails"))
                    {
                        userDto = new UserDto();
                        var res = response["userDetails"];
                        userDto = JsonConvert.DeserializeObject<UserDto>(res);
                    }
                    if (userDto != null && userDto.Id > 0)
                    {
                        eventTracker.SendEvent("LoginSuccessfull");
                        App.Current.Properties["UserId"] = userDto.Id;
                        //App.Current.Properties["UserName"] = userDto.Email;
                        //App.Current.Properties["Password"] = userDto.UserPassword;
                        App.Current.Properties["Access_token"] = App.AccessToken;
                        App.UserId = userDto.Id;
                        App.GroupId = userDto.GroupId != null ? (long)userDto.GroupId : 0;
                        App.Current.Properties["GroupId"] = App.GroupId;
                        int? RoledId = userDto.RoleId != null ? userDto.RoleId : 0;
                        if (userDto.IsAcceptAggrement)
                            App.Current.Properties["IsAggrementAccepted"] = true;
                        else
                            App.Current.Properties["IsAggrementAccepted"] = false;

                        if (userDto.ProofFilePath != null)
                            App.Current.Properties["IsProfileUpdated"] = true;
                        else
                            App.Current.Properties["IsProfileUpdated"] = false;
                        await App.Current.SavePropertiesAsync();
                        if (RoledId == (int)Roles.groupadmin)
                        {
                            App.IsGroupAdmin = true;
                            App.Current.Properties["GroupAdmin"] = true;
                        }
                        await App.Current.SavePropertiesAsync();
                        if (!userDto.IsAcceptAggrement)
                        {
                            NavigationParameters np = new NavigationParameters();
                            np.Add("userDto", userDto);
                            await NavigationService.NavigateAsync("ServiceAggrement", np);
                        }
                        else if (userDto.GroupId > 0)
                        {
                            await NavigationService.NavigateAsync("HomePage");
                        }
                        else if (userDto.ProofFilePath == null)
                        {
                            NavigationParameters np = new NavigationParameters();
                            np.Add("userDto", userDto);
                            await NavigationService.NavigateAsync("UploadIdProof", np);
                        }
                        else
                        {
                            await NavigationService.NavigateAsync("LandingPage");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Enter valid email and password", "ok");
                    }
                    await App.Current.SavePropertiesAsync();
                    IsLoading = false;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Enter vaild EmailId", "Ok");
                    return;
                }
            }

        }
        public void ForgotPassword()
        {
            IsLoading = true;
            UserMail = string.Empty;
            ForgotPasswordEmailPlaceholderColor = Color.FromHex("#083b66");
            IsForgotPasswordVisible = true;
            IsLoading = false;
        }

        private void CloseForgetPassword()
        {
            IsForgotPasswordVisible = false;
        }
        private async void SubmitForgetPassword()
        {
            if (!string.IsNullOrWhiteSpace(UserMail) && !IsLoading)
            {
                IsLoading = true;
                bool result = await ServiceBase.ForgotPassword(UserMail);
                if (result)
                {
                    if (await App.Current.MainPage.DisplayAlert("", "Verification code send to your enterd emailId", "OK", " "))
                    {
                        IsForgotPasswordVisible = false;
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("", "Please enter a valid EmailId", "OK");
                }
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
                ForgotPasswordEmailPlaceholderColor = Color.Red;
            }
        }

        private async void AppUpdate()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.esorr.esorrApp&hl=en_IN");
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                IsAppUpdateVisible = false;
            }
            else
            {
                Uri uri = new Uri("https://apps.apple.com/us/app/id1523820384");
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                IsAppUpdateVisible = false;
            }
        }
        private void Cancel()
        {
            IsAppUpdateVisible = false;
        }
        #endregion
    }
}
