using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scrummer.Common.Model;
using Scrummer.Common.Helpers;
using Scrummer.Entity.Agent;
using System.Web.Http;
using System.Net;
using System.Data;
using System.Globalization;

namespace Scrummer.WebApi.Controllers
{
    public class ScrumController : ApiController
    {
        //
        // GET: /ScrumrView/

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _errorMessage = string.Empty;

        [HttpGet]
        public DataTable UsersScrumDetailsByDate(string displayDate, int scrumID)
        {
            try
            {
                DateTime dispDt = new DateTime();

                dispDt = DateTime.ParseExact(displayDate.Substring(0, displayDate.Length), "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                return ScrumEntity.UsersScrumDetailsByDate(dispDt, scrumID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching user roles."));
            }
        }

        [HttpGet]
        public List<ScrumHistroy> GetScrumrHistroyStatus(string selectedDate)
        {
            try
            {

                return ScrumEntity.GetScrumrHistroyStatus(selectedDate);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching user roles."));
            }
        }

        [HttpGet]
        public IEnumerable<string> BindPreviousStartingWeekDates(int scrum)
        {

            try
            {
                return ScrumEntity.BindPreviousStartingWeekDates();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching user roles."));
            }
        }

        [HttpGet]
        public List<string> DisplayAllScrumUsersByScrumID(int userScrumID)
        {
            try
            {
                return ScrumEntity.DisplayAllScrumUsersByScrumID(userScrumID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching scrum users."));
            }
        }

        [HttpGet]
        public List<UserScrumDetails> ExportScrumStatusCSVExport(string fromDate, string toDate, int scrumID)
        {
            try
            {
                DateTime fromDt = new DateTime();
                DateTime toDt = new DateTime();

                fromDt = DateTime.ParseExact(fromDate.Substring(0, 10), "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                toDt = DateTime.ParseExact(toDate.Substring(0, 10), "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                return ScrumEntity.ExportScrumStatusCSVExport(fromDt, toDt, scrumID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while Exporting Scrum Status."));
            }
        }

        [HttpGet]
        public List<ScrumDetails> GetAllScrumDetails()
        {
            try
            {
                return ScrumEntity.GetAllScrumDetails();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching Scrum Details."));
            }
        }

        [HttpGet]
        public IEnumerable<string> GetAllUserNames(string email,int scrumID, bool isautocomplete)
        {
            try
            {
                return ScrumEntity.GetAllUserNames(email, scrumID, isautocomplete);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching user names."));
            }

        }

        [HttpGet]

        public int CheckThisUserNameAndScrumIDPresentOrNot(string userName)
        {
            try
            {
                return ScrumEntity.CheckThisUserNameAndScrumIDPresentOrNot(userName);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while checking User Name and Scrum ID already exists or not."));
            }
        }

        [HttpPost]

        public void AddNewUsersOrRemoveUsersForScrum(string users, string removeUsers, int scrumID)
        {

            try
            {
                ScrumEntity.AddNewUsersOrRemoveUsersForScrum(users, removeUsers, scrumID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while adding new users or removing users for this scrum"));
            }
        }

        [HttpPost]

        public void UpdateScrumDetails(string email,int scrumID, string scrumName, string description, string daysOfOccurence, string dailyOccursTime, string remainder,string timeZone,int isActive, string users, string removeUsers)
        {

            try
            {
                ScrumEntity.UpdateScrumDetails(email, scrumID, scrumName, description, daysOfOccurence, dailyOccursTime, remainder, timeZone, isActive, users, removeUsers);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while adding new users or removing users for this scrum"));
            }
        }

        [HttpGet]
        public List<ScrumInformation> GetAllScrums(string email)
        {
            try
            {
                return ScrumEntity.GetAllScrums(email);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching all scrum information"));
            }
        }

        [HttpGet]
        public List<ScrumDetails> GetAllScrumInformationByScrumID(int scrumID)
        {
            try
            {
                return ScrumEntity.GetAllScrumInformationByScrumID(scrumID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching scrum information"));
            }
        }

        [HttpPost]
        public void DeleteScrum(int scrumID)
        {
            try
            {
                ScrumEntity.DeleteScrum(scrumID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while deleting the scrum"));
            }
        }

        [HttpGet]
        public bool CheckIsUserUpdateStatusOrNot(string email, int scrumID)
        {
            try
            {
                return ScrumEntity.CheckIsUserUpdateStatusOrNot(email, scrumID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while checking the user status exists today or not"));
            }
        }

       
    }
}
