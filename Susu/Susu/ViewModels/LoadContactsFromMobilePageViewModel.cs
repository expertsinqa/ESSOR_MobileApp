using Plugin.ContactService.Shared;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace Susu.Views
{
    public class LoadContactsFromMobilePageViewModel : ViewModelBase
    {
        #region Constructor
        public ObservableCollection<Contact> contactslist;
        public ObservableCollection<Contact> ContactList { get { return contactslist; } set { SetProperty(ref contactslist, value); } }

        public List<Contact> SelectedContacts = new List<Contact>();
        public List<Contact> FinallySelected = null;


        public ICommand BackClicked
        {
            get
            {
                return new Command(Back);
            }
        }
        public ICommand selectionChanged
        {
            get
            {
                return new Command(Select);
            }
        }

        public ICommand DoneClicked
        {
            get
            {
                return new Command(SaveContacts);
            }
        }

        #endregion

        #region Constructor
        public LoadContactsFromMobilePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            ReadContacts();
        }
        #endregion
        #region Functions
        public async void ReadContacts()
        {
            
                try
                {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<ContactsPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Contacts))
                    {
                        await App.Current.MainPage.DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<ContactsPermission>();
                    if (status == PermissionStatus.Granted)
                    {
                        Loadcontacts();
                    }
                }
                if (status == PermissionStatus.Granted)
                {
                    Loadcontacts();
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //location denied
                }
            }
                catch (Exception ex)
                {
                    //Something went wrong
                }
           
        }
        public void Back()
        {
            NavigationService.NavigateAsync("CreateGroupPage");
        }
        public void Select()
        {
            //var data = SelectedContacts;
        }
        public async void SaveContacts()
        {
            if (SelectedContacts != null && SelectedContacts.Count > 0)
            {
                FinallySelected = SelectedContacts.Where(e => e.Number != null).ToList();
            }
            await NavigationService.NavigateAsync("SamplePage");
        }

        public async void Loadcontacts()
        {
            IsLoading = true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
                if (contacts != null && contacts.Count > 0)
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        ContactList = new ObservableCollection<Contact>(contacts.Where(x => x.Number != null));
                        IsLoading = false;
                    }
                    else if(Device.RuntimePlatform == Device.iOS)
                    {
                        Contact contacts1 = new Contact();
                        ContactList = new ObservableCollection<Contact>();
                        foreach (var items in contacts)
                        {
                            contacts1.Name = items.Name;
                            contacts1.Number= getBetween(items.Number, "stringValue=", ", ");
                            ContactList.Add(contacts1);
                            IsLoading = false;
                        }

                        //ContactList = new ObservableCollection<Contact>(contacts.Where(x => x.Number != null));
                        
                    }
                }
            });

        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }



        //public async System.Threading.Tasks.Task<bool> checkCameraPermission()
        //{
        //    var status = await CrossPermissions.Current.CheckPermissionStatusAsync<ContactsPermission>();
        //            if (status != PermissionStatus.Granted)
        //            {
        //                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Contacts))
        //                {
        //                    await App.Current.MainPage.DisplayAlert("Need location", "Gunna need that location", "OK");
        //                }

        //               var res = await CrossPermissions.Current.RequestPermissionAsync<ContactsPermission>();
        //        if(res == PermissionStatus.Granted)
        //        {
        //            return true;
        //        }

        //            }
        //            if (status == PermissionStatus.Granted)
        //            {
        //        return true;
        //            }
        //            else if (status != PermissionStatus.Unknown)
        //            {
        //        return false;
        //            }
        //    return false;
        //}

        #endregion
    }
}
