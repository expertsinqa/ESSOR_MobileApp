using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Susu.ViewModels
{
    public class PaymentsPageViewModel : ViewModelBase
    {
        public ICommand NextClciked { get { return new Command(Next); } }
        public ICommand PayNowClicked { get { return new Command(PayNow); } }

        public ICommand paymentContributionDetailsClicked {get { return new Command(paymentContribution); } }
        public PaymentsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
        }
        public void Next()
        {

        }
        public async void PayNow()
        {
            //var result = await CrossPayPalManager.Current.Buy(new PayPalItem("ESORR", new Decimal(12.50), "USD"), new Decimal(0));
            //if (result.Status == PayPalStatus.Cancelled)
            //{
            //    //Debug.WriteLine("Cancelled");
            //}
            //else if (result.Status == PayPalStatus.Error)
            //{
            //    //Debug.WriteLine(result.ErrorMessage);
            //}
            //else if (result.Status == PayPalStatus.Successful)
            //{
            //   // Debug.WriteLine(result.ServerResponse.Response.Id);
            //}
        }

        private async void paymentContribution()
        {
            if(NavigationService!=null)
            await NavigationService.NavigateAsync("GroupContributionDetailPage");
            else
            {
                
            }
        }

    }
}
