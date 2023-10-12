using eSya.ConfigStores.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.IF
{
    public interface IItemControlRepository
    {
        #region ItemGroup
        Task<List<DO_ItemGroup>> GetItemGroup();
        Task<DO_ItemGroup> GetItemGroupByID(int ItemGroupID);
        Task<DO_ReturnParameter> AddOrUpdateItemGroup(DO_ItemGroup obj);
        #endregion

        #region ItemCategory
        Task<List<DO_ItemCategory>> GetItemCategory();
        Task<DO_ItemCategory> GetItemCategoryByID(int ItemCategory);
        Task<DO_ReturnParameter> AddOrUpdateItemCategory(DO_ItemCategory obj);
        #endregion

        #region SubCategory
        Task<List<DO_ItemSubCategory>> GetItemSubCategoryByCateID(int ItemCategory);
        Task<DO_ItemSubCategory> GetItemSubCategoryByID(int ItemSubCategory);
        Task<DO_ReturnParameter> AddOrUpdateItemSubCategory(DO_ItemSubCategory obj);
        Task<List<DO_ItemSubCategory>> GetItemSubCategories();
        //Task<List<DO_ItemSParameter>> GetItemSParameter();
        #endregion

        #region ItemGroupCategoryMapping
        Task<List<DO_ItemCategory>> GetItemCategoryByItemGroupID(int ItemGroup);
        Task<List<DO_ItemSubCategory>> GetItemCategoryByItemGroupCategory(int ItemGroup, int ItemCategory);
        Task<DO_ReturnParameter> ItemGroupCateSubCateMapping(DO_ItemGroupCategory obj);
        Task<DO_ItemGroupCategory> GetMappinRecord(int ItemGroupID, int ItemCategory, int ItemSubCategory);
        Task<List<DO_ItemCategory>> GetItemCategoriesByItemGroup();
        Task<List<DO_ItemSubCategory>> GetItemSubCategoriesByItemGroupCategory();
        #endregion
    }
}
