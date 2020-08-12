using ESORR.Models;
using Newtonsoft.Json;
using Susu.Models;
using Susu.Services;
using Susu.Views;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

[assembly: Dependency(typeof(ServiceBase))]
namespace Susu.Services
{
    public class ServiceBase : IServiceBase
    {
        public void Initialize()
        {

        }
         //public string baseUrl = "http://167.86.126.236/ESORRAPI/api/";
          public string baseUrl = "https://www.esorr.com/ESORRAPI/api/";
        public HttpClient _client { get; set; }
        public ServiceBase()
        {
            _client = new HttpClient();
        }
        public async Task<Dictionary<string, string>> Login(string username, string password)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            try
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                            new KeyValuePair<string,string>("username",username),
                             new KeyValuePair<string, string>("password",password),
                             new KeyValuePair<string, string>("grant_type","password")
                });
                // public string baseUrl = "http://167.86.126.236/ESORRAPI/api/";
                 string url = "https://www.esorr.com/ESORRAPI/token";
               // string url = "http://167.86.126.236/ESORRAPI/token";
                var request = await _client.PostAsync(url, formcontent);
                request.EnsureSuccessStatusCode();
                var response = await request.Content.ReadAsStringAsync();
                if (response != null)
                {
                    Result = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                }
            }
            catch (Exception ex)
            {

            }
            return Result;
        }

        public async Task<UserDto> SaveUser(UserDto userdata)
        {
            UserDto userDto = new UserDto();
            try
            {
                string url = baseUrl + "user/saveuserdetails";
                var data = JsonConvert.SerializeObject(userdata);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                userDto = JsonConvert.DeserializeObject<UserDto>(result);
            }
            catch (Exception ex)
            {

            }
            return userDto;
        }

        public async Task<GroupDto> SaveGroupInfo(GroupDto groupDto)
        {
            GroupDto group = new GroupDto();
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/savegroupdetails";
                var data = JsonConvert.SerializeObject(groupDto);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                if (result != null)
                    group = JsonConvert.DeserializeObject<GroupDto>(result);
            }
            catch (Exception ex)
            {

            }
            return group;
        }

        public async Task<bool> SaveUserProofFile(byte[] proofFileBytes, string fileExtension, long userId)
        {
            bool isUploaded = false;
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/saveuserprooffile?proofFileBytes=" + proofFileBytes + "&fileExtension=" + fileExtension + "&userId=" + userId;
                var data = JsonConvert.SerializeObject("");
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                if (result != null)
                    isUploaded = JsonConvert.DeserializeObject<bool>(result);
            }
            catch (Exception ex)
            {

            }
            return isUploaded;
        }

        public async Task<UserDto> GetUserById(long userId)
        {
            UserDto userDto = null;
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/getuserbyid?userId=" + userId;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    userDto = JsonConvert.DeserializeObject<UserDto>(res);
                }
            }
            catch (Exception ex)
            {

            }
            return userDto;
        }

        public async Task<GroupDto> JoinUser(string lstUserIds, int groupnumber, bool IsAcceptCustomRules)
        {
            GroupDto groupDto = new GroupDto();
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/saveusergroupdetails?lstUserIds=" + lstUserIds + "&groupnumber=" + groupnumber+ "&IsAcceptCustomRules="+IsAcceptCustomRules;
                var data = JsonConvert.SerializeObject("");
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                groupDto = JsonConvert.DeserializeObject<GroupDto>(result);
            }
            catch (Exception ex)
            {

            }
            return groupDto;
        }


        public async Task<GroupDto> GetGroupDetialsByGroupId(long? groupId)
        {
            GroupDto groupDto = null;
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                groupDto = new GroupDto();
                string url = baseUrl + "user/getgroupdetailsbyid?groupId=" + groupId;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    groupDto = JsonConvert.DeserializeObject<GroupDto>(res);
                }
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("401"))
                {
                    App.Current.MainPage = new LoginPage();
                }
            }
            return groupDto;
        }

        public async Task<bool> ForgotPassword(string usermail)
        {
            bool isResetPasswordSuccess = false;
            try
            {
                string url = baseUrl + "user/resetpassword?usermail=" + usermail;
                var response = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (response != null)
                {
                    isResetPasswordSuccess = JsonConvert.DeserializeObject<bool>(response);
                }
            }
            catch (Exception ex)
            {

            }
            return isResetPasswordSuccess;
        }

        public async Task<bool> ResetPassword(long userId, string oldpassword, string newpassword)
        {
            bool isPswrdReset = false;
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/changepassword?userId=" + userId + "&oldpassword=" + oldpassword + "&newpassword=" + newpassword;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    isPswrdReset = JsonConvert.DeserializeObject<bool>(res);
                }
            }
            catch (Exception ex)
            { }
            return isPswrdReset;
        }

        public async Task<List<NotificationDto>> GetNotifications(int? notificationlevelId)
        {
            List<NotificationDto> lstnotificationDto = new List<NotificationDto>();
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "notification/getnotifications?notificationlevelId="+notificationlevelId;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    lstnotificationDto = JsonConvert.DeserializeObject<List<NotificationDto>>(res);
                }
            }
            catch (Exception ex)
            {

            }
            return lstnotificationDto;
        }

        public async Task<List<UserDto>> GetUsersByGroupId(long? groupId)
        {
            List<UserDto> lstUserDto = new List<UserDto>();
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/getusersbygroupId?groupId=" + groupId;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    lstUserDto = JsonConvert.DeserializeObject<List<UserDto>>(res);
                }
            }
            catch (Exception ex)
            {

            }
            return lstUserDto;
        }

        public async Task<bool> SaveAndSendNotificationMail(List<EmailNotificatinDetailsDto> lstNotificationmailDetails)
        {
            bool isSuccess = false;
            try {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "notification/saveandsendnotificationmail";
                var data = JsonConvert.SerializeObject(lstNotificationmailDetails);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                if (result != null)
                    isSuccess = JsonConvert.DeserializeObject<bool>(result);
            }
            catch(Exception ex)
            {

            }
            return isSuccess;
        }

        public async Task<List<EmailNotificatinDetailsDto>> GetUserNotificationsById(long userId)
        {
            List<EmailNotificatinDetailsDto> lstEmailNotificationDto = new List<EmailNotificatinDetailsDto>();
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "notification/getusernotificationsbyId?userId=" + userId;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    lstEmailNotificationDto = JsonConvert.DeserializeObject<List<EmailNotificatinDetailsDto>>(res);
                }
            }
            catch (Exception ex)
            {

            }
            return lstEmailNotificationDto;
        }

        public async Task<bool> UpdateNotificationReadStatus(long userNotificationId)
        {
            bool isSuccess = false;
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "notification/updatenotificationreadstatus?userNotificationId="+userNotificationId;
                var data = JsonConvert.SerializeObject("");
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                if (result != null)
                    isSuccess = JsonConvert.DeserializeObject<bool>(result);
            }
            catch (Exception ex)
            {

            }
            return isSuccess;
        }
        public async Task<bool> SwapUserOrder(long? requestFromUserId, long requestToUserId)
        {
            bool isSuccess = false;
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/swapuserorder?requestFromUserId=" + requestFromUserId+ "&requestToUserId="+requestToUserId;
                var data = JsonConvert.SerializeObject("");
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                if (result != null)
                    isSuccess = JsonConvert.DeserializeObject<bool>(result);
            }
            catch (Exception ex)
            {

            }
            return isSuccess;
        }

        public async Task<GroupContributionDetails> GetContributionDetailsByGroupNO(int groupNumber)
        {
            GroupContributionDetails groupContributionDetails = new GroupContributionDetails();
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/getcontributiondetailsbygroupno?groupNumber=" + groupNumber;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    groupContributionDetails = JsonConvert.DeserializeObject<GroupContributionDetails>(res);
                }
            }
            catch (Exception ex)
            {

            }
            return groupContributionDetails;
        }

        public async Task<bool> SaveUserPayInDetails(List<UserPayInDetails> lstUserPayInDetails)
        {
            bool isSuccess = false;
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/saveuserpayIndetails";
                var data = JsonConvert.SerializeObject(lstUserPayInDetails);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                if (result != null)
                    isSuccess = JsonConvert.DeserializeObject<bool>(result);
            }
            catch (Exception ex)
            {

            }
            return isSuccess;
        }

        public async Task<long> SavePayPalPaymentDetails(UserPaymentDetails paymentDetails)
        {
            long paymentId = 0;
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/savepaypalpaymentdetails";
                var data = JsonConvert.SerializeObject(paymentDetails);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                if (result != null)
                    paymentId = JsonConvert.DeserializeObject<long>(result);
            }
            catch (Exception ex)
            {

            }
            return paymentId;
        }

        public async Task<List<UserPayInDetails>> GetPayInDetailByGroupNO(int groupNumber, int contributionId)
        {
            List<UserPayInDetails> lstUserPayInDetails = new List<UserPayInDetails>();
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/getpayInDetailsbygroupno?groupNumber=" + groupNumber+ "&contributionId="+contributionId;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    lstUserPayInDetails = JsonConvert.DeserializeObject<List<UserPayInDetails>>(res);
                }
            }
            catch (Exception ex)
            {

            }
            return lstUserPayInDetails;
        }

        public async Task<List<UserPayOutDetails>> GetPayOutDetailsByGroupNO(int groupNumber)
        {
            List<UserPayOutDetails> lstUserOutDetails = new List<UserPayOutDetails>();
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/getpayOutDetailsbygroupno?groupNumber=" + groupNumber;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    lstUserOutDetails = JsonConvert.DeserializeObject<List<UserPayOutDetails>>(res);
                }
            }
            catch (Exception ex)
            {

            }
            return lstUserOutDetails;
        }

        public async Task<bool> SaveUserPayOutDetails(UserPayOutDetails payOutDetails)
        {
            bool isSuccess = false;
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/saveuserpayOutdetails";
                var data = JsonConvert.SerializeObject(payOutDetails);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync(url, content);
                var result = await res.Content.ReadAsStringAsync();
                if (result != null)
                    isSuccess = JsonConvert.DeserializeObject<bool>(result);
            }
            catch (Exception ex)
            {

            }
            return isSuccess;
        }
        public async Task<SwapUserDetails> SwapUserDetailsById(long usernotificationId)
        {
            SwapUserDetails swapUserDetails = new SwapUserDetails();
            try
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                if (App.AccessToken != null)
                    _client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.AccessToken);
                string url = baseUrl + "user/swapuserdetailsbyId?usernotificationId=" + usernotificationId;
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    swapUserDetails = JsonConvert.DeserializeObject<SwapUserDetails>(res);
                }
            }
            catch (Exception ex)
            {

            }
            return swapUserDetails;
        }

        public async Task<APPVersionDetails> GetAppVesrion()
        {
            APPVersionDetails appVersionDetails = null;
            try
            {
                string url = baseUrl + "user/getappversiondetails";
                var res = await _client.GetStringAsync(url).ConfigureAwait(false);
                if (res != null)
                {
                    appVersionDetails = JsonConvert.DeserializeObject<APPVersionDetails>(res);
                }
                return appVersionDetails;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

    }
}
