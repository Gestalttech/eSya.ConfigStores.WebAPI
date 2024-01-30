using eSya.ConfigStores.DL.Entities;
using eSya.ConfigStores.DO;
using eSya.ConfigStores.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.DL.Repository
{
    public class InventoryRulesRepository : IInventoryRulesRepository
    {
        private readonly IStringLocalizer<InventoryRulesRepository> _localizer;
        public InventoryRulesRepository(IStringLocalizer<InventoryRulesRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Inventory Rules
        public async Task<List<DO_InventoryRules>> GetInventoryRules()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcinvrs

                    .Select(i => new DO_InventoryRules
                    {
                        InventoryRuleId = i.InventoryRuleId,
                        InventoryRuleDesc = i.InventoryRuleDesc,
                        InventoryRule = i.InventoryRule,
                        ApplyToSrn = i.ApplyToSrn,
                        ActiveStatus = i.ActiveStatus
                    }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertInventoryRule(DO_InventoryRules inventoryRule)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_invRuleId = db.GtEcinvrs.Where(i => i.InventoryRuleId.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_invRuleId != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0134", Message = string.Format(_localizer[name: "W0134"]) };
                        }
                        var is_invRuledescExists = db.GtEcinvrs.Where(i => i.InventoryRuleDesc.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleDesc.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_invRuledescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0135", Message = string.Format(_localizer[name: "W0135"]) };
                        }

                        var inv_Rule = new GtEcinvr
                        {
                            InventoryRuleId = inventoryRule.InventoryRuleId,
                            InventoryRuleDesc = inventoryRule.InventoryRuleDesc,
                            InventoryRule = inventoryRule.InventoryRule,
                            ApplyToSrn = inventoryRule.ApplyToSrn,
                            FormId = inventoryRule.FormId,
                            ActiveStatus = inventoryRule.ActiveStatus,
                            CreatedBy = inventoryRule.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = inventoryRule.TerminalID
                        };
                        db.GtEcinvrs.Add(inv_Rule);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateInventoryRule(DO_InventoryRules inventoryRule)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_invRuledescExists = db.GtEcinvrs.Where(i => i.InventoryRuleDesc.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleDesc.ToUpper().Replace(" ", "")
                        && i.InventoryRuleId.ToUpper().Replace(" ", "") != inventoryRule.InventoryRuleId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_invRuledescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0135", Message = string.Format(_localizer[name: "W0135"]) };
                        }

                        GtEcinvr inv_Rule = db.GtEcinvrs.Where(i => i.InventoryRuleId.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (inv_Rule != null)
                        {
                            inv_Rule.InventoryRuleDesc = inventoryRule.InventoryRuleDesc;
                            inv_Rule.InventoryRule = inventoryRule.InventoryRule;
                            inv_Rule.ApplyToSrn = inventoryRule.ApplyToSrn;
                            inv_Rule.ActiveStatus = inventoryRule.ActiveStatus;
                            inv_Rule.ModifiedBy = inventoryRule.UserID;
                            inv_Rule.ModifiedOn = DateTime.Now;
                            inv_Rule.ModifiedTerminal = inventoryRule.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0136", Message = string.Format(_localizer[name: "W0136"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveInventoryRules(bool status, string InventoryId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcinvr inventory_rule = db.GtEcinvrs.Where(w => w.InventoryRuleId.ToUpper().Replace(" ", "") == InventoryId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (inventory_rule == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0136", Message = string.Format(_localizer[name: "W0136"]) };
                        }

                        inventory_rule.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion
    }
}
