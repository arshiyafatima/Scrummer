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
    
    public partial class UserDetail
    {
        public UserDetail()
        {
            this.UserScrums = new HashSet<UserScrum>();
        }
    
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Image { get; set; }
        public string TimeZone { get; set; }
    
        public virtual ICollection<UserScrum> UserScrums { get; set; }
    }
}
