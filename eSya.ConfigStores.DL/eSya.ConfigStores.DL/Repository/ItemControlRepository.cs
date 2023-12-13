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
    public class ItemControlRepository: IItemControlRepository
    {
        private readonly IStringLocalizer<ItemControlRepository> _localizer;
        public ItemControlRepository(IStringLocalizer<ItemControlRepository> localizer)
        {
            _localizer = localizer;
        }

        #region ItemGroup
        public async Task<List<DO_ItemGroup>> GetItemGroup()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEiitgrs
                                 .Select(x => new DO_ItemGroup
                                 {
                                     ItemGroupId = x.ItemGroup,
                                     ItemGroupDesc = x.ItemGroupDesc,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ItemGroup> GetItemGroupByID(int ItemGroupID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEiitgrs
                        .Where(i => i.ItemGroup == ItemGroupID)
                                 .Select(x => new DO_ItemGroup
                                 {
                                     ItemGroupId = x.ItemGroup,
                                     ItemGroupDesc = x.ItemGroupDesc,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).FirstOrDefaultAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> AddOrUpdateItemGroup(DO_ItemGroup obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.ItemGroupId == null || obj.ItemGroupId == 0)
                        {
                            var RecordExist = db.GtEiitgrs.Where(w => w.ItemGroupDesc == obj.ItemGroupDesc).Count();
                            if (RecordExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0091", Message = string.Format(_localizer[name: "W0091"]) };
                            }
                            else
                            {


                                var newItemGroupId = db.GtEiitgrs.Select(a => (int)a.ItemGroup).DefaultIfEmpty().Max() + 1;

                                var itemgroup = new GtEiitgr
                                {
                                    ItemGroup = newItemGroupId,
                                    ItemGroupDesc = obj.ItemGroupDesc,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = obj.CreatedOn,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEiitgrs.Add(itemgroup);

                            }
                        }
                        else
                        {
                            if (!obj.ActiveStatus)
                            {
                                var LinkExist = db.GtEiitgcs.Where(w => w.ItemGroup == obj.ItemGroupId && w.ActiveStatus).Count();
                                if (LinkExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0092", Message = string.Format(_localizer[name: "W0092"]) };
                                }
                            }
                            var updatedItemGroup = db.GtEiitgrs.Where(w => w.ItemGroup == obj.ItemGroupId).FirstOrDefault();
                            if (updatedItemGroup.ItemGroupDesc != obj.ItemGroupDesc)
                            {
                                var RecordExist = db.GtEiitgrs.Where(w => w.ItemGroupDesc == obj.ItemGroupDesc).Count();
                                if (RecordExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0093", Message = string.Format(_localizer[name: "W0093"]) };
                                }

                            }
                            updatedItemGroup.ItemGroupDesc = obj.ItemGroupDesc;
                            updatedItemGroup.ActiveStatus = obj.ActiveStatus;
                            updatedItemGroup.ModifiedBy = obj.UserID;
                            updatedItemGroup.ModifiedOn = obj.CreatedOn;
                            updatedItemGroup.ModifiedTerminal = obj.TerminalID;

                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

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

        #region ItemCategory
        public async Task<List<DO_ItemCategory>> GetItemCategory()
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitcts
                             .Select(c => new DO_ItemCategory
                             {
                                 ItemCategory = c.ItemCategory,
                                 ItemCategoryDesc = c.ItemCategoryDesc,
                                 ActiveStatus = c.ActiveStatus
                             }
                            ).ToListAsync();
                return await result;
            }

        }
        public async Task<DO_ItemCategory> GetItemCategoryByID(int ItemCategory)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitcts
                    .Where(c => c.ItemCategory == ItemCategory)
                    .Select(s => new DO_ItemCategory
                    {
                        ItemCategoryDesc = s.ItemCategoryDesc,
                        //BudgetAmount = s.RevisedBudgetAmount,
                        //CommittmentAmount = s.ComittmentAmount,
                        ActiveStatus = s.ActiveStatus
                    }).FirstOrDefaultAsync();

                return await result;
            }
        }
        public async Task<DO_ReturnParameter> AddOrUpdateItemCategory(DO_ItemCategory obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.ItemCategory == null || obj.ItemCategory == 0)
                        {
                            var RecordExist = db.GtEiitcts.Where(w => w.ItemCategoryDesc == obj.ItemCategoryDesc).Count();
                            if (RecordExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0094", Message = string.Format(_localizer[name: "W0094"]) };
                            }
                            else
                            {


                                var newItemCategoryId = db.GtEiitcts.Select(a => (int)a.ItemCategory).DefaultIfEmpty().Max() + 1;

                                var itemcategory = new GtEiitct
                                {
                                    ItemCategory = newItemCategoryId,
                                    ItemCategoryDesc = obj.ItemCategoryDesc,
                                    //OriginalBudgetAmount = obj.BudgetAmount,
                                    //RevisedBudgetAmount = obj.BudgetAmount,
                                    //ComittmentAmount = obj.CommittmentAmount,
                                    ActiveStatus = obj.ActiveStatus,

                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = obj.CreatedOn,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEiitcts.Add(itemcategory);

                            }
                        }
                        else
                        {
                            if (!obj.ActiveStatus)
                            {
                                var LinkExist = db.GtEiitgcs.Where(w => w.ItemCategory == obj.ItemCategory && w.ActiveStatus).Count();
                                if (LinkExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0095", Message = string.Format(_localizer[name: "W0095"]) };
                                }
                            }

                            var updatedItemCategory = db.GtEiitcts.Where(w => w.ItemCategory == obj.ItemCategory).FirstOrDefault();
                            if (updatedItemCategory.ItemCategoryDesc != obj.ItemCategoryDesc)
                            {
                                var RecordExist = db.GtEiitcts.Where(w => w.ItemCategoryDesc == obj.ItemCategoryDesc).Count();
                                if (RecordExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0094", Message = string.Format(_localizer[name: "W0094"]) };
                                }
                            }
                            updatedItemCategory.ItemCategoryDesc = obj.ItemCategoryDesc;
                            //updatedItemCategory.RevisedBudgetAmount = obj.BudgetAmount;
                            //updatedItemCategory.ComittmentAmount = obj.CommittmentAmount;
                            updatedItemCategory.ActiveStatus = obj.ActiveStatus;

                            updatedItemCategory.FormId = obj.FormID;
                            updatedItemCategory.ModifiedBy = obj.UserID;
                            updatedItemCategory.ModifiedOn = obj.CreatedOn;
                            updatedItemCategory.ModifiedTerminal = obj.TerminalID;

                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

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

        #region SubCategory
        public async Task<List<DO_ItemSubCategory>> GetItemSubCategoryByCateID(int ItemCategory)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitscs
                    .Where(c => c.ItemCategory == ItemCategory)
                    .Select(s => new DO_ItemSubCategory
                    {
                        ItemSubCategory = s.ItemSubCategory,
                        ItemSubCategoryDesc = s.ItemSubCategoryDesc,
                        ActiveStatus = s.ActiveStatus
                    }).ToListAsync();
                return await result;
            }
        }
        public async Task<DO_ItemSubCategory> GetItemSubCategoryByID(int ItemSubCategory)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitscs
                    .Where(c => c.ItemSubCategory == ItemSubCategory)
                    .Select(s => new DO_ItemSubCategory
                    {
                        ItemSubCategoryDesc = s.ItemSubCategoryDesc,
                        ActiveStatus = s.ActiveStatus,
                    }).FirstOrDefaultAsync();

                return await result;
            }
        }
        public async Task<DO_ReturnParameter> AddOrUpdateItemSubCategory(DO_ItemSubCategory obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (obj.ItemSubCategory == null || obj.ItemSubCategory == 0)
                        {
                            var RecordExist = db.GtEiitscs.Where(w => w.ItemSubCategoryDesc == obj.ItemSubCategoryDesc && w.ItemCategory == obj.ItemCategory).Count();
                            if (RecordExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0096", Message = string.Format(_localizer[name: "W0096"]) };
                            }
                            else
                            {

                                var newItemSubCategoryId = db.GtEiitscs.Select(a => (int)a.ItemSubCategory).DefaultIfEmpty().Max() + 1;

                                var itemsubcategory = new GtEiitsc
                                {
                                    ItemCategory = obj.ItemCategory,
                                    ItemSubCategory = newItemSubCategoryId,
                                    ItemSubCategoryDesc = obj.ItemSubCategoryDesc,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = obj.CreatedOn,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEiitscs.Add(itemsubcategory);
                            }
                        }
                        else
                        {
                            if (!obj.ActiveStatus)
                            {
                                var LinkExist = db.GtEiitgcs.Where(w => w.ItemCategory == obj.ItemCategory && w.ActiveStatus).Count();
                                if (LinkExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0097", Message = string.Format(_localizer[name: "W0097"]) };
                                }
                            }
                            var updatedItemSubCategory = db.GtEiitscs.Where(w => w.ItemSubCategory == obj.ItemSubCategory).FirstOrDefault();
                            if (updatedItemSubCategory.ItemSubCategoryDesc != obj.ItemSubCategoryDesc)
                            {
                                var RecordExist = db.GtEiitscs.Where(w => w.ItemSubCategoryDesc == obj.ItemSubCategoryDesc && w.ItemCategory == obj.ItemCategory).Count();
                                if (RecordExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0098", Message = string.Format(_localizer[name: "W0098"]) };
                                }
                            }
                            updatedItemSubCategory.ItemCategory = obj.ItemCategory;
                            updatedItemSubCategory.ItemSubCategoryDesc = obj.ItemSubCategoryDesc;
                            updatedItemSubCategory.ActiveStatus = obj.ActiveStatus;

                            updatedItemSubCategory.FormId = obj.FormID;
                            updatedItemSubCategory.ModifiedBy = obj.UserID;
                            updatedItemSubCategory.ModifiedOn = obj.CreatedOn;
                            updatedItemSubCategory.ModifiedTerminal = obj.TerminalID;


                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<List<DO_ItemSubCategory>> GetItemSubCategories()
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitscs
                    .Select(s => new DO_ItemSubCategory
                    {
                        ItemCategory = s.ItemCategory,
                        ItemSubCategory = s.ItemSubCategory,
                        ItemSubCategoryDesc = s.ItemSubCategoryDesc,
                        ActiveStatus = s.ActiveStatus
                    }).ToListAsync();
                return await result;
            }
        }
        #endregion

        #region ItemGroupCategoryMapping
        public async Task<List<DO_ItemCategory>> GetItemCategoryByItemGroupID(int ItemGroup)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitgcs
                    .Join(db.GtEiitcts,
                    gc => gc.ItemCategory,
                    ct => ct.ItemCategory,
                    (gc, ct) => new { gc, ct })
                    .Where(w => w.gc.ItemGroup == ItemGroup)
                    .Select(s => new DO_ItemCategory
                    {
                        ItemCategory = s.gc.ItemCategory,
                        ItemCategoryDesc = s.ct.ItemCategoryDesc,
                        // ActiveStatus=s.gc.ActiveStatus
                    }
                    ).Distinct().ToListAsync();

                return await result;
            }
        }
        public async Task<List<DO_ItemSubCategory>> GetItemCategoryByItemGroupCategory(int ItemGroup, int ItemCategory)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitgcs
                    .Join(db.GtEiitscs,
                    gc => gc.ItemSubCategory,
                    sc => sc.ItemSubCategory,
                    (gc, sc) => new { gc, sc })
                    .Where(w => w.gc.ItemGroup == ItemGroup && w.gc.ItemCategory == ItemCategory)
                    .Select(s => new DO_ItemSubCategory
                    {
                        ItemSubCategory = s.gc.ItemSubCategory,
                        ItemSubCategoryDesc = s.sc.ItemSubCategoryDesc,
                        ActiveStatus = s.gc.ActiveStatus

                    }).Distinct().ToListAsync();

                return await result;
            }
        }
        public async Task<DO_ReturnParameter> ItemGroupCateSubCateMapping(DO_ItemGroupCategory obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (obj.flag == 0)
                        {
                            var RecordExist = db.GtEiitgcs.Where(w => w.ItemGroup == obj.ItemGroupID && w.ItemCategory == obj.ItemCategory && w.ItemSubCategory == obj.ItemSubCategory).Count();
                            if (RecordExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0099", Message = string.Format(_localizer[name: "W0099"]) };
                            }
                            else
                            {


                                var mappingrecord = new GtEiitgc
                                {
                                    ItemGroup = obj.ItemGroupID,
                                    ItemCategory = obj.ItemCategory,
                                    ItemSubCategory = obj.ItemSubCategory,
                                    OriginalBudgetAmount = obj.BudgetAmount,
                                    RevisedBudgetAmount = obj.BudgetAmount,
                                    ComittmentAmount = obj.CommittmentAmount,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = obj.CreatedOn,
                                    CreatedTerminal = obj.TerminalID

                                };
                                db.GtEiitgcs.Add(mappingrecord);


                            }
                        }

                        else if (obj.flag == 1)
                        {
                            var updatedMappingRecord = db.GtEiitgcs.Where(w => w.ItemGroup == obj.ItemGroupID && w.ItemCategory == obj.ItemCategory && w.ItemSubCategory == obj.ItemSubCategory).FirstOrDefault();
                            updatedMappingRecord.RevisedBudgetAmount = obj.BudgetAmount;
                            updatedMappingRecord.ComittmentAmount = obj.CommittmentAmount;
                            updatedMappingRecord.ActiveStatus = obj.ActiveStatus;
                            updatedMappingRecord.FormId = obj.FormID;
                            updatedMappingRecord.ModifiedBy = obj.UserID;
                            updatedMappingRecord.ModifiedOn = obj.CreatedOn;
                            updatedMappingRecord.ModifiedTerminal = obj.TerminalID;
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }


                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ItemGroupCategory> GetMappinRecord(int ItemGroupID, int ItemCategory, int ItemSubCategory)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitgcs
                    .Where(w => w.ItemGroup == ItemGroupID && w.ItemCategory == ItemCategory && w.ItemSubCategory == ItemSubCategory)
                             .Select(s => new DO_ItemGroupCategory
                             { 
                                 BudgetAmount = s.RevisedBudgetAmount,
                                 CommittmentAmount = s.ComittmentAmount,
                                 ActiveStatus = s.ActiveStatus
                             }
                            ).FirstOrDefaultAsync();
                return await result;
            }
        }

        public async Task<List<DO_ItemCategory>> GetItemCategoriesByItemGroup()
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitgcs
                    .Join(db.GtEiitcts,
                    gc => gc.ItemCategory,
                    ct => ct.ItemCategory,
                    (gc, ct) => new { gc, ct })
                    .Select(s => new DO_ItemCategory
                    {
                        ItemGroupId = s.gc.ItemGroup,
                        ItemCategory = s.gc.ItemCategory,
                        ItemCategoryDesc = s.ct.ItemCategoryDesc,
                    }
                    ).Distinct().ToListAsync();

                return await result;
            }
        }
        public async Task<List<DO_ItemSubCategory>> GetItemSubCategoriesByItemGroupCategory()
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                var result = db.GtEiitgcs
                    .Join(db.GtEiitscs,
                    gc => gc.ItemSubCategory,
                    sc => sc.ItemSubCategory,
                    (gc, sc) => new { gc, sc })
                    .Select(s => new DO_ItemSubCategory
                    {
                        ItemGroupId = s.gc.ItemGroup,
                        ItemCategory = s.gc.ItemCategory,
                        ItemSubCategory = s.gc.ItemSubCategory,
                        ItemSubCategoryDesc = s.sc.ItemSubCategoryDesc,
                        ActiveStatus = s.gc.ActiveStatus

                    }).Distinct().ToListAsync();

                return await result;
            }
        }
        #endregion
    }
}
