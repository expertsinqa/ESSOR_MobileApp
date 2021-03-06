﻿using ESORR.Models;
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
        #region Properties
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
        public double _listViewHeightRequest = 0;
        public double listViewHeightRequest { get { return _listViewHeightRequest; } set { SetProperty(ref _listViewHeightRequest, value); } }

        #endregion
       

        #region Constructor
        public GroupPayoutDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Function
        /// <summary>
        /// Bind the payout details 
        /// </summary>
        private async void BindData()
        {
            try
            {
                bool isContributionCompleted = false;
                if (App.IsGroupAdmin)
                    IsAdmin = true;
                else
                    IsAdmin = false;

                groupContributionDetails = new GroupContributionDetails();
                IsLoading = true;
                groupContributionDetails = await ServiceBase.GetContributionDetailsByGroupNO(groupNumber);
                IsLoading = false;
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
                IsLoading = true;
                UserPayOutDetails = await ServiceBase.GetPayOutDetailsByGroupNO(groupNumber);
                IsLoading = false;
                if (UserPayOutDetails != null && UserPayOutDetails.Count > 0)
                {
                    listViewHeightRequest = UserPayOutDetails.Count * 20;
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
                                if (u.isPaymentCompleted == false && u.BufferPayOutDate >= DateTime.Now && u.ContributionId <= App.contributionId && isContributionCompleted)
                                {
                                    u.IsEnabled = true;
                                }
                                else
                                {
                                    u.IsEnabled = false;
                                }
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
            }
            catch(Exception ex)
            {
                IsLoading = false;
            }
          
        }

        /// <summary>
        /// This method hits when the admin check the checkbox
        /// </summary>
        /// <param name="userPayOutDetails"></param>
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

        /// <summary>
        /// This method is send the notification
        /// </summary>
        /// <param name="userPayOutDetails"></param>
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
                            emailNotificatinDetailsDto.UserId = UserPayInDetails.UserId;
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

        /// <summary>
        ///  This method hits when the user click on back button
        /// </summary>
        /// <param name="obj"></param>
        private async void Back(object obj)
        {
            await NavigationService.GoBackAsync();
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method get the data from the previous page
        /// </summary>
        /// <param name="parameters"></param>
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

        #endregion
    }
}
