using System;
using System.Collections.Generic;
using System.Text;

namespace Susu.Models
{
    public class UserDto
    {
        public long Id { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserPassword { get; set; }
        public string MobileNo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public byte[] ProofFileByte { get; set; }
        public string ProofFileName { get; set; }
        public string ProofFilePath { get; set; }
        public int Status { get; set; }
        public long? GroupId { get; set; }
        public string GroupName { get; set; }
        public int? RoleId { get; set; }
        public int? GroupNumber { get; set; }
        public bool IsAcceptAggrement { get; set; }
        public bool IsUpdateAggrement { get; set; }
        public string PayPalEmailId { get; set; }
        public int? UserOrderNo { get; set; }
        public string FullName
        {
            get
            {
                return FirstName +" "+ LastName;
            }
            set { }
        }
    }
}
