using Prism.Navigation;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using static Susu.Models.Enums;

namespace Susu.ViewModels
{
    public class ViewDetailNotificationViewModel : ViewModelBase, INavigatedAware
    {
        #region Properties
        EmailNotificatinDetailsDto emailNotificatinDetailsDto = null;
        public string _NotificationTitle;
        public string NotificationTitle { get { return _NotificationTitle; } set { SetProperty(ref _NotificationTitle, value); } }

        public string _NotificationMessage;
        public string NotificationMessage { get { return _NotificationMessage; } set { SetProperty(ref _NotificationMessage, value); } }

        public ICommand BackClicked { get { return new Command(Back); } }

        public bool _isBackVisible = true;
        public bool isBackVisible { get { return _isBackVisible; } set { SetProperty(ref _isBackVisible, value); } }

        public bool _IsOrderSwap = false;
        public bool IsOrderSwap { get { return _IsOrderSwap; } set { SetProperty(ref _IsOrderSwap, value); } }

        public ICommand AcceptSwapClicked { get { return new Command(AcceptSwap); } }

        public ICommand RejectSwapClicked { get { return new Command(RejectSwap); } }

        public long FromUserId = 0;

        public long RequestToId = 0;
        bool isRequestToUser = false;

        public bool _IsAdminApprove = false;
        public bool IsAdminApprove { get { return _IsAdminApprove; } set { SetProperty(ref _IsAdminApprove, value); } }

        public ICommand AdminApproveClicked { get { return new Command(AdminApprove); } }

        public ICommand AdminRejectCicked { get { return new Command(AdminReject); } }

        public SwapUserDetails swapUserDetails = new SwapUserDetails();

        #endregion

        #region Constructor
        public ViewDetailNotificationViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Functions
        private async void Back()
        {
            await NavigationService.NavigateAsync("ViewNotifications");

        }

        /// <summary>
        /// Bind the notification Intially
        /// </summary>
        /// <param name="notificationId"></param>
        public async void BindData(long notificationId)
        {
            swapUserDetails = await ServiceBase.SwapUserDetailsById(notificationId);
            if (swapUserDetails != null)
            {

                RequestToId = swapUserDetails.RequestToId;
                FromUserId = swapUserDetails.RequestFromId;
                if (swapUserDetails.IsOrderRequestUserToUser != null && swapUserDetails.IsOrderRequestUserToUser.HasValue)
                {
                    if (swapUserDetails.IsOrderRequestUserToUser.Value == true)
                        isRequestToUser = true;
                    else
                        isRequestToUser = false;
                }
                if (App.IsGroupAdmin)
                {
                    if (swapUserDetails.LevelId == 1 && !swapUserDetails.IsCompleted)
                    {
                        isBackVisible = true;
                        IsOrderSwap = false;
                        IsAdminApprove = true;
                        return;
                    }
                    else if (swapUserDetails.LevelId == 1 && swapUserDetails.IsCompleted)
                    {
                        isBackVisible = true;
                        IsOrderSwap = false;
                        IsAdminApprove = false;
                        return;
                    }
                    else if (swapUserDetails.LevelId == 2 && swapUserDetails.IsCompleted)
                    {
                        isBackVisible = true;
                        IsOrderSwap = false;
                        IsAdminApprove = false;
                        return;
                    }
                    else if (swapUserDetails.LevelId == 0 && !swapUserDetails.IsCompleted && !isRequestToUser)
                    {
                        isBackVisible = false;
                        IsOrderSwap = true;
                        IsAdminApprove = false;
                        return;
                    }
                    else if (swapUserDetails.LevelId == 0 && !swapUserDetails.IsCompleted)
                    {
                        isBackVisible = true;
                        IsOrderSwap = false;
                        IsAdminApprove = false;
                        return;
                    }
                    else
                    {
                        isBackVisible = true;
                        IsOrderSwap = false;
                        IsAdminApprove = false;
                        return;
                    }
                }
                else
                {
                    if (swapUserDetails.LevelId == 1 && swapUserDetails.IsCompleted)
                    {
                        isBackVisible = true;
                        IsOrderSwap = false;
                        IsAdminApprove = false;
                    }
                    else if (swapUserDetails.LevelId == 1 && !swapUserDetails.IsCompleted)
                    {
                        isBackVisible = true;
                        IsOrderSwap = false;
                        IsAdminApprove = false;
                    }
                    else if (swapUserDetails.LevelId == 2 && swapUserDetails.IsCompleted)
                    {
                        isBackVisible = true;
                        IsOrderSwap = false;
                        IsAdminApprove = false;
                    }
                    else
                    {
                        isBackVisible = true;
                        IsOrderSwap = true;
                        IsAdminApprove = false;
                    }
                }
            }
        }

        /// <summary>
        /// Method hit when user  accept swap
        /// </summary>
        public void AcceptSwap()
        {
            if (emailNotificatinDetailsDto != null && emailNotificatinDetailsDto.Id > 0)
            {
                FromUserId = (long)emailNotificatinDetailsDto.FromUserId;
                SendNotification((int)NotificationType.UserAcceptChangeOrder);

            }
        }
        /// <summary>
        /// Method hit when user  reject swap
        /// </summary>
        public void RejectSwap()
        {
            if (emailNotificatinDetailsDto != null && emailNotificatinDetailsDto.Id > 0)
            {
                FromUserId = (long)emailNotificatinDetailsDto.FromUserId;
                SendNotification((int)NotificationType.UserRejectChangeOrder);
            }
        }
        /// <summary>
        /// Method hit when Admin  Approve swap
        /// </summary>
        public void AdminApprove()
        {
            if (emailNotificatinDetailsDto != null && emailNotificatinDetailsDto.Id > 0)
            {
                FromUserId = (long)emailNotificatinDetailsDto.FromUserId;
                SendNotification((int)NotificationType.AdminAcceptChangeOrder);

            }
        }
        /// <summary>
        /// Method hit when Admin  reject swap
        /// </summary>
        public void AdminReject()
        {
            if (emailNotificatinDetailsDto != null && emailNotificatinDetailsDto.Id > 0)
            {
                FromUserId = (long)emailNotificatinDetailsDto.FromUserId;
                SendNotification((int)NotificationType.AdminRejectChangeOrder);

            }
        }

        /// <summary>
        /// Method to send notification
        /// </summary>
        /// <param name="notificationType"></param>
        private async void SendNotification(int notificationType)
        {
            IsLoading = true;
            List<EmailNotificatinDetailsDto> lstemailNotificatinDetailsDto = new List<EmailNotificatinDetailsDto>();
            EmailNotificatinDetailsDto emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
            List<UserDto> UsersList = await ServiceBase.GetUsersByGroupId(App.GroupId > 0 ? App.GroupId : 0);
            List<UserDto> selectedUser = new List<UserDto>();

            if (UsersList != null && UsersList.Count > 0)
            {
                if (notificationType == (int)NotificationType.AdminAcceptChangeOrder || notificationType == (int)NotificationType.AdminRejectChangeOrder)
                {
                    selectedUser.Add(UsersList.Where(x => x.Id == FromUserId).FirstOrDefault());
                    selectedUser.Add(UsersList.Where(x => x.Id == RequestToId).FirstOrDefault());
                }
                else if (App.IsGroupAdmin && notificationType == (int)NotificationType.UserAcceptChangeOrder || notificationType == (int)NotificationType.UserRejectChangeOrder)
                {
                    selectedUser.Add(UsersList.Where(x => x.Id == FromUserId).FirstOrDefault());
                }
                else
                {
                    selectedUser.Add(UsersList.Where(x => x.Id == FromUserId).FirstOrDefault());
                    selectedUser.Add(UsersList.Where(x => x.RoleId == (int)Roles.groupadmin).FirstOrDefault());
                }
            }
            List<NotificationDto> lstNotifications = await ServiceBase.GetNotifications(0);
            NotificationDto AcceptSwapNotification = new NotificationDto();
            if (lstNotifications != null && lstNotifications.Count > 0)
            {
                AcceptSwapNotification = lstNotifications.Where(x => x.NotificationType == notificationType).FirstOrDefault();
                if (AcceptSwapNotification != null)
                {
                    selectedUser.ForEach(userDto =>
                    {
                        if (userDto != null)
                        {
                            emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
                            emailNotificatinDetailsDto.NotificationId = AcceptSwapNotification.Id;
                            emailNotificatinDetailsDto.UserId = userDto.Id;
                            emailNotificatinDetailsDto.UserMail = userDto != null ? userDto.Email : null;
                            emailNotificatinDetailsDto.mailSubject = AcceptSwapNotification.Tittle;
                            string RequestFirstName = UsersList.Where(x => x.Id == FromUserId).Select(x => x.FirstName).FirstOrDefault();
                            string RequestLastName = UsersList.Where(x => x.Id == FromUserId).Select(x => x.LastName).FirstOrDefault();
                            string RequestFromname = RequestFirstName + RequestLastName;
                            string RequestToFirstName = UsersList.Where(x => x.Id == RequestToId).Select(x => x.FirstName).FirstOrDefault();
                            string RequestToLastName = UsersList.Where(x => x.Id == RequestToId).Select(x => x.LastName).FirstOrDefault();
                            string RequestToName = RequestToFirstName + RequestToLastName;
                            string ApproveFirstName = UsersList.Where(x => x.RoleId == (int)Roles.groupadmin).Select(x => x.FirstName).FirstOrDefault();
                            string ApproveLastName = UsersList.Where(x => x.RoleId == (int)Roles.groupadmin).Select(x => x.LastName).FirstOrDefault();
                            string ApproveName = ApproveFirstName + ApproveLastName;
                            if (userDto != null && userDto.RoleId != null && userDto.RoleId == (int)Roles.groupadmin)
                            {
                                emailNotificatinDetailsDto.Isupdateswap = false;
                                if (userDto.RoleId == (int)Roles.groupadmin && userDto.Id == RequestToId)
                                {
                                    emailNotificatinDetailsDto.Isupdateswap = true;
                                }
                                //if (!isRequestToUser)
                                //    emailNotificatinDetailsDto.Isupdateswap = true;
                            }
                            else
                            {
                                if (userDto.Id == RequestToId)
                                    emailNotificatinDetailsDto.Isupdateswap = true;
                                else
                                    emailNotificatinDetailsDto.Isupdateswap = false;
                                if (!isRequestToUser)
                                    emailNotificatinDetailsDto.Isupdateswap = true;
                            }


                            if (AcceptSwapNotification.Message != null && notificationType == (int)NotificationType.UserAcceptChangeOrder || notificationType == (int)NotificationType.UserRejectChangeOrder)
                            {
                                AcceptSwapNotification.Message = AcceptSwapNotification.Message.Replace("<requesttoname>", RequestToName);
                                AcceptSwapNotification.Message = AcceptSwapNotification.Message.Replace("<requestfromname>", RequestFromname);
                            }
                            else if (AcceptSwapNotification.Message != null && notificationType == (int)NotificationType.AdminAcceptChangeOrder || notificationType == (int)NotificationType.AdminRejectChangeOrder)
                            {
                                AcceptSwapNotification.Message = AcceptSwapNotification.Message.Replace("<requesttoname>", RequestToName);
                                AcceptSwapNotification.Message = AcceptSwapNotification.Message.Replace("<requestfromname>", RequestFromname);
                                AcceptSwapNotification.Message = AcceptSwapNotification.Message.Replace("<approvername>", ApproveName);
                            }

                            emailNotificatinDetailsDto.NotificationMessage = AcceptSwapNotification.Message;
                            emailNotificatinDetailsDto.isReadbyUser = false;
                            if (notificationType == (int)NotificationType.RequestToChangeOrder || notificationType == (int)NotificationType.AdminAcceptChangeOrder || notificationType == (int)NotificationType.AdminRejectChangeOrder || notificationType == (int)NotificationType.UserAcceptChangeOrder || notificationType == (int)NotificationType.UserRejectChangeOrder)
                            {
                                emailNotificatinDetailsDto.FromUserId = FromUserId;
                                emailNotificatinDetailsDto.ToUserId = RequestToId;
                            }
                            else
                            {
                                emailNotificatinDetailsDto.FromUserId = App.UserId;
                                emailNotificatinDetailsDto.ToUserId = RequestToId;
                            }

                            emailNotificatinDetailsDto.NotificationType = notificationType;
                            emailNotificatinDetailsDto.IsOrderRequestUserToUser = isRequestToUser;
                            emailNotificatinDetailsDto.contributionId = App.contributionId;
                            emailNotificatinDetailsDto.GroupNumber = App.GroupNumber;
                            emailNotificatinDetailsDto.RequestId = swapUserDetails.RequestId;
                            //emailNotificatinDetailsDto.EmailBody = notificationDto.Message;
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
                    if (await App.Current.MainPage.DisplayAlert("", "Notification send successfully", "ok", " "))
                    {
                        await NavigationService.GoBackAsync();
                    }
                    //await NavigationService.GoBackAsync();
                    //BindData(emailNotificatinDetailsDto.Id);
                    //await App.Current.MainPage.DisplayAlert("", "Notification send successfully", "OK");
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
            if (parameters.ContainsKey("EmailNotification"))
            {
                IsLoading = true;
                emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
                emailNotificatinDetailsDto = (EmailNotificatinDetailsDto)parameters["EmailNotification"];
                if (emailNotificatinDetailsDto != null)
                {
                    NotificationTitle = emailNotificatinDetailsDto.mailSubject;
                    NotificationMessage = emailNotificatinDetailsDto.NotificationMessage;
                    BindData(emailNotificatinDetailsDto.Id);
                    //if (emailNotificatinDetailsDto.NotificationType == (int)NotificationType.RequestToChangeOrder)
                    //{
                    //    if (App.IsGroupAdmin)
                    //    {
                    //        IsOrderSwap = false;
                    //        isBackVisible = true;
                    //    }
                    //    else
                    //    {
                    //        IsOrderSwap = true;
                    //        isBackVisible = false;
                    //    }

                    //}
                    if (emailNotificatinDetailsDto.Id > 0)
                    {
                        ServiceBase.UpdateNotificationReadStatus(emailNotificatinDetailsDto.Id);
                    }
                }
                IsLoading = false;
            }
        }

        #endregion
    }


}
