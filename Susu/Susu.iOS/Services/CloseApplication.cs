using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Foundation;
using Susu.Interface;
using Susu.iOS.Services;
using UIKit;
using Xamarin.Forms;

[assembly:Dependency(typeof(CloseApplication))]
namespace Susu.iOS.Services
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            Thread.CurrentThread.Abort();
        }
    }
}