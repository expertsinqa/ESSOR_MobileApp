using Android.OS;
using ESORR.Interface;
using Firebase;
using Firebase.Analytics;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency(typeof(Susu.Droid.Services.FirebaseAnalyticsAndroid))]
namespace Susu.Droid.Services
{
    public class FirebaseAnalyticsAndroid: IFirebaseAnalytics
    {
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                {paramName, value}
            }); ;
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            
            var firebaseAnalytics = FirebaseAnalytics.GetInstance(Android.App.Application.Context);

            if (parameters == null)
            {
                firebaseAnalytics.LogEvent(eventId, null);
                return;
            }

            var bundle = new Bundle();
            foreach (var param in parameters)
            {
                bundle.PutString(param.Key, param.Value);
            }

            firebaseAnalytics.LogEvent(eventId, bundle);
        }
    }
}