//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scrummer.Entity.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserScrum
    {
        public UserScrum()
        {
            this.UserScrumDetailBlockers = new HashSet<UserScrumDetailBlocker>();
            this.UserScrumDetailTodayTasks = new HashSet<UserScrumDetailTodayTask>();
            this.UserScrumDetailYesterdayTasks = new HashSet<UserScrumDetailYesterdayTask>();
        }
    
        public int UserScrumID { get; set; }
        public int UserID { get; set; }
        public int ScrumID { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Scrum Scrum { get; set; }
        public virtual UserDetail UserDetail { get; set; }
        public virtual ICollection<UserScrumDetailBlocker> UserScrumDetailBlockers { get; set; }
        public virtual ICollection<UserScrumDetailTodayTask> UserScrumDetailTodayTasks { get; set; }
        public virtual ICollection<UserScrumDetailYesterdayTask> UserScrumDetailYesterdayTasks { get; set; }
    }
}
