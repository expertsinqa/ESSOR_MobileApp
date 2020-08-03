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
    public class GroupPayoutDetailsViewModel : ViewModelBase, INavigationAware
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
        public bool IsAdmin { get { return _IsAdmin; } set { SetProperty(ref _IsAdmin, value); } }

        public string _PaidMembers = "";
        public string PaidMembers { get { return _PaidMembers; } set { SetProperty(ref _PaidMembers, value); } }
        public string _UnPaidMembers = "";
        public string UnPaidMembers { get { return _UnPaidMembers; } set { SetProperty(ref _UnPaidMembers, value); } }

        public List<UserPayInDetails> _usersPayInDetails;
        public List<UserPayInDetails> UserPayInDetails { get { return _usersPayInDetails; } set { SetProperty(ref _usersPayInDetails, value); } }

        public string NextPaymentUserName { get; set; }
        public string NextPaymentDate { get; set; }

        private async void Back(object obj)
        {
            await NavigationService.GoBackAsync();
        }

        public GroupPayoutDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
        }

        private async void BindData()
        {
            bool isContributionCompleted = false;
            if (App.IsGroupAdmin)
                IsAdmin = true;
            else
                IsAdmin = false;
            IsLoading = true;
            groupContributionDetails = new GroupContributionDetails();
            groupContributionDetails = await ServiceBase.GetContributionDetailsByGroupNO(groupNumber);
            if (groupContributionDetails != null)
            {
                UserPayInDetails = await ServiceBase.GetPayInDetailByGroupNO(groupNumber, groupContributionDetails.ContributionId);
                if (UserPayInDetails != null && UserPayInDetails.Count > 0)
                {
                    int userscount = UserPayInDetails.Count();
                    int paidUsers = UserPayInDetails.Where(x => x.isPaymentCompleted).Count();
                    if (userscount == paidUsers)
                    {
                        isContributionCompleted = true;
                    }
                    else
                    {
                        isContributionCompleted = false;
                    }
                }
            }
            UserPayOutDetails = await ServiceBase.GetPayOutDetailsByGroupNO(groupNumber);
            if (UserPayOutDetails != null && UserPayOutDetails.Count > 0)
            {
                int Pm = UserPayOutDetails.Where(x => x.isPaymentCompleted == true).Count();
                int UPm = UserPayOutDetails.Where(x => x.isPaymentCompleted == false).Count();
                if (Pm < 10)
                {
                    PaidMembers = "0" + Pm;
                }
                else
                {
                    PaidMembers = Pm.ToString();
                }
                if (UPm < 10)
                {
                    UnPaidMembers = "0" + UPm;
                }
                else
                {
                    UnPaidMembers = UPm.ToString();
                }
                UserPayOutDetails.ForEach(u =>
                {
                    if (u != null)
                    {
                        if (u.ContributionId == App.contributionId && IsAdmin && isContributionCompleted)
                        {
                            u.IsEnabled = true;
                        }
                        else
                        {
                            u.IsEnabled = false;
                        }
                    }
                });
                int nextpaymentContributionId = UserPayOutDetails.Where(x => x.ContributionId == App.contributionId).FirstOrDefault().ContributionId + 1;
                if (nextpaymentContributionId > 0)
                {
                    UserPayOutDetails userPayOut = UserPayOutDetails.Where(x => x.ContributionId == nextpaymentContributionId).FirstOrDefault();
                    if (userPayOut != null)
                    {
                        NextPaymentUserName = userPayOut.UserName;
                        NextPaymentDate = userPayOut.PayOutDate.ToString("M/d/yyyy");
                        //NextPaymentUserName = UserPayOutDetails.Where(x => x.ContributionId == nextpaymentContributionId)?.FirstOrDefault().PayOutDate.ToString("d/M/yyyy");
                        //NextPaymentDate = UserPayOutDetails.Where(x => x.ContributionId == nextpaymentContributionId)?.FirstOrDefault().PayOutDate.ToString("d/M/yyyy");
                    }
                }
            }
            IsLoading = false;
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
            //EmailNotificatinDetailsDto emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
            UserDto userDto = await ServiceBase.GetUserById(userPayOutDetails.UserId);
            List<NotificationDto> lstNotifications = await ServiceBase.GetNotifications(0);
            NotificationDto PaymentnotificationDto = new NotificationDto();
            if (lstNotifications != null && lstNotifications.Count > 0)
            {
                PaymentnotificationDto = lstNotifications.Where(x => x.NotificationType == (int)NotificationType.PayOutPaymentSuccess).FirstOrDefault();
                if (PaymentnotificationDto != null)
                {
                    UserPayInDetails.ForEach(UserPayInDetails =>
                    {
                        EmailNotificatinDetailsDto emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
                        if (UserPayInDetails != null)
                        {
                            emailNotificatinDetailsDto.NotificationId = PaymentnotificationDto.Id;
                            emailNotificatinDetailsDto.UserId = UserPayInDetails.Id;
                            emailNotificatinDetailsDto.UserMail = UserPayInDetails.UserMail;
                            emailNotificatinDetailsDto.mailSubject = PaymentnotificationDto.Tittle;
                            if (PaymentnotificationDto.Message != null)
                            {
                                PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<username>", userDto.FirstName + " " + userDto.LastName);
                                PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<paidamount>", userPayOutDetails.PaidAmount.ToString());
                                PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<nextusername>", NextPaymentUserName);
                                PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<nextpayoutdate>", NextPaymentDate);
                                // PaymentnotificationDto.Message = PaymentnotificationDto.Message.Replace("<<paidamount>>", userDto.CreateDate.ToString());
                            }
                            emailNotificatinDetailsDto.NotificationMessage = PaymentnotificationDto.Message;
                            emailNotificatinDetailsDto.isReadbyUser = false;
                            emailNotificatinDetailsDto.FromUserId = App.UserId;
                            lstemailNotificatinDetailsDto.Add(emailNotificatinDetailsDto);
                        }
                    });
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
