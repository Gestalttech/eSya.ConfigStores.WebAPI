using System;
using System.Collections.Generic;

namespace eSya.ConfigStores.DL.Entities
{
    public partial class GtEcpast
    {
        public int StoreCode { get; set; }
        public int ParameterId { get; set; }
        public bool ParamAction { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEcstrm StoreCodeNavigation { get; set; } = null!;
    }
}
