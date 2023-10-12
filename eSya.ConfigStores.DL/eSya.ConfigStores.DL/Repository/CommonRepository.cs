using eSya.ConfigStores.DL.Entities;
using eSya.ConfigStores.DO;
using eSya.ConfigStores.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.DL.Repository
{
    public class CommonRepository: ICommonRepository
    {
        public async Task<List<DO_ItemGroup>> GetItemGroup()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEiitgrs
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_ItemGroup
                        {
                            ItemGroupId = r.ItemGroup,
                            ItemGroupDesc = r.ItemGroupDesc
                        }).OrderBy(o => o.ItemGroupDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ItemCategory>> GetItemCategory(int ItemGroup)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEiitgcs
                        .Where(w => w.ItemGroup == ItemGroup && w.ActiveStatus)
                        .Select(r => new DO_ItemCategory
                        {
                            ItemCategory = r.ItemCategory,
                            ItemCategoryDesc = r.ItemCategoryNavigation.ItemCategoryDesc,
                        }).OrderBy(o => o.ItemCategoryDesc).Distinct().ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ItemSubCategory>> GetItemSubCategory(int ItemCategory)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEiitscs
                        .Where(w => w.ItemCategory == ItemCategory && w.ActiveStatus)
                        .Select(r => new DO_ItemSubCategory
                        {
                            ItemSubCategory = r.ItemSubCategory,
                            ItemSubCategoryDesc = r.ItemSubCategoryDesc,
                        }).OrderBy(o => o.ItemSubCategoryDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ItemGroupCategoryLink>> GetItemCategoryForItemGroup(int ItemGroup)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEiitgcs
                        .Where(w => w.ItemGroup == ItemGroup && w.ActiveStatus)
                        .Select(r => new DO_ItemGroupCategoryLink
                        {
                            ItemCategory = r.ItemCategory,
                            ItemSubCategory = r.ItemSubCategory
                        }).OrderBy(o => o.ItemCategory).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_BusinessLocation>> GetBusinessKey()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcbslns
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.BusinessKey,
                            LocationDescription = r.BusinessName + "-" + r.LocationDescription
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
