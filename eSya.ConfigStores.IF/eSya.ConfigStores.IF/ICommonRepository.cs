using eSya.ConfigStores.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.IF
{
    public interface ICommonRepository
    {
     
        Task<List<DO_ItemGroup>> GetItemGroup();

        Task<List<DO_ItemCategory>> GetItemCategory(int ItemGroup);

        Task<List<DO_ItemSubCategory>> GetItemSubCategory(int ItemCategory);

        Task<List<DO_ItemGroupCategoryLink>> GetItemCategoryForItemGroup(int ItemGroup);

        Task<List<DO_BusinessLocation>> GetBusinessKey();

      
    }
}
