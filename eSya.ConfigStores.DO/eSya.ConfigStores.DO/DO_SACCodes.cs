using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.DO
{
    public class DO_SACCodes
    {
        public int Isdcode { get; set; }
        public string Sacclass { get; set; } 
        public string Saccategory { get; set; } 
        public string Saccode { get; set; } 
        public string Sacdescription { get; set; } 
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? ParentId { get; set; }
    }
}
