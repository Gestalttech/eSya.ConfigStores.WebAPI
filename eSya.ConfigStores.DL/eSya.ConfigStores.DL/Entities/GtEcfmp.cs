using System;
using System.Collections.Generic;

namespace eSya.ConfigStores.DL.Entities
{
    public partial class GtEcfmp
    {
        public int FormId { get; set; }
        public int ParameterId { get; set; }
        public int SubParameterId { get; set; }
        public bool ParmAction { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
