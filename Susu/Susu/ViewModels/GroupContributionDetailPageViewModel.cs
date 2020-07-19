using ESORR.Models;
using Prism.Navigation;
using Susu;
using Susu.Models;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using static Susu.Models.Enums;

namespace ESORR.ViewModels
{
    public class GroupContributionDetailPageViewModel : ViewModelBase,INavigationAware
    {
        #region Properties
        GroupContributionDetails groupContributionDetails { get; set; }
        public int groupNumber = 0;
        public List<UserPayInDetails> _usersPayInDetails;
        public List<UserPayInDetails> UserPayInDetails { get { return _usersPayInDetails; } set { SetProperty(ref _usersPayInDetails, value); } }

        public string _ContributionDate;
        public string ContributionDate { get { return _ContributionDate; } set { SetProperty(ref _ContributionDate, value); } }

        public string _ContributionDay;
        public string ContributionDay { get { return _ContributionDay; } set { SetProperty(ref _ContributionDay, value); } }

        public ICommand BackClicked { get { return new Command(Back); } }

        public ICommand ToggleClicked { get; set; }

        public bool isHittedManullay { get; set; } = false;

        public ImageSource _AllnumberCheckbox = "check_box_empty.png";
        public ImageSource AllnumberCheckbox { get { return _AllnumberCheckbox; } set { SetProperty(ref _AllnumberCheckbox, value); } }

        public bool _IsAdmin;
        public bool IsAdmin { get { return _IsAdmin; } set { SetProperty(ref _IsAdmin, value); } }

        public string _NextScheduleDate;
        public string NextScheduleDate { get { return _NextScheduleDate; } set { SetProperty(ref _NextScheduleDate, value); } }

        string amount { get; set; }

        #endregion

        #region Constructor
        public GroupContributionDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
            ToggleClicked = new Command<UserPayInDetails>(Toggle);
           
        }
        #endregion

        #region Functions
        private async void BindData()
        {
            if (App.IsGroupAdmin)
                IsAdmin = true;
            else
                IsAdmin = false;
            groupContributionDetails = new GroupContributionDetails();
            groupContributionDetails = await ServiceBase.GetContributionDetailsByGroupNO(groupNumber);
            if(groupContributionDetails!=null)
            {
                ContributionDate = groupContributionDetails.ContributionDate.ToString("MM/dd/yyyy");
                ContributionDay = groupContributionDetails.ContributionDay;
                if(groupContributionDetails.NextContributionDate.HasValue)
                {
                    NextScheduleDate = groupContributionDetails.NextContributionDate?.ToString("MM/dd/yyyy");
                }
                UserPayInDetails = await ServiceBase.GetPayInDetailByGroupNO(groupNumber, groupContributionDetails.ContributionId);
                if(UserPayInDetails.Count == UserPayInDetails.Where(x=>x.isPaymentCompleted).Count())
                {
                    AllnumberCheckbox = "check_box.png";
                }
                else
                {
                    AllnumberCheckbox = "check_box_empty.png";
                }
                
            }
        }

        public async void UpdatePayment(UserPayInDetails userPayInDetails)
        {
            List<UserPayInDetails> lstuserPayInDetails = new List<UserPayInDetails>();
            if(userPayInDetails != null && userPayInDetails.ContributionId>0)
            {
                lstuserPayInDetails.Add(userPayInDetails);
                IsLoading = true;
                bool isSuccess = await ServiceBase.SaveUserPayInDetails(lstuserPayInDetails);
                IsLoading = false;
                if(isSuccess)
                {
                    SendNotification(lstuserPayInDetails);
                }
                else
                {
                   // await App.Current.MainPage.DisplayAlert("", "Something went wrong", "OK");
                }
            }
        }
        public void Toggle(UserPayInDetails userPayOutDetails)
        {

        }
        private async void SendNotification(List<UserPayInDetails> lstuserPayInDetails)
        {
            IsLoading = true;
            List<EmailNotificatinDetailsDto> lstemailNotificatinDetailsDto = new List<EmailNotificatinDetailsDto>();
            foreach (var items in lstuserPayInDetails)
            {
                EmailNotificatinDetailsDto emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
                UserDto userDto = await ServiceBase.GetUserById(items.UserId);
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
                            PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<Name>", items.UserName);
                            PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<paymentdate>", NextScheduleDate);
                            PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<contributionamount>", amount);
                        }
                        emailNotificatinDetailsDto.NotificationMessage = PaymentnotificationDto.Message;
                        emailNotificatinDetailsDto.isReadbyUser = false;
                        emailNotificatinDetailsDto.FromUserId = App.UserId;
                        lstemailNotificatinDetailsDto.Add(emailNotificatinDetailsDto);
                    }
                }
                IsLoading = false;
            }
            if (lstemailNotificatinDetailsDto != null && lstemailNotificatinDetailsDto.Count > 0)
            {
                IsLoading = true;
                bool isSuccess = await ServiceBase.SaveAndSendNotificationMail(lstemailNotificatinDetailsDto);
                IsLoading = false;
                if (isSuccess)
                {
                    BindData();
                    await App.Current.MainPage.DisplayAlert("", "Notification send successfully", "OK");
                }
                else
                {

                }
            }
            else
            {

            }

        }

        public async void UpdateAllpayments()
        {
            List<UserPayInDetails> lstPayedUsers = new List<UserPayInDetails>();
            if (UserPayInDetails != null && UserPayInDetails.Count > 0)
            {
                lstPayedUsers = UserPayInDetails.Where(x => x.isPaymentCompleted == false).ToList();
                if (lstPayedUsers != null && lstPayedUsers.Count > 0)
                {
                    lstPayedUsers.Select(c => { c.isPaymentCompleted = true; return c; }).ToList();
                    IsLoading = true;
                    bool isSuccess = await ServiceBase.SaveUserPayInDetails(lstPayedUsers);
                    IsLoading = false;
                    if (isSuccess)
                    {
                        SendNotification(lstPayedUsers);
                    }
                    else
                    {
                        // await App.Current.MainPage.DisplayAlert("", "Something went wrong", "OK");
                    }
                }
            }
            else
            {
                 await App.Current.MainPage.DisplayAlert("", "Something went wrong", "OK");
            }
        }
        private void Back()
        {
            NavigationService.GoBackAsync();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new System.NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey("GroupContributionDetailPage"))
                {
                    IsLoading = true;
                    groupNumber = int.Parse(parameters["GroupContributionDetailPage"].ToString());
                    if (parameters.ContainsKey("ContributionAmount"))
                    {
                        amount = parameters["ContributionAmount"].ToString();
                    }
                    BindData();
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
