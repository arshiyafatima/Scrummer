using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrummer.Common.Model;
using Scrummer.Entity.Entity;
using Scrummer.Entity.Helper;
using System.Data;
using System.Globalization;
namespace Scrummer.Entity.Agent
{
   public class UserEntity
    {
       public static void UpdateUserScrumStatus(string email, int userScrumId, string scrumrYesterDayStatus, string scrumrToDayStatus, string blockers, string parkingLot)
       {
           
           using (ScrumEntities context = new ScrumEntities())
           {
               //context.UpdateUserScrumStatus(userId, scrumId, scrumerYesterDayStatus, scrumerToDayStatus, scrumerBlockerStatus);
               //context.SaveChanges();
               context.AddUserScrumStatus(email, userScrumId, scrumrYesterDayStatus, scrumrToDayStatus, blockers, parkingLot, scrumrYesterDayStatus.Length, scrumrToDayStatus.Length, blockers == null ? 0 : blockers.Length, parkingLot == null ? 0 : parkingLot.Length);
               context.SaveChanges();
           }
       }

       public static List<string> GetUserYesterdayStatusByUserScrumId(int scrumID,string email)
       {
           string imagePath;
           List<string> lstUserPreviousDayScrumStatusandImagePath = new List<string>();
           using (ScrumEntities context = new ScrumEntities())
           {
               int userID = context.UserDetails.Where(x => x.Email == email).Select(x => x.UserID).FirstOrDefault();
               int userScrumID = context.UserScrums.Where(x => x.ScrumID == scrumID && x.UserID == userID).Select(x=>x.UserScrumID).FirstOrDefault();
               byte[] userImageUrl = (from c in context.UserScrums
                                   join d in context.UserDetails on c.UserID equals d.UserID
                                      where c.UserScrumID == userScrumID
                                   select d.Image).FirstOrDefault();

               imagePath = userImageUrl == null ? "" : System.Text.Encoding.UTF8.GetString(userImageUrl);
               
               var lstUserPreviousDayScrumStatus = (from c in context.UserScrumDetailYesterdayTasks
                                                     where c.Date==((from d in context.UserScrumDetailYesterdayTasks
                                                                     where d.UserScrumID == userScrumID && d.Date < DateTime.Now
                                                        orderby d.Date descending
                                                                     select d.Date).FirstOrDefault()) && c.UserScrumID == userScrumID
                                                        select c.Task).ToList();
               lstUserPreviousDayScrumStatusandImagePath.Add(imagePath);
               lstUserPreviousDayScrumStatusandImagePath.AddRange(lstUserPreviousDayScrumStatus);
               return lstUserPreviousDayScrumStatusandImagePath;
           }
       }
       public static UserDetails ValidateUser(string emailId, string password)
       {
           using (ScrumEntities context = new ScrumEntities())
           {
               UserDetails user = (from c in context.GetUserDeatilsByEmailAndPassword(emailId, password, Constants.EncryptKey)
                                   select new UserDetails
                                   {
                                       UserID = c.UserID,
                                       FirstName = c.FirstName,
                                       LastName = c.LastName,
                                       Password = c.Password
                                   }).FirstOrDefault();
               return user;
           }
       }

    }
}
