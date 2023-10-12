using System;
using System.Collections.Generic;

namespace eSya.ConfigStores.DL.Entities
{
    public partial class GtEcfmfd
    {
        public int FormId { get; set; }
        public string? FormCode { get; set; }
        public string FormName { get; set; } = null!;
        public string ControllerName { get; set; } = null!;
        public string? ToolTip { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
