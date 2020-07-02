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
            long userId = await ServiceBase.SaveUser(userDto);
            if(userId>0)
            {
                await NavigationService.NavigateAsync("UploadIdProof");
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
