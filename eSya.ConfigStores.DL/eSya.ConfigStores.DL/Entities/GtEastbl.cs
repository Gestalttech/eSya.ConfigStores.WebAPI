using System;
using System.Collections.Generic;

namespace eSya.ConfigStores.DL.Entities
{
    public partial class GtEastbl
    {
        public int BusinessKey { get; set; }
        public int StoreCode { get; set; }
        public string StoreClass { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEcbsln BusinessKeyNavigation { get; set; } = null!;
    }
}
