using Prism.Navigation;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ESORR.ViewModels
{
    public class PrivacyPolicyPageViewModel : ViewModelBase
    {
        public ICommand CloseClicked { get; set; }
        public PrivacyPolicyPageViewModel(INavigationService navigationService) : base(navigationService) {
            CloseClicked = new Command(close);
        }

        private void close()
        {
            NavigationService.GoBackAsync();
        }
    }
}
