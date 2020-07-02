using Susu.Models;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Susu.Models.Enums;

namespace Susu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminNotificationPage : ContentPage
    {
        AdminNotificationPageViewModel vm;
        public AdminNotificationPage()
        {
            InitializeComponent();
            vm = BindingContext as AdminNotificationPageViewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            NotificationDto notificationDto = new NotificationDto();
            notificationDto = (NotificationDto)e.Item;
            if (notificationDto != null)
            {
                if (vm != null)
                {
                    vm.selectedNotification(notificationDto);
                }
                else
                {
                    vm = BindingContext as AdminNotificationPageViewModel;
                    vm.selectedNotification(notificationDto);
                }
            }
        }

        private void AllRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                int id = (int)NotificationLevel.AllUsers;
                vm.BindData(id);
            }
            else
            {
                //vm.isAllCheckboxChecked = false;
                //vm.IsSelectedUsersVisible = false;
            }
        }

        private void SpecificRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                int id = (int)NotificationLevel.SpecificUser;
                vm.BindData(id);
            }
            else
            {
                //vm.isSpecificCheckboxChed = false;
                //vm.IsSelectedUsersVisible = false;
                //}
            }

        }
    }
}