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
    
    public partial class Scrum
    {
        public Scrum()
        {
            this.UserScrums = new HashSet<UserScrum>();
        }
    
        public int ScrumID { get; set; }
        public string Name { get; set; }
        public string DailyOccursTime { get; set; }
        public string DaysOfOccurence { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string Remainder { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string TimeZone { get; set; }
    
        public virtual ICollection<UserScrum> UserScrums { get; set; }
    }
}
