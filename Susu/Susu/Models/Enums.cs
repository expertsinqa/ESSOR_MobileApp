using System;
using System.Collections.Generic;
using System.Text;

namespace Susu.Models
{
    public class Enums
    {
        public enum Roles
        {
            undefined = 0,
            rootadmin = 1,
            groupadmin = 2,
            user = 3
        }
        public enum NotificationLevel
        {
            AllUsers = 1,
            SpecificUser = 2,
        }

        public enum NotificationType
        {
            ThreeDayPaymentReminder = 1,
            OneDayPaymentReminder = 2,
            PayInPaymentSuccess = 3,
            GroupCreationSuccess = 4,
            WelcomeForGroupJoin = 5,
            RequestToChangeOrder = 6,
            UserAcceptChangeOrder = 7,
            UserRejectChangeOrder = 8,
            AdminAcceptChangeOrder = 9,
            AdminRejectChangeOrder = 10,
            PayOutPaymentSuccess = 11,
            ContributionPaymentReminder=12
        }

    }
}
