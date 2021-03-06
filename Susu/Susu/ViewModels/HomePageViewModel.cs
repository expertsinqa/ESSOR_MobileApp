﻿using Prism.Navigation;
using Susu.Models;
using Susu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ESORR.ViewModels
{
    public class HomePageViewModel: ViewModelBase
    {
        #region Properties
        public ICommand GroupInfoClicked { get { return new Command(GroupInfo); } }
        public ICommand GroupUsersClicked { get { return new Command(GroupUsers); } }
        public ICommand PaymentsClicked { get { return new Command(Payment); } }
        public ICommand ProfileClicked { get { return new Command(Profile); } }
        public ICommand MoreClicked { get { return new Command(More); } }

        public ICommand UpdateAppClicked { get { return new Command(AppUpdate); } }
        public ICommand CancelAppClicked { get { return new Command(Cancel); } }

        public bool _IsAppUpdateVisible = false;
        public bool IsAppUpdateVisible { get { return _IsAppUpdateVisible; } set { SetProperty(ref _IsAppUpdateVisible, value); } }
        public string _AppUpdateText = "";
        public string AppUpdateText { get { return _AppUpdateText; } set { SetProperty(ref _AppUpdateText, value); } }



        #endregion
        #region Constructor
        public HomePageViewModel(INavigationService navigationService): base(navigationService)
        {
            NavigationService = navigationService;
            GetAppVersionDetails();
        }
        #endregion
        #region Functions
        /// <summary>
        /// App version update call
        /// </summary>
        public async void GetAppVersionDetails()
        {
            List<APPVersionDetails> appVersionDetails = new List<APPVersionDetails>();
            try
            {
                IsLoading = true;
                appVersionDetails = await ServiceBase.GetAppVesrion();
                if (appVersionDetails != null && appVersionDetails.Count > 0)
                {
                    var currentVersion = VersionTracking.CurrentVersion;
                    if (!string.IsNullOrEmpty(currentVersion))
                    {
                        double version = double.Parse(currentVersion);
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            double androidVersion = 0;
                            if (appVersionDetails[0] != null && !string.IsNullOrEmpty(appVersionDetails[0].VersionNUmber.ToString()))
                            {
                                androidVersion = appVersionDetails[0].VersionNUmber;
                            }
                            if (version < androidVersion)
                            {
                                AppUpdateText = "An updated version of app is available.";
                                IsAppUpdateVisible = true;
                            }
                            else
                            {
                                IsAppUpdateVisible = false;
                            }
                        }
                        else
                        {
                            double iosVersion = 0;
                            if (appVersionDetails[1] != null && !string.IsNullOrEmpty(appVersionDetails[1].VersionNUmber.ToString()))
                                iosVersion = appVersionDetails[1].VersionNUmber;
                            if (version < iosVersion)
                            {
                                AppUpdateText = "An updated version of app is available.";
                                IsAppUpdateVisible = true;
                            }
                            else
                            {
                                IsAppUpdateVisible = false;
                            }
                        }
                        //if (version < appVersionDetails[0].VersionNUmber)
                        //{
                        //    if (Device.RuntimePlatform == Device.Android)
                        //    {
                        //        AppUpdateText = "An updated version of app is available.";
                        //        IsAppUpdateVisible = true;
                        //    }
                        //    else
                        //    {
                        //        //AppUpdateText = "An updated version of app is available.";
                        //        //IsAppUpdateVisible = true;
                        //    }
                        //    //IsAppUpdateVisible = true;
                        //}
                        //else
                        //{
                        //    IsAppUpdateVisible = false;
                        //}
                    }
                    IsLoading = false;
                }
                else
                {
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
            }

        }

        /// <summary>
        /// This method hits when the user click on GroupInfo
        /// </summary>
        private void GroupInfo()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "GroupInfo");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }
        /// <summary>
        /// This method hits when the user click on GroupUsers Tab
        /// </summary>
        private void GroupUsers()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "GroupUsers");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }

        /// <summary>
        /// This method hits when the user click on More Tab
        /// </summary>
        private void More()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "More");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }
        /// <summary>
        /// This method hits when the user click on Profile Tab
        /// </summary>

        private void Profile()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "Profile");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }

        /// <summary>
        /// This method hits when the user click on Payment Tab
        /// </summary>
        private void Payment()
        {
            IsLoading = true;
            NavigationParameters np = new NavigationParameters();
            np.Add("IsFromHomePage", true);
            np.Add("TappedIcon", "Payment");
            NavigationService.NavigateAsync("SamplePage", np);
            IsLoading = false;
        }

        /// <summary>
        /// This method to navigate to stores fro app updates
        /// </summary>
        private async void AppUpdate()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.esorr.esorrApp&hl=en_IN");
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                IsAppUpdateVisible = false;
            }
            else
            {
                Uri uri = new Uri("https://apps.apple.com/us/app/id1523820384");
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                IsAppUpdateVisible = false;
            }
        }

        /// <summary>
        /// This method hits when the user click on Cancel button in app update
        /// </summary>
        private void Cancel()
        {
            IsAppUpdateVisible = false;
        }
        #endregion
    }
}
