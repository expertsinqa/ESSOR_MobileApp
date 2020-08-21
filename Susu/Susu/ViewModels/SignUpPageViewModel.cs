using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Navigation.Xaml;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using NavigationParameters = Prism.Navigation.NavigationParameters;

namespace Susu.ViewModels
{
    public class SignUpPageViewModel:ViewModelBase
    {
        #region Properties
        public string _FullName;
        public string FullName { get { return _FullName; } set { SetProperty(ref _FullName, value); } }

        public string _Email;
        public string Email { get { return _Email; } set { SetProperty(ref _Email, value); } }

        public string _Password;
        public string Password { get { return _Password; } set { SetProperty(ref _Password, value); } }

        public string _MobileNumber;
        public string MobileNumber { get { return _MobileNumber; } set { SetProperty(ref _MobileNumber, value); } }

        public Color _FullNamePlaceholderColor= Color.Gray;
        public Color FullNamePlaceholderColor { get { return _FullNamePlaceholderColor; } set { SetProperty(ref _FullNamePlaceholderColor, value); } }

        public Color _EmailPlaceholderColor = Color.Gray;
        public Color EmailPlaceholderColor { get { return _EmailPlaceholderColor; } set { SetProperty(ref _EmailPlaceholderColor, value); } }


        public Color _PasswordPlaceholderColor = Color.Gray;
        public Color PasswordPlaceholderColor { get { return _PasswordPlaceholderColor; } set { SetProperty(ref _PasswordPlaceholderColor, value); } }

        public Color _MobilePlaceholderColor = Color.Gray;
        public Color MobilePlaceholderColor { get { return _MobilePlaceholderColor; } set { SetProperty(ref _MobilePlaceholderColor, value); } }

        public ICommand CreateAccountClicked { get; set; }
        public ICommand LoginClicked { get; set; }

        public bool _IsSuccessMessageVisible = false;
        public bool IsSuccessMessageVisible { get { return _IsSuccessMessageVisible; } set { SetProperty(ref _IsSuccessMessageVisible, value); } }

        public ICommand CloseClicked { get { return new Command(close); } }

        public string _PaypalEmailId;
        public string PaypalEmailId { get { return _PaypalEmailId; } set { SetProperty(ref _PaypalEmailId, value); } }

        public Color _PaypalEmailIdPlaceholder = Color.Gray;
        public Color PaypalEmailIdPlaceholder { get { return _PaypalEmailIdPlaceholder; } set { SetProperty(ref _PaypalEmailIdPlaceholder, value); } }

        public string _FirstName;
        public string FirstName { get { return _FirstName; } set { SetProperty(ref _FirstName, value); } }

        public string _LastName;
        public string LastName { get { return _LastName; } set { SetProperty(ref _LastName, value); } }

        public Color _FirstNamePlaceholder = Color.Gray;
        public Color FirstNamePlaceholder { get { return _FirstNamePlaceholder; } set { SetProperty(ref _FirstNamePlaceholder, value); } }

        public Color _LastNamePlaceholderColor = Color.Gray;
        public Color LastNamePlaceholderColor { get { return _LastNamePlaceholderColor; } set { SetProperty(ref _LastNamePlaceholderColor, value); } }

        public string _ZelleId = "";
        public string ZelleId { get { return _ZelleId; } set { SetProperty(ref _ZelleId, value); } }

        public string _ConfirmPassword = "";
        public string ConfirmPassword { get { return _ConfirmPassword; } set { SetProperty(ref _ConfirmPassword, value); } }

        public Color _ConfirmPlaceholderColor = Color.Gray;
        public Color ConfirmPlaceholderColor { get { return _ConfirmPlaceholderColor; } set { SetProperty(ref _ConfirmPlaceholderColor, value); } }

        public ICommand PrivacyPlociyClicked { get { return new Command(PrivacyPolicy); } }

        public ImageSource _PrivacyPolicyImage = "check_box_empty.png";
        public ImageSource PrivacyPolicyImage { get { return _PrivacyPolicyImage; } set { SetProperty(ref _PrivacyPolicyImage, value); } }

        public ICommand CheckTermsandConditions { get { return new Command(checkprivacyPloicy); } }

        public bool isPolicyAccepted = false;

       
        #endregion
        #region Constructor
        public SignUpPageViewModel(INavigationService navigationService) :base(navigationService)
        {
            NavigationService = navigationService;
            CreateAccountClicked = new Command(CreateAccount);
            LoginClicked = new Command(Login);

        }
        #endregion
        #region Functions
        public async void CreateAccount()
        {
            if(string.IsNullOrEmpty(FirstName))
            {
                FirstNamePlaceholder = Color.Red;
                return;
            }
            else if(string.IsNullOrEmpty(LastName))
            {
                LastNamePlaceholderColor = Color.Red;
                return;
            }
            else if(string.IsNullOrEmpty(Email))
            {
                EmailPlaceholderColor = Color.Red;
                return;
            }
            else if(string.IsNullOrEmpty(Password))
            {
                PasswordPlaceholderColor = Color.Red;
                return;
            }
            else if (string.IsNullOrEmpty(ConfirmPassword))
            {
                ConfirmPlaceholderColor = Color.Red;
                return;
            }
            else if(string.IsNullOrEmpty(MobileNumber))
            {
                MobilePlaceholderColor = Color.Red;
                return;
            }
            else if (Password != ConfirmPassword)
            {
               await App.Current.MainPage.DisplayAlert("", "Password and confirm password should be same", "Ok");
                return;
            }
            else if (!isPolicyAccepted)
            {
                await App.Current.MainPage.DisplayAlert("", "Please accept terms & conditions", "Ok");
                return;
            }
            //else if(string.IsNullOrEmpty(PaypalEmailId))
            //{
            //    PaypalEmailIdPlaceholder = Color.Red;
            //    return;
            //}
            IsLoading = true;
            UserDto userDto = new UserDto();
            //userDto.FullName = FullName;
            userDto.Email = Email;
            userDto.UserPassword = Password;
            userDto.MobileNo = MobileNumber;
            userDto.PayPalEmailId = PaypalEmailId;
            userDto.FirstName = FirstName;
            userDto.LastName = LastName;
            userDto.ZelleId = ZelleId;
            UserDto usersdetails = await ServiceBase.SaveUser(userDto);
            
            if (usersdetails!=null && usersdetails.Id>0)
            {
               // IsSuccessMessageVisible = true;
                FullName = "";
                FirstName = "";
                LastName = "";
                Email = "";
                Password = "";
                MobileNumber = "";
                PaypalEmailId = "";
                ConfirmPassword = "";
                ZelleId = "";
                FirstNamePlaceholder = Color.Gray;
                LastNamePlaceholderColor = Color.Gray;
                EmailPlaceholderColor = Color.Gray;
                PasswordPlaceholderColor = Color.Gray;
                MobilePlaceholderColor = Color.Gray;
                ConfirmPlaceholderColor = Color.Gray;
                Dictionary<string, string> response = await ServiceBase.Login(usersdetails.Email, usersdetails.UserPassword);
                if (response != null && response.ContainsKey("access_token"))
                {
                    App.AccessToken = response["access_token"];
                    App.Current.Properties["Access_token"] = App.AccessToken;
                    await App.Current.SavePropertiesAsync();
                    if (response != null && response.ContainsKey("userDetails"))
                    {
                        UserDto user = new UserDto();
                        var res = response["userDetails"];
                        user = JsonConvert.DeserializeObject<UserDto>(res);
                        if (user != null && user.Id > 0)
                        {
                            App.UserId = user.Id;
                            App.Current.Properties["UserId"] = user.Id;
                            NavigationParameters np = new NavigationParameters();
                            np.Add("userDto", user);
                            await NavigationService.NavigateAsync("ServiceAggrement", np);
                        }
                        else
                        {
                            IsLoading = false;
                            await Application.Current.MainPage.DisplayAlert("Alert", "Something went wrong", "ok");
                        }
                    }
                    
                }
                else
                {
                    IsLoading = false;
                    await Application.Current.MainPage.DisplayAlert("Alert", "Something went wrong", "ok");
                }
                IsLoading = false;
            }
            else if(usersdetails==null)
            {
                IsLoading = false;
                await Application.Current.MainPage.DisplayAlert("Alert", "User already exists", "ok");
            }
            //else
            //{
            //    await App.Current.MainPage.DisplayAlert("", "Something went wrong", "OK");
            //}
        }
        public async void Login()
        {
           await NavigationService.NavigateAsync("LoginPage");
        }

        public void close()
        {
            IsSuccessMessageVisible = false;
        }

        private async void PrivacyPolicy(object obj)
        {
            await Browser.OpenAsync("https://esorr.com/privatepolicy.html");
        }

        private void checkprivacyPloicy()
        {
            if (PrivacyPolicyImage.ToString().Contains("check_box_empty.png"))
            {
                PrivacyPolicyImage = "check_box.png";
                isPolicyAccepted = true;
            }
            else
            {
                PrivacyPolicyImage = "check_box_empty.png";
                isPolicyAccepted = false;
            }
        }

        #endregion

    }
}
