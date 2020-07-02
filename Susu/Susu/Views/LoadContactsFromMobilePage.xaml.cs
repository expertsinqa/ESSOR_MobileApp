using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Susu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadContactsFromMobilePage : ContentPage
    {
        LoadContactsFromMobilePageViewModel viewModel;
        public LoadContactsFromMobilePage()
        {
            InitializeComponent();
           viewModel =  BindingContext as LoadContactsFromMobilePageViewModel;
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel != null && viewModel.SelectedContacts != null)
            {
                if (e != null && e.CurrentSelection != null && e.CurrentSelection.Count > 0)
                {
                    viewModel.SelectedContacts = new List<Contact>();
                    foreach (var item in e.CurrentSelection)
                    {
                        viewModel.SelectedContacts.Add((Contact)item);
                    }
                }

            }

        }

        
    }
}