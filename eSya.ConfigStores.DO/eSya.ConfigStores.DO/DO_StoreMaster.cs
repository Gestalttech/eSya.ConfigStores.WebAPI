using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.DO
{
    public class DO_StoreMaster
    {
        public int StoreCode { get; set; }
        public string StoreDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }

        public List<DO_eSyaParameter> l_FormParameter { get; set; }

    }

    public class DO_StoreBusinessLink
    {
        public int BusinessKey { get; set; }
        public int StoreCode { get; set; }
        public string? StoreClass { get; set; }
        public int PortfolioId { get; set; }
        public bool ActiveStatus { get; set; }
        public string? FormId { get; set; }
        public int UserID { get; set; }
        public string? TerminalID { get; set; }
        public List<DO_StoreBusinessLink>? lst_businessLink { get; set; }
        public string? PortfolioDesc { get; set; }
    }

    public class DO_StoreFormLink
    {
        public int FormId { get; set; }
        public int StoreCode { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
    public class DO_eSyaParameter
    {
        public int ParameterID { get; set; }
        public bool ParmAction { get; set; }
        public decimal ParmValue { get; set; }
        public decimal ParmPerct { get; set; }
        public string? ParmDesc { get; set; }
        public bool ActiveStatus { get; set; }
    }
    public class DO_PortfolioBusinessLink
    {
        public int BusinessKey { get; set; }
        public int StoreCode { get; set; }
        public int PortfolioId { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
    public class DO_PortfolioMaster
    {
        public int PortfolioId { get; set; }
        public string PortfolioDesc { get; set; } 
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
