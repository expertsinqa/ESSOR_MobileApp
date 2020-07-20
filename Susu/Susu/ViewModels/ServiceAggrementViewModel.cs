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
        public UserDto userDto = null;
        public ICommand AcceptClicked { get { return new Command(Accept); } }
        public ICommand RejectClicked { get { return new Command(Reject); } }
        public ServiceAggrementViewModel(INavigationService navigationService):base(navigationService)
        {
            NavigationService = navigationService;
        }
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
    }
}
