using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scrummer.Common.Model;
using Scrummer.Common.Helpers;
using Scrummer.Entity.Agent;
using System.Web.Http;
using System.Net;
namespace Scrummer.WebApi.Controllers
{
    public class UserController : ApiController
    {
        //
        // GET: /UserScrumerDetails/

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _errorMessage = string.Empty;

        [HttpPost]
        public void UpdateUserScrumStatus(string email, int userScrumId, string scrumrYesterDayStatus, string scrumrToDayStatus, string blockers, string parkingLot)
        {
            try
            {
                UserEntity.UpdateUserScrumStatus(email, userScrumId, scrumrYesterDayStatus, scrumrToDayStatus, blockers, parkingLot);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while updating user scrum status."));
            }
        }

        [HttpGet]
        public List<string> GetUserYesterdayStatusByUserScrumId(int scrumID, string email)
        {
            try
            {
                return UserEntity.GetUserYesterdayStatusByUserScrumId(scrumID, email);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching user yesterday status."));
            }
        }

        [HttpGet]
        public UserDetails ValidateUser(string emailId, string password)
        {
            try
            {

                return UserEntity.ValidateUser(WebUtility.UrlDecode(emailId), password);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while reset the password to respect user."));
            }
        }
    }
}
