using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Susu.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Susu.ViewModels
{
    public class ViewModelBase : BindableBase, IDestructible
    {
        public INavigationService NavigationService { get;  set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool _IsLoading = false;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { SetProperty(ref _IsLoading, value); }
        }

        IServiceBase serviceBase;
        public IServiceBase ServiceBase
        {
            get
            {
                if (serviceBase == null)
                {
                    serviceBase = Xamarin.Forms.DependencyService.Get<IServiceBase>();
                }
                return serviceBase;
            }
            set
            {
                SetProperty(ref serviceBase, value);
            }

        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }


        public virtual void Destroy()
        {

        }

        //public void OnNavigatedFrom(NavigationParameters parameters)
        //{
        //    throw new NotImplementedException();
        //}

        //public  void OnNavigatedTo(NavigationParameters parameters)
        //{
        //    throw new NotImplementedException();
        //}

        //public  void OnNavigatingTo(NavigationParameters parameters)
        //{
        //    throw new NotImplementedException();
        //}

        //public  void OnNavigatedFrom(INavigationParameters parameters)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual void OnNavigatedTo(INavigationParameters parameters)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
