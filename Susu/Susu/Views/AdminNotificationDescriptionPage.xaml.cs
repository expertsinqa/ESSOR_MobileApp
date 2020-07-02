using Susu.Models;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Susu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminNotificationDescriptionPage : ContentPage
    {
        AdminNotificationDescriptionPageViewModel vm;
        public AdminNotificationDescriptionPage()
        {
            InitializeComponent();
            vm = BindingContext as AdminNotificationDescriptionPageViewModel;
        }

        //private void AllRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    if(e.Value)
        //    {
        //        vm.isAllCheckboxChecked = true;
        //        vm.IsSelectedUsersVisible = false;
        //    }
        //    else
        //    {
        //        vm.isAllCheckboxChecked = false;
        //        vm.IsSelectedUsersVisible = false;
        //    }
        //}

        //private async void SpecificRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    if (e.Value)
        //    {
        //        await vm.GetUsers();
        //        vm.IsSelectedUsersVisible = true;
        //        vm.isSpecificCheckboxChed = true;
        //    }
        //    else
        //    {
        //        vm.isSpecificCheckboxChed = false;
        //        vm.IsSelectedUsersVisible = false;
        //    }
        //}

        private void CustomPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            vm.userDto = (UserDto)picker.SelectedItem;
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var picker = sender as Picker;
                //vm.userDto = (UserDto)e.SelectedItem;
                
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}