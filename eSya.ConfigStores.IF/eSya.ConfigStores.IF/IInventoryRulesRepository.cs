using eSya.ConfigStores.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.IF
{
    public interface IInventoryRulesRepository
    {
        #region Inventory Rules
        Task<List<DO_InventoryRules>> GetInventoryRules();
        Task<DO_ReturnParameter> InsertInventoryRule(DO_InventoryRules inventoryRule);
        Task<DO_ReturnParameter> UpdateInventoryRule(DO_InventoryRules inventoryRule);
        Task<DO_ReturnParameter> ActiveOrDeActiveInventoryRules(bool status, string InventoryId);
        #endregion
    }
}
