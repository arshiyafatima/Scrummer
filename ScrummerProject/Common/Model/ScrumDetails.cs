using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrummer.Common.Model
{
   public  class ScrumDetails
    {
       public int ScrumID { get; set; }
       public string ScrumName { get; set; }
       public string ScrumDescription { get; set; }
       public string DailyOccursTime { get; set; }
       public string Remainder { get; set; }
       public string DaysOfOccurence { get; set; }
    }
}
