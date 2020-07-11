using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Susu.Models
{
    public class NotificationDto
    {
        public long Id { get; set; }
        public string Tittle { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public int NotificationType { get; set; }
        public int NotificationLevel { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

    }


    public class EmailNotificatinDetailsDto : SwapUserRequestLevel
    {
        public long Id { get; set; }
        public long NotificationId { get; set; }
        public long UserId { get; set; }
        public string UserMail { get; set; }
        public string NotificationMessage { get; set; }
        public string EmailBody { get; set; }
        public bool isSentMail { get; set; }
        public string mailSubject { get; set; }
        public DateTime CreateDate { get; set; }
        public bool isReadbyUser { get; set; }
        public int NotificationType { get; set; }
        public int NotificationLevel { get; set; }
        public long? FromUserId { get; set; }
        public long? ToUserId { get; set; }
        public int? GroupNumber { get; set; }
        public int? contributionId { get; set; }

        public bool? Isupdateswap { get; set; }

        public bool? IsOrderRequestUserToUser { get; set; }

        public Color BGcolor
        {
            get
            {
                if (isReadbyUser)
                    return Color.White;
                else
                    return Color.FromHex("#2d67e4");
            }
            set { }
        }

        public Color TextColor
        {
            get
            {
                if (isReadbyUser)
                    return Color.Black;
                else
                    return Color.White;
            }
            set { }
        }

    }

    public class SwapUserRequestLevel
    {
        public long RequestId { get; set; }
        public int LevelId { get; set; }
        public bool IsCompleted { set; get; }
    }
    public class SwapUserDetails : SwapUserRequestLevel
    {
        public long Id { get; set; }
        public long EmailNotificationID { get; set; }
        public long RequestFromId { get; set; }
        public long RequestToId { get; set; }
        public long ApproverId { get; set; }
        public bool IsUserAccept { get; set; }
        public bool IsUserReject { get; set; }
        public bool IsAdminApprove { get; set; }
        public bool IsAdminReject { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsOrderRequestUserToUser { get; set; }

    }
}
