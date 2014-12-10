using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrummer.Common.Model
{
    public class ScrumInformation
    {
        public int ScrumID { get; set; }
        public string ScrumName { get; set; }
        public string ScrumDescription { get; set; }
        public bool ScrumUserFlag { get; set; }
        public bool IsScrumMaster { get; set; }

    }
}
