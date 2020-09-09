using Prism.Navigation;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Susu.ViewModels
{
    public class LandingPageViewModel : ViewModelBase
    {
        #region Properties
        public int _GroupId;
        public int GroupId { get { return _GroupId; } set { SetProperty(ref _GroupId, value); } }

        public string _GroupNumber = "";
        public string GroupNumber { get { return _GroupNumber; } set { SetProperty(ref _GroupNumber, value); } }

        public Color _GroupIdPlaceholderColor = Color.FromHex("#083b66");
        public Color GroupIdPlaceholderColor { get { return _GroupIdPlaceholderColor; } set { SetProperty(ref _GroupIdPlaceholderColor, value); } }
        public ICommand JoinGroupClicked
        {
            get { return new Command(JoinGroup); }
        }
        public ICommand CreateNewGroupClicked
        {
            get { return new Command(CreateGroup); }
        }

        public bool _IsJoinGroupViisble = false;
        public bool IsJoinGroupViisble { get { return _IsJoinGroupViisble; } set { SetProperty(ref _IsJoinGroupViisble, value); } }

        public ICommand CloseClicked
        {
            get { return new Command(Close); }
        }

        public ICommand JoinButtonClicked
        {
            get { return new Command(Join); }
        }

        public bool _GroupCustomMeassageVisible = false;
        public bool GroupCustomMeassageVisible { get { return _GroupCustomMeassageVisible; } set { SetProperty(ref _GroupCustomMeassageVisible, value); } }

        public ICommand GroupCustomCloseClicked
        {
            get { return new Command(CloseCustomGroup); }
        }

        public ICommand AcceptCustomGroupMessage
        {
            get { return new Command(AcceptCustomRules); }
        }

        public string _CustomRulesText;
        public string CustomRulesText { get { return _CustomRulesText; } set { SetProperty(ref _CustomRulesText, value); } }

        public bool IsAcceptCustomRule = false;

        string userId = "";
        GroupDto groupDto = null;

        public ICommand IsMoreClicked { get { return new Command(MoreClicked); } }

       

        #endregion


        #region Constructor
        public LandingPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
        #endregion

        #region Functions
        /// <summary>
        /// This method hits when user click on Join Group Image
        /// </summary>
        public void JoinGroup()
        {
            IsJoinGroupViisble = true;
        }
        /// <summary>
        /// This method hits when user click on Create Group Image
        /// </summary>
        /// <param name="obj"></param>
        private async void CreateGroup(object obj)
        {
            await NavigationService.NavigateAsync("CreateGroupPage");
        }
        /// <summary>
        /// This method hits when user close button in  Join Group 
        /// </summary>
        public void Close()
        {
            IsJoinGroupViisble = false;
        }

        /// <summary>
        /// This method hits when user click on join button
        /// </summary>
        public async void Join()
        {
            groupDto = new GroupDto();
            if (!string.IsNullOrWhiteSpace(GroupNumber))
            {
                GroupId = Convert.ToInt32(GroupNumber);
            }
            if (GroupId == 0)
            {
                GroupIdPlaceholderColor = Color.Red;
                return;
            }
            userId = App.UserId + ",";
            IsLoading = true;
            groupDto = await ServiceBase.JoinUser(userId, GroupId, IsAcceptCustomRule);
            IsLoading = false;
            if (groupDto != null && groupDto.Id > 0)
            {
                if(groupDto.ErrorId == -1)
                {
                   await App.Current.MainPage.DisplayAlert("", "Sorry the group you are trying to join is already in session. Please join another group or create a new group.", "OK");
                    return;
                }
                App.GroupId = groupDto.Id;
                App.Current.Properties["GroupId"] = groupDto.Id;
                await App.Current.SavePropertiesAsync();
                App.IsGroupAdmin = false;
                App.Current.Properties["GroupAdmin"] = App.IsGroupAdmin;
                IsJoinGroupViisble = false;
                if (groupDto.CustomRules != null)
                {
                    GroupCustomMeassageVisible = true;
                    CustomRulesText = groupDto.CustomRules;
                }
                else
                {
                     AcceptCustomRules();
                }

                //await NavigationService.NavigateAsync("SamplePage");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("InValid", "Please enter a valid Group number", "OK");
            }

        }

        /// <summary>
        /// This method hits when user click on Close custom Rules
        /// </summary>
        private void CloseCustomGroup()
        {
            GroupCustomMeassageVisible = false;
            IsJoinGroupViisble = false;
        }

        /// <summary>
        /// This method hits when user click on accept custom Rules
        /// </summary>
        private async void AcceptCustomRules()
        {
            IsAcceptCustomRule = true;
            groupDto = await ServiceBase.JoinUser(userId, GroupId, IsAcceptCustomRule);
            if (groupDto.Id > 0)
                await NavigationService.NavigateAsync("HomePage");
        }

        /// <summary>
        /// This method hits when user click on Three dots icon
        /// </summary>
        private void MoreClicked()
        {
            NavigationService.NavigateAsync("MorePage");
        }
        #endregion
    }
}
