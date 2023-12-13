using System;
using System.Collections.Generic;

namespace eSya.ConfigStores.DL.Entities
{
    public partial class GtEiitct
    {
        public GtEiitct()
        {
            GtEiitgcs = new HashSet<GtEiitgc>();
            GtEiitscs = new HashSet<GtEiitsc>();
        }

        public int ItemCategory { get; set; }
        public string ItemCategoryDesc { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual ICollection<GtEiitgc> GtEiitgcs { get; set; }
        public virtual ICollection<GtEiitsc> GtEiitscs { get; set; }
    }
}
