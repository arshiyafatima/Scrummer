using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Scrummer.Common.Model
{
   public class UserDetails
    {
       public int UserID { get; set; }
       [Required]
       public string Password { get; set; }
       public string UserName { get; set; }
       
       public string FirstName { get; set; }
       
       public string LastName { get; set; }
       public string EmailId { get; set; }
       public int? UserScrumID { get; set; }
       public bool IsStatusUpdated { get; set; }
       public byte[] ImageName { get; set; }

       public string TimeZone { get; set; }
    }
}
