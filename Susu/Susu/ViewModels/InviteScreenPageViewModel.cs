using Prism.Navigation;
using Susu.ViewModels;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Susu.Views
{
    public class InviteScreenPageViewModel : ViewModelBase,INavigationAware
    {
        public ICommand InviteClicked { get { return new Command(Invite); } }

        public ICommand NextClicked { get { return new Command(Next); } }
        public string _GroupCode = string.Empty;
        public string GroupCode { get { return _GroupCode; } set { SetProperty(ref _GroupCode, value); } }

        public string _GroupName;
        public string GroupName { get { return _GroupName; } set { SetProperty(ref _GroupName, value); } }

        public bool isFromSamplePage = false;

        public string _ButtonText;
        public string ButtonText { get { return _ButtonText; } set { SetProperty(ref _ButtonText, value); } }

        public string _GroupCreationSuccess;
        public string GroupCreationSuccess { get { return _GroupCreationSuccess; } set { SetProperty(ref _GroupCreationSuccess, value); } }
        #region Constructor
        public InviteScreenPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion
        public async void Invite()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = "I want to invite you to join the group! "+GroupName +" Please install app from "+ "Android:https://play.google.com/store/apps/details?id=com.esorr.esorrApp " + "Install the app and signup and the join the group with code: "+GroupCode,
                Title = "Share Text",
                Subject="Group Invitation"
               
            });;
        }

        public async void Next()
        {
            await NavigationService.NavigateAsync("HomePage");
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
           // throw new System.NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if(parameters.ContainsKey("GroupCode"))
                {
                    GroupCode = parameters["GroupCode"].ToString();
                    ButtonText = "Next";
                }
                if(parameters.ContainsKey("GroupName"))
                {
                    GroupName = parameters["GroupName"].ToString();
                    GroupCreationSuccess = "You have successfully created the Group "+GroupName;
                }
                if(parameters.ContainsKey("IsFromSamplePage"))
                {
                    isFromSamplePage = bool.Parse(parameters["IsFromSamplePage"].ToString());
                    ButtonText = "Back";
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
