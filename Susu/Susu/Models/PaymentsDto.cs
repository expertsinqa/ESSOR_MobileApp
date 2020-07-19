using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ESORR.Models
{
    public class GroupContributionDetails
    {
        public long Id { get; set; }
        public int ContributionId { get; set; }
        public int GroupNumber { get; set; }
        public string ContributionPeriod { get; set; }
        public string ContributionDay { get; set; }
        public bool isNextContributionScheduled { get; set; }
        public bool isCompleted { get; set; }
        public DateTime ContributionDate { get; set; }
        public DateTime CreateDate { get; set; }
        public List<UserPayInDetails> LstPayInUsers { get; set; }
        public List<UserPayOutDetails> LstPayOutUsers { get; set; }

        public long GroupId { get; set; }
        public DateTime? NextContributionDate { get; set; }
    }
    public class UserPayInDetails
    {
        public long Id { get; set; }
        public int ContributionId { get; set; }
        public int GroupNumber { get; set; }
        public long UserId { get; set; }
        public bool isPaymentCompleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string UserName { get; set; }
        public bool isChecked { get; set; } = false;
        public bool IsSwitchEnabled { get; set; } = false;
        public string UserMail { get; set; }

        public ImageSource PaidImage
        {
            get
            {
                if (isPaymentCompleted)
                {
                    return "check_box.png";
                }
                else
                {
                    return "check_box_empty.png";
                }
            }
        }

        public string PaidStatus
        {
            get
            {
                if (isPaymentCompleted)
                {
                    return "Paid";
                }
                else
                {
                    return "Pending";
                }
            }
        }

    }

    public class UserPayOutDetails
    {
        public long Id { get; set; }
        public int ContributionId { get; set; }
        public int GroupNumber { get; set; }
        public long UserId { get; set; }
        public bool isPaymentCompleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string UserName { get; set; }
        public int UserOrderNo { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PayOutDate { get; set; }
        public DateTime ContributionDate { get; set; }
        public ImageSource PaidImage
        {
            get
            {
                if (isPaymentCompleted)
                {
                    return "check_box.png";
                }
                else
                {
                    return "check_box_empty.png";
                }
            }
        }

        public string ContributionDateString
        {
            get
            {
                if(PayOutDate!=null)
                {
                    return string.Format("{0:M/dd/yyyy}", PayOutDate);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

       public bool IsEnabled { get; set; }


    }

    public class UserPaymentDetails
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string CardType { get; set; }
        public long CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }
        public double PaidAmount { get; set; }
        public int GroupNumber { get; set; }
        public int ContributionId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
