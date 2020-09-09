using Prism.Navigation;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static Susu.Models.Enums;

namespace Susu.ViewModels
{
    public class AdminNotificationPageViewModel: ViewModelBase,INavigatedAware
    {
        #region Properties

        public List<NotificationDto> _lstnotificationDto;
        public List<NotificationDto> lstnotificationDto { get { return _lstnotificationDto; } set { SetProperty(ref _lstnotificationDto, value); } }
        public ICommand BackClicked { get { return new Command(Back); } }

        public bool _IsListViewVisible = false;
        public bool IsListViewVisible { get { return _IsListViewVisible; } set { SetProperty(ref _IsListViewVisible, value); } }

        public int notificationlevelId = 0;

        public bool _IsSendToVisible = true;
        public bool IsSendToVisible { get { return _IsSendToVisible; } set { SetProperty(ref _IsSendToVisible, value); } }

        public DateTime ContributionDate;

        #endregion
        #region Constructor
        public AdminNotificationPageViewModel(INavigationService navigationService):base(navigationService)
        {
            NavigationService = navigationService;
            
               
        }
        #endregion

        #region Functions
        /// <summary>
        /// Intially bind the data to the screen
        /// </summary>
        /// <param name="Id"></param>
        public async void BindData(int Id)
        {
            IsLoading = true;
            List<NotificationDto> lstAllnotificationDto = await ServiceBase.GetNotifications(Id);
            IsLoading = false;
            if(lstAllnotificationDto != null && lstAllnotificationDto.Count>0)
            {
                IsListViewVisible = true;
                if(App.IsGroupAdmin )
                {
                    if(System.DateTime.Now.Date <= ContributionDate.Date)
                    {
                        lstnotificationDto = lstAllnotificationDto.Where(y => y.Status == true).ToList();
                    }
                    else
                    {
                        lstnotificationDto = lstAllnotificationDto.Where(x => x.NotificationType != (int)NotificationType.RequestToChangeOrder).Where(y => y.Status == true).ToList();
                    }
                     
                }
                if(!App.IsGroupAdmin && System.DateTime.Now.Date <= ContributionDate.Date)
                {
                    lstnotificationDto = lstAllnotificationDto.Where(x => x.NotificationType == (int)NotificationType.RequestToChangeOrder).Where(y => y.Status == true).ToList();
                }

            }

        }

        /// <summary>
        /// When user click on Back button
        /// </summary>
        private async void Back()
        {
           await NavigationService.NavigateAsync("SamplePage");
        }

        /// <summary>
        /// When the user click on specific notification
        /// </summary>
        /// <param name="notificationDto"></param>
        public async void selectedNotification(NotificationDto notificationDto)
        {
            NavigationParameters np = new NavigationParameters();
            np.Add("Notification", notificationDto);
            await NavigationService.NavigateAsync("AdminNotificationDescriptionPage",np);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// When the page navigating from previous page
        /// </summary>
        /// <param name="parameters"></param>
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey("ContributionDate"))
                {
                    IsLoading = true;
                    ContributionDate = (DateTime)parameters["ContributionDate"];
                    if (!App.IsGroupAdmin && System.DateTime.Now.Date <= ContributionDate.Date)
                    {
                        BindData((int)NotificationLevel.SpecificUser);
                        IsSendToVisible = false;
                    }
                    else
                    {
                        IsSendToVisible = false;
                    }
                    if (App.IsGroupAdmin)
                        IsSendToVisible = true;

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
