using Susu.Droid.Services;
using Susu.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseApplication))]
namespace Susu.Droid.Services
{
    public  class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}