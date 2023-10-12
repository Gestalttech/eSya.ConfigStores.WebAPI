using System;
using System.Collections.Generic;

namespace eSya.ConfigStores.DL.Entities
{
    public partial class GtEiitgc
    {
        public int ItemGroup { get; set; }
        public int ItemCategory { get; set; }
        public int ItemSubCategory { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEiitct ItemCategoryNavigation { get; set; } = null!;
        public virtual GtEiitgr ItemGroupNavigation { get; set; } = null!;
    }
}
