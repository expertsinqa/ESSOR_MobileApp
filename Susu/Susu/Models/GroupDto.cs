﻿using System;
namespace Susu.Models
{
    public class GroupDto
    {
        public long Id { get; set; }
        public string GroupName { get; set; }
        public int GroupNumber { get; set; }
        public long CreatorId { get; set; }
        public decimal ContributionAmount { get; set; }
        public string ContributionPeriod { get; set; }
        public DateTime? ContributionDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ContributionDay { get; set; }
        public string CustomRules { get; set; }
        public int status { get; set; }
        public DateTime? GroupStartDate { get; set; }
        public DateTime? PayOutDate { get; set; }
        public string PayOutDay { get; set; }
        public int? ErrorId { get; set; }
        public string ContributinDateString
        {
            get
            {
                if (ContributionDate != null)
                    return string.Format("{0:M/d/yyyy}", ContributionDate);
                else 
                    return "";
            }

        }

        public string PayOutDateString
        {
            get
            {
                if (PayOutDate != null)
                    return string.Format("{0:M/d/yyyy}", PayOutDate);
                else
                    return "";
            }

        }

        public string GroupStartDateString
        {
            get
            {
                if (GroupStartDate != null)
                    return string.Format("{0:M/d/yyyy}", GroupStartDate);
                else
                    return "";
            }
        }

        public string Contrbution_peroid
        {
            get
            {

                if (!string.IsNullOrEmpty(ContributionPeriod) && ContributionPeriod == "biweekly")
                {
                    return "Bi-Weekly";
                }
                else if (!string.IsNullOrEmpty(ContributionPeriod) && ContributionPeriod.ToLower() == "semimonthly")
                {
                    return "Semi-Monthly";
                }
                else if (!string.IsNullOrEmpty(ContributionPeriod) && ContributionPeriod.ToLower() == "semiyearly")
                {
                    return "Semi-Yearly";
                }
                else if (!string.IsNullOrEmpty(ContributionPeriod) && ContributionPeriod.ToLower() == "monthly")
                {
                    return "Monthly";
                }
                else if (!string.IsNullOrEmpty(ContributionPeriod) && ContributionPeriod.ToLower() == "weekly")
                {
                    return "Weekly";
                }
                else if (!string.IsNullOrEmpty(ContributionPeriod) && ContributionPeriod.ToLower() == "yearly")
                {
                    return "Yearly";
                }
                else
                {
                    return string.Empty;
                }
            }
            set { }
        }
    }
}
