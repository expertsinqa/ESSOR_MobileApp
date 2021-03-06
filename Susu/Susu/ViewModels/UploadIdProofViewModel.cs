﻿using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Susu.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Susu.ViewModels
{
    public class UploadIdProofViewModel : ViewModelBase
    {
        #region Properties
        public ICommand CameraClicked { get { return new Command(UploadViaCamera); } }

        public ICommand FileUploadClicked { get { return new Command(uploadViaFile); } }

        public ICommand IsMoreClicked { get { return new Command(MoreClicked); } }

       

        //public UserDto userDto = null;
        #endregion
        #region Constructor
        public UploadIdProofViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion
        #region Functions
        /// <summary>
        /// When user click on uplod via camera
        /// </summary>
        public  void UploadViaCamera()
        {
            Photo();
        }

        /// <summary>
        /// When user click on uplod via camera
        /// </summary>
        public void uploadViaFile()
        {
            Upload();
        }

        /// <summary>
        /// Camer premission Alert
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckCameraPermissionAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != Xamarin.Essentials.PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status == PermissionStatus.Granted)
                    {
                        return true;
                    }
                }
                if (status == Xamarin.Essentials.PermissionStatus.Granted)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to take photo
        /// </summary>
        public async void Photo()
        {
            if (await CheckCameraPermissionAsync())
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Directory = "Sample",
                    Name = "test.jpg",
                    AllowCropping=true,
                });

                if (file == null)
                    return;

                var ImageBytes = ReadFully(file.GetStream());
                if(ImageBytes!=null && ImageBytes.Length>0)
                {
                    SaveGovernmentIdProof(ImageBytes, ".jpg", App.UserId);
                }
                 
                //await NavigationService.NavigateAsync("LandingPage");
            }

        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();


            }
        }

        /// <summary>
        /// Storage premission alert
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckStoragePermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (status != Xamarin.Essentials.PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageRead>();
                    if (status == PermissionStatus.Granted)
                    {
                        return true;
                    }
                }
                if (status == Xamarin.Essentials.PermissionStatus.Granted)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to upload photo
        /// </summary>
        public async void Upload()
        {
            if (await CheckStoragePermission())
            {
                try
                {
                    var mediaoptions = new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium
                    };
                    MediaFile selectedImage = await CrossMedia.Current.PickPhotoAsync(mediaoptions);
                    if (selectedImage == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Image not selected", "Ok");
                        return;
                    }
                    else
                    {
                        var ImageBytes = ReadFully(selectedImage.GetStream());
                        if (ImageBytes != null && ImageBytes.Length > 0)
                        {
                            SaveGovernmentIdProof(ImageBytes, ".jpg", App.UserId);
                            //  await NavigationService.NavigateAsync("LandingPage");
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("401"))
                    {

                    }


                }
            }
        }

        /// <summary>
        /// Method to save Id Proof
        /// </summary>
        /// <param name="file"></param>
        /// <param name="extension"></param>
        /// <param name="userId"></param>
        public async void SaveGovernmentIdProof(byte[] file,string extension,long userId=0)
        {
            
            try {
                IsLoading = true;
                bool isProofUploaded = await ServiceBase.SaveUserProofFile(file, extension, userId);
                IsLoading = true;
                if (isProofUploaded)
                {
                    App.Current.Properties["IsProfileUpdated"] = true;
                    await App.Current.SavePropertiesAsync();
                    await NavigationService.NavigateAsync("LandingPage");
                }
                else
                {
                    App.Current.Properties["IsProfileUpdated"] = false;
                    await App.Current.SavePropertiesAsync();
                    await App.Current.MainPage.DisplayAlert("Alert", "Something went wrong", "OK");
                }
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// When user click on MoreOption
        /// </summary>
        /// <param name="obj"></param>
        private void MoreClicked(object obj)
        {
            NavigationService.NavigateAsync("MorePage");
        }
        
        #endregion
    }
}
