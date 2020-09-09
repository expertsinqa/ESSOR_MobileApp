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
        #region properties
        public ICommand CloseClicked { get; set; }

        #endregion

        #region Constructor
        public PrivacyPolicyPageViewModel(INavigationService navigationService) : base(navigationService) {
            CloseClicked = new Command(close);
        }
        #endregion

        #region Functions
        private void close()
        {
            NavigationService.GoBackAsync();
        }
        #endregion
    }
}
