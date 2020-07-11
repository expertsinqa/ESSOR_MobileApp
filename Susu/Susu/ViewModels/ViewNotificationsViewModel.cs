using Prism.Navigation;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;

namespace Susu.ViewModels
{
    public class ViewNotificationsViewModel : ViewModelBase
    {
        public List<EmailNotificatinDetailsDto> _lstEmailNotificationDto = null;
        public List<EmailNotificatinDetailsDto> lstEmailNotificationDto { get { return _lstEmailNotificationDto; } set { SetProperty(ref _lstEmailNotificationDto, value); } }

        public System.Windows.Input.ICommand BackClicked { get { return new Xamarin.Forms.Command(Back); } }
        //ObservableCollection<EmailNotificatinDetailsDto> lstEmailNotificationDto = null;
        #region Constructor
        public ViewNotificationsViewModel(INavigationService navigationService):base(navigationService)
        {
            NavigationService = navigationService;
            BindData();

        }
        #endregion

        #region Functions

        public async void BindData()
        {
            try
            {
                IsLoading = true;
                lstEmailNotificationDto = new List<EmailNotificatinDetailsDto>();
                lstEmailNotificationDto = await ServiceBase.GetUserNotificationsById(App.UserId);
                IsLoading = false;
            }
            catch(Exception ex)
            {

            }
        }
        public async void ViewNotification(EmailNotificatinDetailsDto emailNotificatinDetailsDto)
        {
            NavigationParameters np = new NavigationParameters();
            np.Add("EmailNotification", emailNotificatinDetailsDto);
            await NavigationService.NavigateAsync("ViewDetailNotification", np);
        }

        public async void Back()
        {
            await NavigationService.NavigateAsync("SamplePage");
        }
        #endregion

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        //public void OnNavigatedTo(INavigationParameters parameters)
        //{
        //    if (parameters.ContainsKey("NotificationsList"))
        //    {
        //        IsLoading = true;
        //        lstEmailNotificationDto = new List<EmailNotificatinDetailsDto>();
        //        List<EmailNotificatinDetailsDto> lst = new List<EmailNotificatinDetailsDto>();
        //        lst = (List<EmailNotificatinDetailsDto>)parameters["NotificationsList"];
        //        lstEmailNotificationDto = new List<EmailNotificatinDetailsDto>(lst);
        //        IsLoading = false;
        //    }
        //}
    }
}
