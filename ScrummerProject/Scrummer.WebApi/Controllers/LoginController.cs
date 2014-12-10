
using Scrummer.Common.Model;
using Scrummer.Common.Helpers;
using Scrummer.Entity.Agent;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Net;
using System.IO;
using System.Web;
namespace Scrummer.WebApi.Controllers
{
    public class LoginController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _errorMessage = string.Empty;
      
       [HttpGet]
        public List<UserDetails> CheckAuthenticationofUserByEmailAndPassword(string emailId, string password)
        {
            try
            {
                return LoginEntity.CheckAuthenticationofUserByEmailAndPassword(emailId, password);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while fetching user roles."));
            }
        }

       [HttpPost]
       public  void RegisterUserDetails(string firstName, string lastName, string emailId, string password, string timeZone,string imageName)

       {
           try
           {

               LoginEntity.RegisterUserDetails(firstName, lastName, emailId, password,timeZone, imageName);
           }
           catch (Exception ex)
           {
               log.Error(ex.Message);
               throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while Registering the new user details."));
           }
       }
       [HttpGet]
       public  bool SendEmail(string email)
       {
           try
           {

              return LoginEntity.SendEmail(email);
           }
           catch (Exception ex)
           {
               log.Error(ex.Message);
               throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while sending mail to respect user."));
           }
       }

       [HttpGet]

       public void ResetPassword(string email, string password)
       {
           try
           {

                LoginEntity.ResetPassword(email, password);
           }
           catch (Exception ex)
           {
               log.Error(ex.Message);
               throw new HttpResponseException((new ExceptionHelpers()).ThrowExceptionMessage(ex, "Error occurred while reset the password to respect user."));
           }
       }

       
         
    }
}
