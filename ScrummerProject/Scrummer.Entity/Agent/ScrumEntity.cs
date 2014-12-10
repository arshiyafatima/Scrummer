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
using System.Net.Mail;
using System.Net;

namespace Scrummer.Entity.Agent
{
    public class ScrumEntity
    {
        public static DataTable UsersScrumDetailsByDate(DateTime? displayDate,int scrumID)
        {
            using (ScrumEntities context = new ScrumEntities())
            {


                //var lstUserScrumDetails = (from c in context.UserScrumDetails
                //                           join d in context.UserDetails on c.UserID equals d.UserID
                //                           where c.Date.Equals(displayDate)
                //                           select new UserScrumDetails
                //                           {
                //                               Name=d.FirstName+" "+d.LastName,
                //                               YesterdayStatus=c.ScrumYesterdayText,
                //                               TodayStatus=c.ScrumTodayText,
                //                               Blockers=c.Blockers
                //                           }).Distinct().ToList();

                var lstUserScrumDetails = (from c in context.UserScrums
                                           join d in context.UserScrumDetailYesterdayTasks on c.UserScrumID equals d.UserScrumID
                                           join e in context.UserDetails on c.UserID equals e.UserID
                                           where d.Date == displayDate && c.ScrumID == scrumID
                                           select new UserDetails
                                           {
                                               UserName = e.FirstName + " " + e.LastName,
                                               UserScrumID = c.UserScrumID,
                                               ImageName = e.Image
                                           }).Distinct().ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("UserName", typeof(string));
                dt.Columns.Add("YesterdayTask", typeof(string));
                dt.Columns.Add("TodayTask", typeof(string));
                dt.Columns.Add("Blockers", typeof(string));
                dt.Columns.Add("ParkingLot", typeof(string));
                dt.Columns.Add("ImageName", typeof(string));
                foreach (var UserScrumDetails in lstUserScrumDetails)
                {
                    string yesterdayTask = string.Empty;
                    string todayTask = string.Empty;
                    string blockers = string.Empty;
                    string parkingLot = string.Empty;
                    DataRow dr = dt.NewRow();
                    dr["UserName"] = UserScrumDetails.UserName;
                    dr["ImageName"] = (UserScrumDetails.ImageName == null ? "" : System.Text.Encoding.UTF8.GetString(UserScrumDetails.ImageName));
                    var lstYesterdayTask = (from c in context.UserScrumDetailYesterdayTasks
                                            where c.UserScrumID == UserScrumDetails.UserScrumID && c.Date == displayDate
                                            select new { Task = c.Task }).ToList();
                    foreach (var userYesterdayTask in lstYesterdayTask)
                    {
                        yesterdayTask = yesterdayTask + userYesterdayTask.Task + "</br></br>";
                    }
                    yesterdayTask = yesterdayTask.Substring(0, yesterdayTask.Length - 10);
                    dr["YesterdayTask"] = yesterdayTask;
                    var lstTodayTask = (from c in context.UserScrumDetailTodayTasks
                                        where c.UserScrumID == UserScrumDetails.UserScrumID && c.Date == displayDate
                                        select new { Task = c.Task }).ToList();
                    foreach (var userTodayTask in lstTodayTask)
                    {
                        todayTask = todayTask + userTodayTask.Task + "</br></br>";
                    }
                    todayTask = todayTask.Substring(0, todayTask.Length - 10);
                    dr["TodayTask"] = todayTask;
                    var lstBlockers = (from c in context.UserScrumDetailBlockers
                                       where c.UserScrumID == UserScrumDetails.UserScrumID && c.Date == displayDate
                                       select new { Blocker = c.Blocker }).ToList();
                    foreach (var userBlockers in lstBlockers)
                    {
                        blockers = blockers + userBlockers.Blocker + "</br></br>";
                    }
                    if (blockers != "")
                        blockers = blockers.Substring(0, blockers.Length - 10);
                    dr["Blockers"] = blockers;

                    var lstParkingLot = (from c in context.UserScrumDetailParkingLots
                                       where c.UserScrumID == UserScrumDetails.UserScrumID && c.Date == displayDate
                                         select new { ParkingLot = c.ParkingLot }).ToList();
                    foreach (var parkingLots in lstParkingLot)
                    {
                        parkingLot = parkingLot + parkingLots.ParkingLot + "</br></br>";
                    }
                    if (parkingLot != "")
                        parkingLot = parkingLot.Substring(0, parkingLot.Length - 10);
                    dr["ParkingLot"] = parkingLot;
                    dt.Rows.Add(dr);
                }

                return dt;

            }
        }

        public static List<ScrumHistroy> GetScrumrHistroyStatus(string selectedDate)
        {
            List<ScrumHistroy> lstScrumrHistroyDetails = new List<ScrumHistroy>();
            string datetime;
            using (ScrumEntities context = new ScrumEntities())
            {
                if (selectedDate == null)
                {
                    DateTime date = DateTime.Now;
                    datetime = date.ToString("ddd dd/MM/yyyy");
                    while (datetime.Substring(0, 3).ToLower() != "sun")
                    {

                        DateTime dt = DateTime.ParseExact(date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        lstScrumrHistroyDetails.Add(new ScrumHistroy() { HistroyDate = datetime, Count = context.UserScrumDetailYesterdayTasks.Where(i => i.Date == dt).Select(i => i.UserScrumID).Distinct().Count() });
                        date = date.AddDays(-1);
                        datetime = date.ToString("ddd dd/MM/yyyy");

                    }
                }
                else
                {
                    DateTime date = DateTime.ParseExact(selectedDate.Substring(0, 10), "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                    date = date.AddDays(4);
                    datetime = date.ToString("ddd dd/MM/yyyy");
                    while (datetime.Substring(0, 3).ToLower() != "sun")
                    {
                        
                        DateTime dt = DateTime.ParseExact(date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                        lstScrumrHistroyDetails.Add(new ScrumHistroy() { HistroyDate = datetime, Count = context.UserScrumDetailYesterdayTasks.Where(i => i.Date == dt).Select(i => i.UserScrumID).Distinct().Count() });
                        date = date.AddDays(-1);
                        datetime = date.ToString("ddd dd/MM/yyyy");

                    }

                }

                return lstScrumrHistroyDetails;
            }

        }

        public static List<string> BindPreviousStartingWeekDates()
        {
            List<string> lstDates = new List<string>();
            DateTime dt = DateTime.Now;
            while (dt.DayOfWeek != DayOfWeek.Monday)
            {
                dt = dt.AddDays(-1);
            }
            lstDates.Add(dt.ToString("ddd dd/MM/yyyy"));
            lstDates.Add(dt.ToString("dd/MM/yyyy"));
            for (int week = 1; week <= 24; week++)
            {
                dt = dt.AddDays(-7);
                lstDates.Add(dt.ToString("ddd dd/MM/yyyy"));
                lstDates.Add(dt.ToString("dd/MM/yyyy"));
            }
            return lstDates;
        }

        public static List<string> DisplayAllScrumUsersByScrumID(int userScrumID)
        {
            List<string> lstScrumUsers = new List<string>();
            using (ScrumEntities context = new ScrumEntities())
            {
                var ScrumUsers = (from c in context.GetAllScrumUsersByScrumID(userScrumID)
                                  select new UserDetails
                                  {
                                      UserName = c.UserName,
                                      ImageName = c.Image
                                  }).ToList();

               
                foreach (var scrumusers in ScrumUsers)
                {
                    lstScrumUsers.Add(scrumusers.UserName);
                    lstScrumUsers.Add(scrumusers.ImageName == null ? "" : System.Text.Encoding.UTF8.GetString(scrumusers.ImageName));
                }
                return lstScrumUsers;
            }
        }

        public static List<UserScrumDetails> ExportScrumStatusCSVExport(DateTime fromDt, DateTime toDt, int scrumID)
        {
            //DateTime date = DateTime.ParseExact(exportDate.Substring(0, 10), "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            //DateTime dt = date;
            //int a=1;
           List<UserScrumDetails> lstUserScrumDetails=new List<UserScrumDetails>();
           using (ScrumEntities context = new ScrumEntities())
           {
               int count = 0;
               while (fromDt <= toDt)
               {
                   string displayDate = fromDt.ToString("dd/MM/yyyy");
                 
                   var lstUserDetails = (from c in context.UserScrums
                                         join d in context.UserScrumDetailYesterdayTasks on c.UserScrumID equals d.UserScrumID
                                         join e in context.UserDetails on c.UserID equals e.UserID
                                         where d.Date == fromDt && c.ScrumID == scrumID
                                         select new
                                         {
                                             UserScrumID = c.UserScrumID,
                                             UserName = e.FirstName + " " + e.LastName,
                                         }).Distinct().ToList();
                   if (lstUserDetails.Count>0)
                   {
                       if (count > 0)
                           lstUserScrumDetails.Add(new UserScrumDetails() { Date = "", UserName = "", YesterdayStatus = "", TodayStatus = "", Blockers = "" });
                       foreach (var userdetails in lstUserDetails)
                       {
                           if (count > 0)
                               lstUserScrumDetails.Add(new UserScrumDetails() { Date = "", UserName = "", YesterdayStatus = "", TodayStatus = "", Blockers = "" });
                           count = count + 1;
                           int yesterdayStatusCount = 0;
                           int todayStatusCount = 0;
                           int blockersCount = 0;
                           var lstYesterdayTask = (from c in context.UserScrumDetailYesterdayTasks
                                                   where c.UserScrumID == userdetails.UserScrumID && c.Date == fromDt
                                                   select new { Task = c.Task }).ToList();

                           var lstTodayTask = (from c in context.UserScrumDetailTodayTasks
                                               where c.UserScrumID == userdetails.UserScrumID && c.Date == fromDt
                                               select new { Task = c.Task }).ToList();
                           var lstBlockers = (from c in context.UserScrumDetailBlockers
                                              where c.UserScrumID == userdetails.UserScrumID && c.Date == fromDt
                                              select new { Blocker = c.Blocker }).ToList();

                           int bigStatusCount = lstYesterdayTask.Count() > lstTodayTask.Count() ? (lstYesterdayTask.Count() > lstBlockers.Count() ? lstYesterdayTask.Count() : lstBlockers.Count()) : (lstTodayTask.Count() > lstBlockers.Count() ? lstTodayTask.Count() : lstBlockers.Count());
                           string[] yesterdayStatus = new string[bigStatusCount];
                           string[] todayStatus = new string[bigStatusCount];
                           string[] blockers = new string[bigStatusCount];
                           foreach (var yesterdayTask in lstYesterdayTask)
                           {
                               yesterdayStatus[yesterdayStatusCount++] = yesterdayTask.Task;
                           }
                           while (bigStatusCount > yesterdayStatusCount)
                           {
                               yesterdayStatus[yesterdayStatusCount++] = "";
                           }

                           foreach (var todayTask in lstTodayTask)
                           {
                               todayStatus[todayStatusCount++] = todayTask.Task;
                           }
                           while (bigStatusCount > todayStatusCount)
                           {
                               todayStatus[todayStatusCount++] = "";
                           }

                           foreach (var blocker in lstBlockers)
                           {
                               blockers[blockersCount++] = blocker.Blocker;
                           }
                           while (bigStatusCount > blockersCount)
                           {
                               blockers[blockersCount++] = "";
                           }

                           for (int i = 0; i < bigStatusCount; i++)
                           {
                               //if (i == 0)
                                   lstUserScrumDetails.Add(new UserScrumDetails() { Date = displayDate, UserName = userdetails.UserName, YesterdayStatus = yesterdayStatus[i], TodayStatus = todayStatus[i], Blockers = blockers[i] });
                               //else
                               //    lstUserScrumDetails.Add(new UserScrumDetails() { Date = "", UserName = "", YesterdayStatus = yesterdayStatus[i], TodayStatus = todayStatus[i], Blockers = blockers[i] });
                           }

                       }
                   }
                  
                   fromDt = fromDt.AddDays(1);
               }
               return lstUserScrumDetails;
           }
                 
        }


        public static List<ScrumDetails> GetAllScrumDetails()
        {
            using (ScrumEntities context = new ScrumEntities())
            {
                var lstScrumDetails = (from c in context.Scrums
                                       select new ScrumDetails
                                       {
                                           ScrumID=c.ScrumID,
                                           ScrumName=c.Name
                                       }).Distinct().ToList();
                return lstScrumDetails;
            }
        }

        public static IEnumerable<string> GetAllUserNames(string email,int scrumID, bool isautocomplete)
        {

            using (ScrumEntities context = new ScrumEntities())
            {
                //var lstUserNames = (from c in context.Scrums
                //                    join d in context.UserScrums on c.ScrumID equals d.ScrumID
                //                    join e in context.UserDetails on d.UserID equals e.UserID
                //                    where c.ScrumID == scrumID
                //                    select e.FirstName + ' ' + e.LastName).Distinct().ToList();

                var lstUserNames = (from c in context.GetAllUserNames(email, scrumID, isautocomplete)
                                    select c.FirstName + ' ' + c.LastName).ToList();
                return lstUserNames;
            }
        }

        public static  int CheckThisUserNameAndScrumIDPresentOrNot(string userName)
        {
            string[] name = new string[2];
            name = userName.Split(' ');
            string firstName = name[0];
            string lastName = name.Length == 1 ? "" : name[1];
            int? userId=0;
            
            using (ScrumEntities context = new ScrumEntities())
            {
                userId = context.UserDetails.Where(x => x.FirstName == firstName && x.LastName == lastName).Select(x => x.UserID).FirstOrDefault();
             if (userId != 0)
                     return 1;
                 else
                     return 0;
            }

        }

        public static void AddNewUsersOrRemoveUsersForScrum(string users,string removeUsers, int scrumID)
        {
            using (ScrumEntities context = new ScrumEntities())
            {
                //context.AddUsersForScrum(users, removeUsers, scrumID, users == null ? 0 : users.Length, removeUsers == null ? 0 : removeUsers.Length);
                //context.SaveChanges();

            }
        }

        public static void UpdateScrumDetails(string email,int scrumID, string scrumName, string description, string daysOfOccurence, string dailyOccursTime, string remainder,string timeZone,int isActive, string users, string removeUsers)
        {
            using (ScrumEntities context = new ScrumEntities())
            {
                context.UpdateScrumDetails(email, scrumID, scrumName, description, daysOfOccurence, dailyOccursTime, remainder, timeZone, isActive, users, removeUsers, users == null ? 0 : users.Length, removeUsers == null ? 0 : removeUsers.Length);
                context.SaveChanges();
            }
        }

         public static List<ScrumInformation> GetAllScrums(string email)
         {
             
             using (ScrumEntities context = new ScrumEntities())
             {
                 UserDetail user = context.UserDetails.Single(i => i.Email == email);
                 //isScrumMaster = (context.UserRoles.Where(x => x.UserID == userID).Select(x => x.RoleID).FirstOrDefault() == 1 ? true : false);
                 //if (isScrumMaster == true)
                 //{
                     var lstScrumDetails = (from c in context.Scrums
                                            where c.IsActive==true
                                            select new ScrumInformation
                                            {
                                                ScrumName = c.Name,
                                                ScrumDescription = c.Description,
                                                ScrumID = c.ScrumID,
                                                IsScrumMaster=(context.Scrums.Where(x => x.ScrumID == c.ScrumID&&x.CreatedBy==user.UserID).Select(x=>x.ScrumID).FirstOrDefault()>0 ? true : false),
                                                ScrumUserFlag = (context.UserScrums.Where(x => x.ScrumID == c.ScrumID && x.UserID == user.UserID&&x.IsActive==true).Select(x => x.UserID).FirstOrDefault() > 0 ? true : false)
                                            }).ToList();
                     return lstScrumDetails;
                 //}
                 //else
                 //{
                 //    var lstScrumDetails = (from c in context.Scrums
                 //                           where c.IsActive == true
                 //                           select new ScrumInformation
                 //                           {
                 //                               ScrumName = c.Name,
                 //                               ScrumDescription = c.Description,
                 //                               ScrumID = c.ScrumID,
                 //                               IsScrumMaster = false,
                 //                               ScrumUserFlag = (context.UserScrums.Where(x => x.ScrumID == c.ScrumID && x.UserID == userID).Select(x => x.UserID).FirstOrDefault() > 0 ? true : false)
                 //                           }).ToList();
                 //    return lstScrumDetails;
                 //}
                
             }
         }

       
         public static  List<ScrumDetails> GetAllScrumInformationByScrumID(int scrumID)
         {
             using (ScrumEntities context = new ScrumEntities())
             {
                 var scrumDetails=(from c in context.Scrums
                                   where c.ScrumID==scrumID
                                   select new ScrumDetails
                                   {
                                       ScrumName=c.Name,
                                       ScrumDescription=c.Description,
                                       DailyOccursTime=c.DailyOccursTime,
                                       DaysOfOccurence = c.DaysOfOccurence,
                                       Remainder=c.Remainder
                                   }).ToList();
                 return scrumDetails;
             }
         }

         public static void DeleteScrum(int scrumID)
         {
             using (ScrumEntities context = new ScrumEntities())
             {
                 context.DeleteScrum(scrumID);
                 context.SaveChanges();
             }
         }

         public static bool CheckIsUserUpdateStatusOrNot(string email, int scrumID)
         {
             DateTime date = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
             bool isStatusUpdated;
             using (ScrumEntities context = new ScrumEntities())
             {
                 int userID = context.UserDetails.Where(x => x.Email == email).Select(x => x.UserID).FirstOrDefault();
                 int userScrumID = context.UserScrums.Where(x => x.UserID == userID && x.ScrumID == scrumID).Select(x=>x.UserScrumID).FirstOrDefault();
                 isStatusUpdated = (context.UserScrumDetailYesterdayTasks.Where(x => x.UserScrumID == userScrumID && x.Date == date).Select(x => x.Task).Count()) > 0 ? true : false;
                 return isStatusUpdated;

             }
         }

        
    }
}
