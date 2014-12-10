using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrummer.Common.Model;
using Scrummer.Entity.Entity;
using Scrummer.Entity.Helper;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using System.Net;
using System.Configuration;

using System.Net.Mail;
namespace Scrummer.Entity.Agent
{
    public class LoginEntity
    {
        public static List<UserDetails> CheckAuthenticationofUserByEmailAndPassword(string emailId, string password)
        {
            using (ScrumEntities context = new ScrumEntities())
            {
               
                var lstUserDetails = (from c in context.GetUserDeatilsByEmailAndPassword(emailId, password, Constants.EncryptKey)
                              select new UserDetails
                              {
                                  UserID = c.UserID,
                                  FirstName=c.FirstName,
                                  LastName=c.LastName,
                                  IsStatusUpdated=false
                              }).ToList();
                if (lstUserDetails != null)
                {
                    DateTime date = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                    foreach (var userDetails in lstUserDetails)
                    {
                        userDetails.IsStatusUpdated = (context.UserScrumDetailYesterdayTasks.Where(x => x.UserScrumID == userDetails.UserScrumID && x.Date == date).Select(x => x.Task).Count()) > 0 ? true : false;

                    }
                }
                return lstUserDetails;
            }
        }

        public static void RegisterUserDetails(string firstName, string lastName, string emailId, string password,string timeZone, string imageName)
       {
            using (ScrumEntities context = new ScrumEntities())
            {

                byte[] imageBytes = Encoding.ASCII.GetBytes(imageName);
                ////byte[] imageData = File.ReadAllBytes(@"C:\Users\Public\Pictures\Sample Pictures\Lighthouse.jpg");
                //byte[] imageData = File.ReadAllBytes(dirs[0]);
                context.RegisterNewUserDetails(firstName, lastName, emailId, password,timeZone, Constants.EncryptKey, imageBytes);
                context.SaveChanges();
            }
        }
        public static bool SendEmail(string email)
        {
            string firstName = string.Empty;
            string subject = "Password Reset Instruction";
            string emailBody = string.Empty;
            using (ScrumEntities context = new ScrumEntities())
            {
                firstName = context.UserDetails.Where(x => x.Email == email).Select(x => x.FirstName).FirstOrDefault();
            }
            if (firstName != null)
            {
                emailBody = "Hello" + ' ' + firstName + ',' + Environment.NewLine + Environment.NewLine + "Please follow this " + ConfigurationManager.AppSettings["ResetPasswordUrl"] + email + " to reset your password";
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("arun.sekar@saggezza.com", "arusek123"),
                    EnableSsl = true
                };
                client.Send("arun.sekar@saggezza.com", email, subject, emailBody);
                return true;
            }
            else
                return false;

        }

        public static void ResetPassword(string email, string password)
        {
            using (ScrumEntities context = new ScrumEntities())
            {
                context.ResetPassword(email, password,Constants.EncryptKey);
                context.SaveChanges();
            }
        }

       
    }
}
