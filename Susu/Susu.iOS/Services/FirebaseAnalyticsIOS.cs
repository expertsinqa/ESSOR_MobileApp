﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESORR.Interface;
using Firebase.Analytics;
using Foundation;
using Susu.iOS.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseAnalyticsIOS))]
namespace Susu.iOS.Services
{
    public class FirebaseAnalyticsIOS: IFirebaseAnalytics
    {
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, (IDictionary<string, string>)null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                { paramName, value }
            });
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                Analytics.LogEvent(eventId, (Dictionary<object, object>)null);
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters)
            {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary =
                NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            Analytics.LogEvent(eventId, parametersDictionary);
        }
    }
}
