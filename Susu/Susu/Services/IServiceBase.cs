using ESORR.Models;
using Susu.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susu.Services
{
    public  interface IServiceBase
    {
        void Initialize();
        Task<Dictionary<string,string>> Login(string username,string password);
        Task<UserDto> SaveUser(UserDto userdata);
        Task<GroupDto> SaveGroupInfo(GroupDto groupDto);
        Task<bool> SaveUserProofFile(byte[] proofFileBytes, string fileExtension, long userId);
        Task<UserDto> GetUserById(long userId);
        Task<GroupDto> JoinUser(string lstUserIds, int groupnumber,bool IsAcceptCustomRules);
        Task<GroupDto> GetGroupDetialsByGroupId(long? groupId);
        Task<bool> ForgotPassword(string usermail);
        Task<bool> ResetPassword(long userId, string oldpassword, string newpassword);
        Task<List<NotificationDto>> GetNotifications(int? notificationlevelId);
        Task<List<UserDto>> GetUsersByGroupId(long? groupId);
        Task<bool> SaveAndSendNotificationMail(List<EmailNotificatinDetailsDto> lstNotificationmailDetails);
        Task<List<EmailNotificatinDetailsDto>> GetUserNotificationsById(long userId);
        Task<bool> UpdateNotificationReadStatus(long userNotificationId);
        Task<bool> SwapUserOrder(long? requestFromUserId, long requestToUserId);
        Task<GroupContributionDetails> GetContributionDetailsByGroupNO(int groupNumber);
        Task<bool> SaveUserPayInDetails(List<UserPayInDetails> lstUserPayInDetails);
        Task<long> SavePayPalPaymentDetails(UserPaymentDetails paymentDetails);
        Task<List<UserPayInDetails>> GetPayInDetailByGroupNO(int groupNumber, int contributionId);

        Task<List<UserPayOutDetails>> GetPayOutDetailsByGroupNO(int groupNumber);
        Task<bool> SaveUserPayOutDetails(UserPayOutDetails payOutDetails);

        Task<SwapUserDetails> SwapUserDetailsById(long usernotificationId);

        Task<APPVersionDetails> GetAppVesrion();


    }
}
