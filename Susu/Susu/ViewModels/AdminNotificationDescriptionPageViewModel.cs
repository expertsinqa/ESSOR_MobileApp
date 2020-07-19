using ESORR.Models;
using Prism.Navigation;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Susu.Models.Enums;

namespace Susu.ViewModels
{
    public class AdminNotificationDescriptionPageViewModel : ViewModelBase, INavigatedAware
    {
        #region Properties
        NotificationDto notificationDto = null;
        public string _NotificationTitle;
        public string NotificationTitle { get { return _NotificationTitle; } set { SetProperty(ref _NotificationTitle, value); } }

        public string _NotificationMessage;
        public string NotificationMessage { get { return _NotificationMessage; } set { SetProperty(ref _NotificationMessage, value); } }

        public List<UserDto> _UsersList;
        public List<UserDto> UsersList { get { return _UsersList; } set { SetProperty(ref _UsersList, value); } }

        public bool _IsSelectedUsersVisible = false;
        public bool IsSelectedUsersVisible { get { return _IsSelectedUsersVisible; } set { SetProperty(ref _IsSelectedUsersVisible, value); } }

        public UserDto userDto = new UserDto();

        public ICommand CloseClicked
        {
            get { return new Command(close); }
        }

        public ICommand SendClicked
        {
            get { return new Command(SendNotification); }
        }

        public bool isAllCheckboxChecked = false;
        public bool isSpecificCheckboxChed = false;

        public bool _IsUser = false;
        public bool IsUser { get { return _IsUser; } set { SetProperty(ref _IsUser, value); } }

        public bool _IsAdmin = false;
        public bool IsAdmin { get { return _IsAdmin; } set { SetProperty(ref _IsAdmin, value); } }

        public string RequestFromName = "";
        GroupContributionDetails groupContributionDetails = new GroupContributionDetails();


        #endregion

        #region Constrctor
        public AdminNotificationDescriptionPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
            if (App.IsGroupAdmin)
                IsAdmin = true;
            else
            {
                IsUser = false;
                Task.Run(async () => await GetUsers());
            }
            IsLoading = true;
            Task.Run(async () =>
            {
                groupContributionDetails = await ServiceBase.GetContributionDetailsByGroupNO(App.GroupNumber);
            });
            IsLoading = false;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            IsLoading = true;
            UsersList = await ServiceBase.GetUsersByGroupId(App.GroupId != null ? App.GroupId : 0);
            if(UsersList!=null && UsersList.Count>0)
            {
                string FirstName = UsersList.Where(x => x.Id == App.UserId).FirstOrDefault().FirstName;
                string LastName = UsersList.Where(x => x.Id == App.UserId).FirstOrDefault().LastName;
                RequestFromName = FirstName + LastName;
                UsersList = UsersList.Where(x => x.Id != App.UserId).ToList();
            }
            IsSelectedUsersVisible = true;
            IsLoading = false;
            return UsersList;
        }
        public async void close()
        {
            //await NavigationService.NavigateAsync("AdminNotificationPage");
           await NavigationService.GoBackAsync();
        }

        private async void SendNotification()
        {
            if(IsSelectedUsersVisible)
            {
                if(userDto.Id == 0)
                {
                    await App.Current.MainPage.DisplayAlert("","please select Users ","OK");
                    return;
                }
            }
            List<EmailNotificatinDetailsDto> lstemailNotificatinDetailsDto = new List<EmailNotificatinDetailsDto>();
            EmailNotificatinDetailsDto emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
            if(isSpecificCheckboxChed && notificationDto!=null && notificationDto.NotificationType == (int)NotificationType.RequestToChangeOrder)
            {
                List<UserDto> lstSelectedUsers = new List<UserDto>();
                bool isRequestToUser = true;
                UserDto user = UsersList.Where(x => x.RoleId == (int)Roles.groupadmin).FirstOrDefault();
                long Id = 0;
                if (user != null && user.Id > 0)
                {
                    
                    Id = user.Id;
                }
                else {
                    isRequestToUser = false;
                }
               // long Id = UsersList.Where(x => x.RoleId == (int)Roles.groupadmin).FirstOrDefault().Id;
                if(userDto.Id == Id)
                {
                    isRequestToUser = false;
                    lstSelectedUsers = UsersList.Where(x => x.Id == userDto.Id).ToList();
                }
                else
                {
                    lstSelectedUsers = UsersList.Where(x => x.Id == userDto.Id).ToList();
                    lstSelectedUsers.Add(UsersList.Where(x => x.RoleId == (int)Roles.groupadmin).FirstOrDefault());
                }
                lstSelectedUsers.ForEach(UserDto =>
                {
                    if (UserDto != null)
                    {
                        emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
                        emailNotificatinDetailsDto.NotificationId = notificationDto.Id;
                        emailNotificatinDetailsDto.UserId = UserDto != null ? UserDto.Id : 0;
                        emailNotificatinDetailsDto.UserMail = UserDto != null ? UserDto.Email : null;
                        emailNotificatinDetailsDto.mailSubject = notificationDto.Tittle;
                        if (!string.IsNullOrEmpty(notificationDto.Message))
                        {
                            string FirstName = UsersList.Where(x => x.Id == userDto.Id).FirstOrDefault().FirstName;
                            string LastName = UsersList.Where(x => x.Id == userDto.Id).FirstOrDefault().LastName;
                            string requestToName = FirstName + LastName;
                            // string requestFromName = UsersList.Where(x => x.Id == App.UserId).FirstOrDefault().FullName;
                            notificationDto.Message = notificationDto.Message.Replace("<requesttoname>", requestToName);
                            notificationDto.Message = notificationDto.Message.Replace("<requestfromname>", RequestFromName);
                        }
                        //if (UserDto.Id == userDto.Id)
                        //{
                        //    isRequestToUser = false;
                        //}
                        //else
                        //{
                        //    isRequestToUser = true;
                        //}

                        if (UserDto != null && UserDto.RoleId != null && UserDto.RoleId == (int)Roles.groupadmin)
                        {
                            emailNotificatinDetailsDto.Isupdateswap = false;
                            if (UserDto.Id == userDto.Id)
                                emailNotificatinDetailsDto.Isupdateswap = true;
                        }
                        else
                        {
                            emailNotificatinDetailsDto.Isupdateswap = true;
                        }

                        emailNotificatinDetailsDto.NotificationMessage = notificationDto.Message;
                        emailNotificatinDetailsDto.isReadbyUser = false;
                        emailNotificatinDetailsDto.FromUserId = App.UserId;
                        emailNotificatinDetailsDto.ToUserId = userDto.Id;
                        emailNotificatinDetailsDto.IsOrderRequestUserToUser = isRequestToUser;
                        emailNotificatinDetailsDto.contributionId = App.contributionId;
                        emailNotificatinDetailsDto.GroupNumber = App.GroupNumber;
                        // emailNotificatinDetailsDto.EmailBody = notificationDto.Message;
                        lstemailNotificatinDetailsDto.Add(emailNotificatinDetailsDto);
                    }
                });

            }
            else if (isSpecificCheckboxChed)
            {
                emailNotificatinDetailsDto.NotificationId = notificationDto.Id;
                emailNotificatinDetailsDto.UserId = userDto != null ? userDto.Id : 0;
                emailNotificatinDetailsDto.UserMail = userDto != null ? userDto.Email : null;
                emailNotificatinDetailsDto.mailSubject = notificationDto.Tittle;
                emailNotificatinDetailsDto.NotificationMessage = notificationDto.Message;
                emailNotificatinDetailsDto.isReadbyUser = false;
                emailNotificatinDetailsDto.FromUserId = App.UserId;
                emailNotificatinDetailsDto.contributionId = App.contributionId;
                emailNotificatinDetailsDto.GroupNumber = App.GroupNumber;
                //emailNotificatinDetailsDto.EmailBody = notificationDto.Message;
                lstemailNotificatinDetailsDto.Add(emailNotificatinDetailsDto);
            }
            else
            {
                UsersList = await GetUsers();
                if (UsersList != null && UsersList.Count > 0)
                {
                    IsSelectedUsersVisible = false;
                    UsersList.ForEach(UserDto =>
                    {
                        if (UserDto != null)
                        {
                            emailNotificatinDetailsDto = new EmailNotificatinDetailsDto();
                            emailNotificatinDetailsDto.NotificationId = notificationDto.Id;
                            emailNotificatinDetailsDto.UserId = UserDto != null ? UserDto.Id : 0;
                            emailNotificatinDetailsDto.UserMail = UserDto != null ? UserDto.Email : null;
                            emailNotificatinDetailsDto.mailSubject = notificationDto.Tittle;
                            if(notificationDto!=null && notificationDto.NotificationType == (int)NotificationType.ContributionPaymentReminder)
                            {
                                notificationDto.Message = notificationDto.Message.Replace("<date>", string.Format("{0:M/d/yyyy}", groupContributionDetails?.ContributionDate));
                                notificationDto.Message = notificationDto.Message.Replace("<contributionamount>", App.Amount);
                            }
                            emailNotificatinDetailsDto.NotificationMessage = notificationDto.Message;
                            emailNotificatinDetailsDto.isReadbyUser = false;
                            emailNotificatinDetailsDto.FromUserId = App.UserId;
                            emailNotificatinDetailsDto.contributionId = App.contributionId;
                            emailNotificatinDetailsDto.GroupNumber = App.GroupNumber;
                            // emailNotificatinDetailsDto.EmailBody = notificationDto.Message;
                            lstemailNotificatinDetailsDto.Add(emailNotificatinDetailsDto);
                        }
                    });
                }
            }

            if(lstemailNotificatinDetailsDto!=null && lstemailNotificatinDetailsDto.Count>0)
            {
                IsLoading = true;
                bool isSuccess = await ServiceBase.SaveAndSendNotificationMail(lstemailNotificatinDetailsDto);
                IsLoading = false;
                if (isSuccess)
                {
                    //await App.Current.MainPage.DisplayAlert("", "Notification send successfully", "OK");
                    if(await App.Current.MainPage.DisplayAlert("", "Notification send successfully", "ok"," "))
                    {
                        await NavigationService.GoBackAsync();
                    }
                }
                else
                {

                }
            }
            else
            {

            }
            
        }
   
        
        #endregion
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey("Notification"))
                {
                    IsLoading = true;
                    notificationDto = new NotificationDto();
                    notificationDto = (NotificationDto)parameters["Notification"];
                    if (notificationDto != null)
                    {
                        NotificationTitle = notificationDto.Tittle;
                        NotificationMessage = notificationDto.Message;
                        if(notificationDto.NotificationLevel == (int)NotificationLevel.SpecificUser)
                        {
                            isSpecificCheckboxChed = true;
                            await GetUsers();
                        }
                        else
                        {
                            isAllCheckboxChecked = true;
                        }
                    }
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
