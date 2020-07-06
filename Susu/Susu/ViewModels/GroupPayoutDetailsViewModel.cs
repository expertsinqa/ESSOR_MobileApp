using ESORR.Models;
using Prism.Navigation;
using Susu;
using Susu.Models;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static Susu.Models.Enums;

namespace ESORR.ViewModels
{
    public class GroupPayoutDetailsViewModel : ViewModelBase,INavigationAware
    {
        GroupContributionDetails groupContributionDetails { get; set; }
        public int groupNumber = 0;
        public List<UserPayOutDetails> _usersPayoutList;
        public List<UserPayOutDetails> UserPayOutDetails { get { return _usersPayoutList; } set { SetProperty(ref _usersPayoutList, value); } }

        public string _ContributionDate;
        public string ContributionDate { get { return _ContributionDate; } set { SetProperty(ref _ContributionDate, value); } }

        public string _ContributionDay;
        public string ContributionDay { get { return _ContributionDay; } set { SetProperty(ref _ContributionDay, value); } }

        public ICommand BackClicked { get { return new Command(Back); } }

        public bool _IsAdmin = false;
        public bool IsAdmin { get { return _IsAdmin; } set { SetProperty(ref _IsAdmin,value); } }

        private async void Back(object obj)
        {
            await NavigationService.GoBackAsync();
        }

        public GroupPayoutDetailsViewModel(INavigationService navigationService):base(navigationService)
        {
            NavigationService = navigationService;
        }

        private async void BindData()
        {
            if (App.IsGroupAdmin)
                IsAdmin = true;
            else
                IsAdmin = false;
            UserPayOutDetails = await ServiceBase.GetPayOutDetailsByGroupNO(groupNumber);
            if(UserPayOutDetails!=null && UserPayOutDetails.Count>0)
            {

            }
            //groupContributionDetails = new GroupContributionDetails();
            //groupContributionDetails = await ServiceBase.GetContributionDetailsByGroupNO(groupNumber);
            //if (groupContributionDetails != null)
            //{
            //    ContributionDate = groupContributionDetails.ContributionDate.ToString("dd/M/yyyy");
            //    ContributionDay = groupContributionDetails.ContributionDay;
            //    UserPayOutDetails = groupContributionDetails.LstPayOutUsers;
            //    //if (App.IsGroupAdmin)
            //    //{
            //    //    UserPayOutDetails.ToList().ForEach(u =>
            //    //    {
            //    //        if (u.isPaymentCompleted)
            //    //        {
            //    //            u.IsSwitchEnabled = false;
            //    //            u.isChecked = true;
            //    //        }
            //    //        else
            //    //        {
            //    //            u.IsSwitchEnabled = true;
            //    //            u.isChecked = false;
            //    //        }
            //    //    });

            //    //}
            //    //else
            //    //{
            //    //    UserPayOutDetails.ToList().ForEach(u =>
            //    //    {
            //    //        if (u.isPaymentCompleted)
            //    //        {
            //    //            u.IsSwitchEnabled = false;
            //    //            u.isChecked = true;
            //    //        }
            //    //        else
            //    //        {
            //    //            u.IsSwitchEnabled = false;
            //    //            u.isChecked = false;
            //    //        }
            //    //    });
            //    //}
            //}
        }

        public async void UpdatePayment(UserPayOutDetails userPayOutDetails)
        {
            //List<UserPayInDetails> lstuserPayInDetails = new List<UserPayInDetails>();
            if (userPayOutDetails != null && userPayOutDetails.ContributionId > 0)
            {
                IsLoading = true;
                bool isSuccess = await ServiceBase.SaveUserPayOutDetails(userPayOutDetails);
                IsLoading = false;
                if (isSuccess)
                {
                    SendNotification(userPayOutDetails);
                }
                else
                {
                    // await App.Current.MainPage.DisplayAlert("", "Something went wrong", "OK");
                }
            }
        }

        private async void SendNotification(UserPayOutDetails userPayOutDetails)
        {
            IsLoading = true;
            List<EmailNotificatinDetailsDto> lstemailNotificatinDetailsDto = new List<EmailNotificatinDetailsDto>();
            EmailNotificatinDetailsDto emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
            UserDto userDto = await ServiceBase.GetUserById(userPayOutDetails.UserId);
            List<NotificationDto> lstNotifications = await ServiceBase.GetNotifications(0);
            NotificationDto PaymentnotificationDto = new NotificationDto();
            if (lstNotifications != null && lstNotifications.Count > 0)
            {
                PaymentnotificationDto = lstNotifications.Where(x => x.NotificationType == (int)NotificationType.PayOutPaymentSuccess).FirstOrDefault();
                if (PaymentnotificationDto != null)
                {
                    emailNotificatinDetailsDto.NotificationId = PaymentnotificationDto.Id;
                    emailNotificatinDetailsDto.UserId = userDto.Id;
                    emailNotificatinDetailsDto.UserMail = userDto != null ? userDto.Email : null;
                    emailNotificatinDetailsDto.mailSubject = PaymentnotificationDto.Tittle;
                    if (PaymentnotificationDto.Message != null)
                    {
                        PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<username>", userDto.FirstName+" "+userDto.LastName);
                        PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<paidamount>", userPayOutDetails.PaidAmount.ToString());
                        // PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<<paidamount>>", userDto.CreateDate.ToString());
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
                    BindData();
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

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey("GroupContributionDetailPage"))
                {
                    IsLoading = true;
                    groupNumber = int.Parse(parameters["GroupContributionDetailPage"].ToString());
                    BindData();
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
