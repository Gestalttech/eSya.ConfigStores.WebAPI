using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.DO
{
    public class DO_SACClass
    {
        public int Isdcode { get; set; }
        public string Sacclass { get; set; } 
        public string SacclassDesc { get; set; } 
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
