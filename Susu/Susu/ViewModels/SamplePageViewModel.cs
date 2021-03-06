﻿using ESORR.Models;
using Prism;
using Prism.Navigation;
using Susu.Models;
using Susu.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Susu.Models.Enums;

namespace Susu.ViewModels
{
    public class SamplePageViewModel : ViewModelBase, INavigationAware
    {
        #region Properties
        public ImageSource _GroupInfoIcon = "info_blue.png";
        public ImageSource GroupInfoIcon { get { return _GroupInfoIcon; } set { SetProperty(ref _GroupInfoIcon, value); } }

        public ImageSource _GroupUserIcon = "Group_black.png";
        public ImageSource GroupUserIcon { get { return _GroupUserIcon; } set { SetProperty(ref _GroupUserIcon, value); } }

        public ImageSource _GroupPaymentIcon = "payment_black.png";
        public ImageSource GroupPaymentIcon { get { return _GroupPaymentIcon; } set { SetProperty(ref _GroupPaymentIcon, value); } }

        public ImageSource _UserIcon = "user_profile.png";
        public ImageSource UserIcon { get { return _UserIcon; } set { SetProperty(ref _UserIcon, value); } }

        public ImageSource _HelpIcon = "more_black.png";
        public ImageSource HelpIcon { get { return _HelpIcon; } set { SetProperty(ref _HelpIcon, value); } }

        public bool _IsGroupInfoVisible = true;
        public bool IsGroupInfoVisible { get { return _IsGroupInfoVisible; } set { SetProperty(ref _IsGroupInfoVisible, value); } }

        public bool _IsGroupMembersVisible = false;
        public bool IsGroupMembersVisible { get { return _IsGroupMembersVisible; } set { SetProperty(ref _IsGroupMembersVisible, value); } }

        public ObservableCollection<UserDto> lstGroupMembers;
        public ObservableCollection<UserDto> LstGroupMembers { get { return lstGroupMembers; } set { SetProperty(ref lstGroupMembers, value); } }

        public bool _IsUserInfoVisible = false;
        public bool IsUserInfoVisible { get { return _IsUserInfoVisible; } set { SetProperty(ref _IsUserInfoVisible, value); } }

        public ICommand SaveUserProfile { get { return new Command(update); } }

        public bool _IsPaymentInfoVisibe = false;
        public bool IsPaymentInfoVisibe { get { return _IsPaymentInfoVisibe; } set { SetProperty(ref _IsPaymentInfoVisibe, value); } }

        public bool _IsHelpVisible = false;
        public bool IsHelpVisible { get { return _IsHelpVisible; } set { SetProperty(ref _IsHelpVisible, value); } }

        public ICommand GroupInfoTapped
        {
            get { return new Command(GroupInfo); }
        }
        public ICommand GroupUsersTapped
        {
            get { return new Command(GroupUsers); }
        }

        public ICommand GroupPaymentsapped
        {
            get { return new Command(Payments); }
        }

        public ICommand UserInfoTapped
        {
            get { return new Command(UserInfo); }
        }

        public ICommand HelpIconTapped
        {
            get { return new Command(Help); }
        }

        public UserDto _userDto = new UserDto();
        public UserDto userDto { get { return _userDto; } set { SetProperty(ref _userDto, value); } }

        public GroupDto _groupDto = new GroupDto();
        public GroupDto groupDto { get { return _groupDto; } set { SetProperty(ref _groupDto, value); } }

        public ICommand LogoutClicked { get { return new Command(Logout); } }

        public ICommand ResetPasswordCliked { get { return new Command(RestPassword); } }

        public ICommand NotificationClicked { get { return new Command(Notification); } }

        public int _NotificationCount = 0;
        public int NotificationCount { get { return _NotificationCount; } set { SetProperty(ref _NotificationCount, value); } }

        public ICommand ViewNotificationClicked
        {
            get { return new Command(ViewNotifications); }
        }
        List<EmailNotificatinDetailsDto> lstemailNotificatinDetailsDtos = null;

        public string _ContributionAmount;
        public string ContributionAmount { get { return _ContributionAmount; } set { SetProperty(ref _ContributionAmount, value); } }

        public string _ContributionDate;
        public string ContributionDate { get { return _ContributionDate; } set { SetProperty(ref _ContributionDate, value); } }

        public ICommand paymentContributionDetailsClicked { get { return new Command(paymentContribution); } }

        public ICommand PayouDetailsClicked { get { return new Command(PayoutClicked); } }

        public ICommand PayNowClicked { get { return new Command(Paynow); } }

        public ICommand InvitelinkClicked { get { return new Command(Invite); } }

        public bool _IsGroupAdmin = false;
        public bool IsGroupAdmin { get { return _IsGroupAdmin; } set { SetProperty(ref _IsGroupAdmin, value); } }

        GroupContributionDetails groupContributionDetails = new GroupContributionDetails();

        public bool _IsContributionDateVisible = false;
        public bool IsContributionDateVisible { get { return _IsContributionDateVisible; } set { SetProperty(ref _IsContributionDateVisible, value); } }

        public bool _IsContributionDayVisible = false;
        public bool IsContributionDayVisible { get { return _IsContributionDayVisible; } set { SetProperty(ref _IsContributionDayVisible, value); } }

        public bool _IsGroupStartDateVisible = false;
        public bool IsGroupStartDateVisible { get { return _IsGroupStartDateVisible; } set { SetProperty(ref _IsGroupStartDateVisible, value); } }

        public bool _IsGroupPayOutDayVisible = false;
        public bool IsGroupPayOutDayVisible { get { return _IsGroupPayOutDayVisible; } set { SetProperty(ref _IsGroupPayOutDayVisible, value); } }

        public bool _IsGroupPayOutDateVisible = false;
        public bool IsGroupPayOutDateVisible { get { return _IsGroupPayOutDateVisible; } set { SetProperty(ref _IsGroupPayOutDateVisible, value); } }

        public ICommand CloseClicked { get { return new Command(Close); } }

        public ICommand OKButtonClicked { get { return new Command(OK); } }

        public bool _TaxMessageVisible = false;
        public bool TaxMessageVisible { get { return _TaxMessageVisible; } set { SetProperty(ref _TaxMessageVisible, value); } }

        public bool _IsHelppopupVisible = false;
        public bool IsHelppopupVisible { get { return _IsHelppopupVisible; } set { SetProperty(ref _IsHelppopupVisible, value); } }

        public ICommand HelpCloseClicked { get { return new Command(CloseHelp); } }

        public ICommand HelpClicked { get { return new Command(HelpPopup); } }

        public bool _IsGroupInfoEditable = false;
        public bool IsGroupInfoEditable { get { return _IsGroupInfoEditable; } set { SetProperty(ref _IsGroupInfoEditable, value); } }

        public List<string> _lstperiod;
        public List<string> ContributionPeriod { get { return _lstperiod; } set { SetProperty(ref _lstperiod, value); } }

        public List<string> _DaysList;
        public List<string> DaysList { get { return _DaysList; } set { SetProperty(ref _DaysList, value); } }

        public DateTime _selectedDate;
        public DateTime selectedDate { get { return _selectedDate; } set { SetProperty(ref _selectedDate, value); } }

        public ICommand updateGroupDetails { get { return new Command(updateGroup); } }

        public bool _IsGroupInfoupdateVisible = false;
        public bool IsGroupInfoupdateVisible { get { return _IsGroupInfoupdateVisible; } set { SetProperty(ref _IsGroupInfoupdateVisible, value); } }

        public ICommand GroupudateClosed { get { return new Command(CloseGroup); } }

        public Color _notificationBGColor = Color.White;
        public Color notificationBGColor { get { return _notificationBGColor; } set { SetProperty(ref _notificationBGColor, value); } }

        public Color _noficationLabelColor = Color.White;
        public Color noficationLabelColor { get { return _noficationLabelColor; } set { SetProperty(ref _noficationLabelColor, value); } }

        public long CreatorId { get; set; }

        public string _AdminPaypalId = "";
        public string AdminPaypalId { get { return _AdminPaypalId; } set { SetProperty(ref _AdminPaypalId, value); } }
        public bool _IsAdminhaspaypalAccount = false;
        public bool IsAdminhaspaypalAccount { get { return _IsAdminhaspaypalAccount; } set { SetProperty(ref _IsAdminhaspaypalAccount, value); } }
        public bool _IsAdminhasnopaypalAccount = false;
        public bool IsAdminhasnopaypalAccount { get { return _IsAdminhasnopaypalAccount; } set { SetProperty(ref _IsAdminhasnopaypalAccount, value); } }

        public bool _IsGroupInviteVisible = false;
        public bool IsGroupInviteVisible { get { return _IsGroupInviteVisible; } set { SetProperty(ref _IsGroupInviteVisible, value); } }

        public ICommand HomeClicked { get { return new Command(Home); } }

        public ICommand PayMemberClicked { get { return new Command(PayMember); } }

        public bool _IsPayMemberpopupVisible = false;
        public bool IsPayMemberpopupVisible { get { return _IsPayMemberpopupVisible; } set { SetProperty(ref _IsPayMemberpopupVisible, value); } }
        public string _ZeeleText = "";
        public string ZeeleText { get { return _ZeeleText; } set { SetProperty(ref _ZeeleText, value); } }

        public ICommand OpenZelleClicked { get { return new Command(OpenZelle); } }



        #endregion

        #region Constructor
        public SamplePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
            if (App.IsGroupAdmin)
                IsGroupAdmin = true;
            else
                IsGroupAdmin = false;
            GetGroupDetails();
            GetNotification();
            _lstperiod = new List<string>() { "Weekly", "Bi-Weekly", "Semi-Monthly", "Monthly", "Semi-Yearly", "Yearly" };
            DaysList = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        }
        #endregion

        #region Functions
        /// <summary>
        /// This method hit when user click on group info
        /// </summary>
        private void GroupInfo()
        {
            GroupInfoIcon = "info_blue.png";
            GroupUserIcon = "Group_black.png";
            GroupPaymentIcon = "payment_black.png";
            UserIcon = "user_profile.png";
            HelpIcon = "more_black.png";
            IsGroupInfoVisible = true;
            IsGroupMembersVisible = false;
            IsUserInfoVisible = false;
            IsPaymentInfoVisibe = false;
            IsHelpVisible = false;
            GetGroupDetails();
            GetNotification();
        }
        /// <summary>
        ///  This method hit when user click on group users
        /// </summary>
        private void GroupUsers()
        {
            BindGroupUsers();
            GetNotification();
            GroupInfoIcon = "info_black.png";
            GroupUserIcon = "Group_blue.png";
            GroupPaymentIcon = "payment_black.png";
            UserIcon = "user_profile.png";
            HelpIcon = "more_black.png";
            IsGroupMembersVisible = true;
            IsGroupInfoVisible = false;
            IsUserInfoVisible = false;
            IsPaymentInfoVisibe = false;
            IsHelpVisible = false;
        }
        /// <summary>
        /// This method hit when user click on help
        /// </summary>
        private void Help()
        {
            GroupInfoIcon = "info_black.png";
            GroupUserIcon = "Group_black.png";
            GroupPaymentIcon = "payment_black.png";
            UserIcon = "user_profile.png";
            HelpIcon = "more_blue.png";
            IsGroupMembersVisible = false;
            IsGroupInfoVisible = false;
            IsUserInfoVisible = false;
            IsPaymentInfoVisibe = false;
            IsHelpVisible = true;
            GetNotification();
        }

        /// <summary>
        /// /// This method hit when user click on payments
        /// </summary>
        private void Payments()
        {
            GroupInfoIcon = "info_black.png";
            GroupUserIcon = "Group_black.png";
            GroupPaymentIcon = "Payment_blue.png";
            UserIcon = "user_profile.png";
            HelpIcon = "more_black.png";
            IsGroupMembersVisible = false;
            IsGroupInfoVisible = false;
            IsUserInfoVisible = false;
            IsPaymentInfoVisibe = true;
            IsHelpVisible = false;
            GetNotification();

        }

        /// <summary>
        /// /// This method hit when user click on user Info
        /// </summary>
        private void UserInfo()
        {
            GroupInfoIcon = "info_black.png";
            GroupUserIcon = "Group_black.png";
            GroupPaymentIcon = "payment_black.png";
            UserIcon = "user_blue.png";
            HelpIcon = "more_black.png";
            IsUserInfoVisible = true;
            IsGroupInfoVisible = false;
            IsGroupMembersVisible = false;
            IsPaymentInfoVisibe = false;
            IsHelpVisible = false;
            GetUserDetails();
            GetNotification();
        }

        /// <summary>
        /// Call to bind Groupusers
        /// </summary>
        public async void BindGroupUsers()
        {

            List<UserDto> lstUsersDto = new List<UserDto>();
            IsLoading = true;
            lstUsersDto = await ServiceBase.GetUsersByGroupId(App.GroupId);
            IsLoading = false;
            if (lstUsersDto != null && lstUsersDto.Count > 0)
            {
                LstGroupMembers = new ObservableCollection<UserDto>(lstUsersDto);
            }
        }

        /// <summary>
        /// Method to update user details
        /// </summary>
        public async void update()
        {
            UserDto UpdateduserDto = new UserDto();
            UpdateduserDto = userDto;
            IsLoading = true;
            UserDto userDetails = await ServiceBase.SaveUser(userDto);
            IsLoading = false;
            if (userDetails != null && userDetails.Id > 0)
                await App.Current.MainPage.DisplayAlert("Alert", "User data updated successfully", "OK");
            else
                await App.Current.MainPage.DisplayAlert("Alert", "Something went wrong please try again", "OK");

        }

        /// <summary>
        /// Method to get User details
        /// </summary>
        public async void GetUserDetails()
        {
            try
            {
                IsLoading = true;
                if (App.Current.Properties.ContainsKey("UserId"))
                {
                    long userId = int.Parse(App.Current.Properties["UserId"].ToString());
                    if (userId > 0)
                    {
                        IsLoading = false;
                        userDto = await ServiceBase.GetUserById(userId);
                    }
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// /// This method hit when user click Logout
        /// </summary>
        public void Logout()
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
        /// Method to get groupdetails
        /// </summary>
        public async void GetGroupDetails()
        {
            try
            {
                if (App.Current.Properties.ContainsKey("GroupId"))
                {
                    IsLoading = true;
                    long groupId = long.Parse(App.Current.Properties["GroupId"].ToString());
                    groupDto = await ServiceBase.GetGroupDetialsByGroupId(App.GroupId != 0 ? App.GroupId : 0);
                    if (groupDto != null && groupDto.Id > 0)
                    {
                        App.GroupId = groupDto.Id;
                        App.Current.Properties["GroupId"] = App.GroupId;
                        CreatorId = groupDto.CreatorId;
                        App.Amount = groupDto.ContributionAmount.ToString();
                        await App.Current.SavePropertiesAsync();
                        if (groupDto.ContributionAmount > 0)
                            ContributionAmount = "$ " + string.Format("{0:00.00}", groupDto.ContributionAmount);
                        if (groupDto.ContributionDate != null)
                            ContributionDate = string.Format("{0:d/M/yyyy}", groupDto.ContributionDate);
                        if (App.UserId != 0 && groupDto.CreatorId != 0 && App.UserId == groupDto.CreatorId)
                        {
                            App.IsGroupAdmin = true;
                            if (groupDto.ErrorId == 0)
                            {
                                IsGroupInviteVisible = true;
                            }
                        }

                        if (groupDto != null && App.IsGroupAdmin && groupDto.ErrorId != -1)
                        {
                            IsGroupInfoEditable = true;
                        }
                        else
                        {
                            IsGroupInfoEditable = false;
                        }
                        if (groupDto != null && groupDto.Contrbution_peroid == "Monthly" || groupDto.Contrbution_peroid == "Semi-Monthly")
                        {
                            IsContributionDateVisible = true;
                            IsContributionDayVisible = false;
                            IsGroupStartDateVisible = false;
                            IsGroupPayOutDateVisible = true;
                            IsGroupPayOutDayVisible = false;
                        }
                        else if (groupDto != null && groupDto.Contrbution_peroid == "Weekly")
                        {
                            IsContributionDateVisible = false;
                            IsContributionDayVisible = true;
                            IsGroupStartDateVisible = true;
                            IsGroupPayOutDateVisible = false;
                            IsGroupPayOutDayVisible = true;
                        }
                        else if (groupDto != null && groupDto.Contrbution_peroid == "Bi-Weekly")
                        {
                            IsContributionDateVisible = false;
                            IsContributionDayVisible = true;
                            IsGroupStartDateVisible = true;
                            IsGroupPayOutDateVisible = false;
                            IsGroupPayOutDayVisible = true;
                        }
                        else
                        {
                            IsContributionDateVisible = true;
                            IsContributionDayVisible = false;
                            IsGroupStartDateVisible = false;
                            IsGroupPayOutDateVisible = true;
                            IsGroupPayOutDayVisible = false;
                        }
                        IsLoading = true;
                        groupContributionDetails = await ServiceBase.GetContributionDetailsByGroupNO(groupDto.GroupNumber);
                        IsLoading = false;
                        if (groupContributionDetails != null && groupContributionDetails.Id > 0)
                        {
                            App.contributionId = groupContributionDetails.ContributionId;
                            App.GroupNumber = groupContributionDetails.GroupNumber;
                        }
                        IsLoading = false;
                    }
                    else
                    {
                        if (await App.Current.MainPage.DisplayAlert("", "Your are  no longer in the system", "Logout", " "))
                        {
                            Logout();
                        }
                    }

                }
                else
                {
                    await NavigationService.NavigateAsync("LoginPage");
                }

            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// Method to update groupdetails by admin 
        /// </summary>
        public async void updateGroup()
        {
            GroupDto updatedgroupDto = groupDto;
            if (updatedgroupDto != null)
            {
                if (updatedgroupDto.ContributionDate != null && updatedgroupDto.PayOutDate != null && updatedgroupDto.PayOutDate < updatedgroupDto.ContributionDate)
                {
                    await App.Current.MainPage.DisplayAlert("", "Group payout date should be greater than group contribution date", "OK");
                    return;
                }
                else if (!string.IsNullOrEmpty(updatedgroupDto.Contrbution_peroid) && updatedgroupDto.Contrbution_peroid.ToLower() == "weekly")
                {

                    if (string.IsNullOrEmpty(updatedgroupDto.ContributionDay))
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group contribution day", "OK");
                        return;
                    }
                    else if (updatedgroupDto.ContributionDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group start date", "OK");
                        return;
                    }
                    else if (string.IsNullOrEmpty(updatedgroupDto.PayOutDay))
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group payout day", "OK");
                        return;
                    }
                }
                else if (!string.IsNullOrEmpty(updatedgroupDto.Contrbution_peroid) && updatedgroupDto.Contrbution_peroid.ToLower() == "bi-weekly")
                {
                    if (string.IsNullOrEmpty(updatedgroupDto.ContributionDay))
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group contribution day", "OK");
                        return;
                    }
                    else if (updatedgroupDto.ContributionDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group start date", "OK");
                        return;
                    }
                    else if (string.IsNullOrEmpty(updatedgroupDto.PayOutDay))
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group payout day", "OK");
                        return;
                    }
                }
                else if (!string.IsNullOrEmpty(updatedgroupDto.Contrbution_peroid) && updatedgroupDto.Contrbution_peroid.ToLower() == "monthly")
                {
                    if (updatedgroupDto.ContributionDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group contribution date", "OK");
                        return;
                    }
                    else if (updatedgroupDto.PayOutDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group payout date", "OK");
                        return;
                    }

                }
                else if (!string.IsNullOrEmpty(updatedgroupDto.Contrbution_peroid) && updatedgroupDto.Contrbution_peroid.ToLower() == "semi-monthly")
                {
                    if (updatedgroupDto.ContributionDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group contribution date", "OK");
                        return;
                    }
                    else if (updatedgroupDto.PayOutDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group payout date", "OK");
                        return;
                    }

                }
                else if (!string.IsNullOrEmpty(updatedgroupDto.Contrbution_peroid) && updatedgroupDto.Contrbution_peroid.ToLower() == "yearly")
                {
                    if (updatedgroupDto.ContributionDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group contribution date", "OK");
                        return;
                    }
                    else if (updatedgroupDto.PayOutDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group payout date", "OK");
                        return;
                    }

                }
                else if (!string.IsNullOrEmpty(updatedgroupDto.Contrbution_peroid) && updatedgroupDto.Contrbution_peroid.ToLower() == "semi-yearly")
                {
                    if (updatedgroupDto.ContributionDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group contribution date", "OK");
                        return;
                    }
                    else if (updatedgroupDto.PayOutDate == null)
                    {
                        await App.Current.MainPage.DisplayAlert("", "Please select group payout date", "OK");
                        return;
                    }

                }
                else if (updatedgroupDto.ContributionDate != null && updatedgroupDto.ContributionDate > updatedgroupDto.PayOutDate)
                {
                    await App.Current.MainPage.DisplayAlert("", "Contribution date should be less than payout date", "OK");
                    return;
                }
                else if (updatedgroupDto.ContributionDate != null && updatedgroupDto.PayOutDate == null)
                {
                    await App.Current.MainPage.DisplayAlert("", "Payout date should be greater than contribution date", "OK");
                    return;
                }
                else if (updatedgroupDto.ContributionDate != null && updatedgroupDto.PayOutDate != null && updatedgroupDto.PayOutDate < updatedgroupDto.ContributionDate)
                {
                    await App.Current.MainPage.DisplayAlert("", "Payout date should be greater than contribution date", "OK");
                    return;
                }
                if (!string.IsNullOrEmpty(updatedgroupDto.Contrbution_peroid) && updatedgroupDto.Contrbution_peroid == "Bi-Weekly")
                {
                    updatedgroupDto.ContributionPeriod = "biweekly";
                }
                else if (!string.IsNullOrEmpty(updatedgroupDto.Contrbution_peroid) && updatedgroupDto.Contrbution_peroid == "Semi-Monthly")
                {
                    updatedgroupDto.ContributionPeriod = "semimonthly";
                }
                else if (!string.IsNullOrEmpty(updatedgroupDto.Contrbution_peroid) && updatedgroupDto.Contrbution_peroid == "Semi-Yearly")
                {
                    updatedgroupDto.ContributionPeriod = "semiyearly";
                }
                else
                {
                    updatedgroupDto.ContributionPeriod = updatedgroupDto.ContributionPeriod;
                }
                IsLoading = true;
                GroupDto group = await ServiceBase.SaveGroupInfo(updatedgroupDto);

                IsLoading = false;
                if (group != null && group.Id > 0)
                {
                    // await App.Current.MainPage.DisplayAlert("", "Group details updated successfully", "OK");
                    IsGroupInfoupdateVisible = true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("", "Something went wrong", "OK");
                }
            }

        }

        /// <summary>
        /// Method to Bind notification count
        /// </summary>
        public async void GetNotification()
        {
            IsLoading = true;
            lstemailNotificatinDetailsDtos = new List<EmailNotificatinDetailsDto>();
            lstemailNotificatinDetailsDtos = await ServiceBase.GetUserNotificationsById(App.UserId);
            IsLoading = false;
            if (lstemailNotificatinDetailsDtos != null && lstemailNotificatinDetailsDtos.Count > 0)
            {
                NotificationCount = lstemailNotificatinDetailsDtos.Where(x => x.isReadbyUser == false).Count();
                if (NotificationCount > 0)
                {
                    notificationBGColor = Color.Red;
                    noficationLabelColor = Color.White;
                }
                else
                {
                    notificationBGColor = Color.White;
                    noficationLabelColor = Color.Black;
                }
            }
            else
            {
                NotificationCount = 0;
                notificationBGColor = Color.White;
                noficationLabelColor = Color.Black;
            }

        }

        /// <summary>
        /// /// This method hit when user click on Resset Password
        /// </summary>
        public async void RestPassword()
        {
            await NavigationService.NavigateAsync("ResetPasswordPage");
        }

        /// <summary>
        /// /// This method hit when user click on notification
        /// </summary>
        private async void Notification()
        {
            NavigationParameters np = new NavigationParameters();
            np.Add("ContributionDate", groupDto.ContributionDate);
            await NavigationService.NavigateAsync("AdminNotificationPage", np);
        }

        /// <summary>
        /// Method to view notifications
        /// </summary>
        public async void ViewNotifications()
        {
            //NavigationParameters np = new NavigationParameters();
            //np.Add("NotificationsList", lstemailNotificatinDetailsDtos);
            await NavigationService.NavigateAsync("ViewNotifications");
        }

        /// <summary>
        /// /// This method hit when user click on group Contribution
        /// </summary>
        private async void paymentContribution()
        {
            NavigationParameters np = new NavigationParameters();
            if (groupDto != null && groupDto.GroupNumber > 0)
            {
                np.Add("GroupContributionDetailPage", groupDto.GroupNumber);
                np.Add("ContributionAmount", groupDto.ContributionAmount);
            }

            await NavigationService.NavigateAsync("GroupContributionDetailPage", np);
        }

        /// <summary>
        /// /// This method hit when user click on group Payout
        /// </summary>
        private async void PayoutClicked()
        {
            NavigationParameters np = new NavigationParameters();
            if (groupDto != null && groupDto.GroupNumber > 0)
                np.Add("GroupContributionDetailPage", groupDto.GroupNumber);

            await NavigationService.NavigateAsync("GroupPayoutDetails", np);
        }

        /// <summary>
        /// Pay now method
        /// </summary>
        private async void Paynow()
        {
            IsLoading = true;
            TaxMessageVisible = true;
            UserDto userDto = await ServiceBase.GetUserById(CreatorId);
            if (userDto != null)
            {
                if (string.IsNullOrEmpty(userDto.ZelleId))
                {
                    IsAdminhasnopaypalAccount = true;
                    IsAdminhaspaypalAccount = false;
                }
                else
                {
                    IsAdminhaspaypalAccount = true;
                    IsAdminhasnopaypalAccount = false;
                    AdminPaypalId = userDto.ZelleId;
                }
            }
            IsLoading = false;

        }

        /// <summary>
        /// /// This method hit when admin click on group invite
        /// </summary>
        private async void Invite()
        {
            NavigationParameters np = new NavigationParameters();
            np.Add("GroupCode", groupDto.GroupNumber);
            np.Add("GroupName", groupDto.GroupName);
            np.Add("IsFromSamplePage", true);
            await NavigationService.NavigateAsync("InviteScreenPage", np);
        }

        private async void SendNotification()
        {
            IsLoading = true;
            List<EmailNotificatinDetailsDto> lstemailNotificatinDetailsDto = new List<EmailNotificatinDetailsDto>();
            EmailNotificatinDetailsDto emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
            UserDto userDto = await ServiceBase.GetUserById(App.UserId);
            List<NotificationDto> lstNotifications = await ServiceBase.GetNotifications(0);
            NotificationDto PaymentnotificationDto = new NotificationDto();
            if (lstNotifications != null && lstNotifications.Count > 0)
            {
                PaymentnotificationDto = lstNotifications.Where(x => x.NotificationType == (int)NotificationType.PayInPaymentSuccess).FirstOrDefault();
                if (PaymentnotificationDto != null)
                {
                    emailNotificatinDetailsDto.NotificationId = PaymentnotificationDto.Id;
                    emailNotificatinDetailsDto.UserId = userDto.Id;
                    emailNotificatinDetailsDto.UserMail = userDto != null ? userDto.Email : null;
                    emailNotificatinDetailsDto.mailSubject = PaymentnotificationDto.Tittle;
                    if (PaymentnotificationDto.Message != null)
                    {
                        PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<Name>", userDto.FirstName + userDto.LastName);
                        PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<payment date>", groupContributionDetails.NextContributionDate.ToString());
                    }
                    emailNotificatinDetailsDto.NotificationMessage = PaymentnotificationDto.Message;
                    emailNotificatinDetailsDto.isReadbyUser = false;
                    emailNotificatinDetailsDto.FromUserId = App.UserId;
                    lstemailNotificatinDetailsDto.Add(emailNotificatinDetailsDto);
                }
            }
            IsLoading = false;

            if (lstemailNotificatinDetailsDto != null && lstemailNotificatinDetailsDto.Count > 0)
            {
                IsLoading = true;
                bool isSuccess = await ServiceBase.SaveAndSendNotificationMail(lstemailNotificatinDetailsDto);
                IsLoading = false;
                if (isSuccess)
                {
                    // BindData();
                    await App.Current.MainPage.DisplayAlert("", "Payment done successfully", "OK");
                }
                else
                {

                }
            }
            else
            {

            }

        }

        /// <summary>
        /// close member popup
        /// </summary>
        private void Close()
        {
            TaxMessageVisible = false;
            if (IsPayMemberpopupVisible)
            {
                IsPayMemberpopupVisible = false;
            }
        }

        //private async void Pay()
        //{
        //    if (groupDto != null && groupDto.ContributionAmount > 0)
        //    {
        //        Double price = AddTaxes(groupDto.ContributionAmount);
        //        var result = await CrossPayPalManager.Current.Buy(new PayPalItem("ESORR", new Decimal(price), "USD"), new Decimal(0));
        //        if (result.Status == PayPalStatus.Cancelled)
        //        {
        //            //Debug.WriteLine("Cancelled");
        //        }
        //        else if (result.Status == PayPalStatus.Error)
        //        {
        //            await App.Current.MainPage.DisplayAlert("", "Something went wrong", "OK");
        //        }
        //        else if (result.Status == PayPalStatus.Successful)
        //        {
        //            //await  App.Current.MainPage.DisplayAlert("", "Payment completed successfully", "OK");
        //            UserPaymentDetails userPaymentDetails = new UserPaymentDetails();
        //            userPaymentDetails.GroupNumber = groupDto.GroupNumber;
        //            userPaymentDetails.PaidAmount = Convert.ToDouble(groupDto.ContributionAmount);
        //            userPaymentDetails.UserId = App.UserId;
        //            userPaymentDetails.ContributionId = groupContributionDetails.ContributionId;
        //            long paymentId = await ServiceBase.SavePayPalPaymentDetails(userPaymentDetails);
        //            if (paymentId > 0)
        //            {
        //                TaxMessageVisible = false;
        //                //await  App.Current.MainPage.DisplayAlert("", "Payment done successfully", "OK");
        //                SendNotification();
        //            }
        //            else
        //            {
        //                TaxMessageVisible = false;
        //                await App.Current.MainPage.DisplayAlert("", "Some thing went wrong", "OK");
        //            }

        //        }
        //    }
        //}

        //private double AddTaxes(decimal amount)
        //{
        //    double TotalAmount = 0;
        //    if (amount > 0)
        //    {
        //        // TotalAmount = Convert.ToDouble(amount) + (Convert.ToDouble(amount) * 0.30);
        //        TotalAmount = Convert.ToDouble(amount) + (Convert.ToDouble(amount) * 0.015);
        //    }
        //    return TotalAmount;
        //    //if (money < 10000)
        //    //{
        //    //    tax = .05 * money;
        //    //}
        //    //else if (money <= 100000)
        //    //{
        //    //    tax = .08 * money;
        //    //}
        //    //else
        //    //{
        //    //    tax = .085 * money;
        //    //}


        //}

        /// <summary>
        /// Method to close hlep popup
        /// </summary>
        private void CloseHelp()
        {
            IsHelppopupVisible = false;
        }
        /// <summary>
        /// Method to display help popup
        /// </summary>
        private void HelpPopup()
        {
            IsHelppopupVisible = true;
        }
        /// <summary>
        /// Method to close group popup
        /// </summary>
        public void CloseGroup()
        {
            IsGroupInfoupdateVisible = false;
        }

        public void OK()
        {
            TaxMessageVisible = false;
            if (IsPayMemberpopupVisible)
            {
                IsPayMemberpopupVisible = false;
            }
        }
        /// <summary>
        /// /// This method hit when user click on home button
        /// </summary>
        private void Home()
        {
            NavigationService.NavigateAsync("HomePage");
        }

        /// <summary>
        /// Pay member popup
        /// </summary>
        private async void PayMember()
        {

            List<UserPayOutDetails> UserPayOutDetails = await ServiceBase.GetPayOutDetailsByGroupNO(App.GroupNumber);
            if (UserPayOutDetails != null && UserPayOutDetails.Count > 0)
            {
                UserPayOutDetails userPayoutDetail = UserPayOutDetails.Where(x => x.ContributionId == App.contributionId).FirstOrDefault();
                if (userPayoutDetail != null)
                {
                    UserDto userDto = await ServiceBase.GetUserById(userPayoutDetail.UserId);
                    if (!string.IsNullOrEmpty(userDto.ZelleId))
                        ZeeleText = "Please pay the contributed amount for the month of " + userPayoutDetail.ContributionDateString.ToString() + " the user " + userPayoutDetail.UserName + " using Zelle ID is " + userDto.ZelleId;
                    else
                        ZeeleText = "Oops, the user " + userPayoutDetail.UserName + " is not provided their Zelle ID, Please ask them to upload Zell ID under their user information.";
                }
            }
            else
            {
                ZeeleText = "Something went wrong,please contact admin";
            }
            IsPayMemberpopupVisible = true;
        }

        /// <summary>
        /// Method to open zelle App
        /// </summary>
        private async void OpenZelle()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.zellepay.zelle&hl=en_IN");
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            else
            {
                Uri uri = new Uri("https://apps.apple.com/us/app/zelle/id1260755201");
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey("IsFromHomePage"))
                {
                    IsLoading = true;
                    if (parameters.ContainsKey("TappedIcon"))
                    {
                        if (parameters["TappedIcon"].ToString() == "GroupInfo")
                        {
                            GroupInfo();
                        }
                        else if (parameters["TappedIcon"].ToString() == "GroupUsers")
                        {
                            GroupUsers();
                        }
                        else if (parameters["TappedIcon"].ToString() == "More")
                        {
                            Help();
                        }
                        else if (parameters["TappedIcon"].ToString() == "Profile")
                        {
                            UserInfo();
                        }
                        else if (parameters["TappedIcon"].ToString() == "Payment")
                        {
                            Payments();
                        }
                    }


                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
            }
        }
    }
    #endregion
}
