using Prism.Navigation;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Susu.ViewModels
{
    public class ServiceAggrementViewModel : ViewModelBase,INavigationAware
    {
        #region Properties
        public UserDto userDto = null;
        public ICommand AcceptClicked { get { return new Command(Accept); } }
        public ICommand RejectClicked { get { return new Command(Reject); } }

        #endregion
        #region Constructor
        public ServiceAggrementViewModel(INavigationService navigationService):base(navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Functions
        /// <summary>
        /// When user click accept aggrement
        /// </summary>
        public async void Accept()
        {
            userDto.IsAcceptAggrement = true;
            userDto.IsUpdateAggrement = true;
            UserDto userDetails = await ServiceBase.SaveUser(userDto);
            if(userDetails!=null && userDetails.Id>0)
            {
                App.Current.Properties["IsAggrementAccepted"] = true;
                await App.Current.SavePropertiesAsync();
                await NavigationService.NavigateAsync("UploadIdProof");
            }
            else
            {
                App.Current.Properties["IsAggrementAccepted"] = false;
                await App.Current.SavePropertiesAsync();
            }
            
        }
        /// <summary>
        /// When user  Rejject aggrement
        /// </summary>
        public async void Reject()
        {
            await NavigationService.NavigateAsync("LoginPage");
        }

        public  void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey("userDto"))
                {
                    userDto = new UserDto();
                    userDto = (UserDto)parameters["userDto"];
                }
            }
            catch (Exception ex)
            {

               
            }

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
           // throw new NotImplementedException();
        }

        #endregion
    }
}
