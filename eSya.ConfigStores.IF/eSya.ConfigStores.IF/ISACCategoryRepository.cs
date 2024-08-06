using eSya.ConfigStores.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.IF
{
    public interface ISACCategoryRepository
    {
        #region SAC Category
        Task<List<DO_SACCategory>> GetSACCategories(int ISDCode);
        Task<DO_SACCategory> GetSACCategoryByCategoryID(int ISDCode, string SACClassID, string SACCategoryID);
        Task<DO_ReturnParameter> InsertIntoSACCategory(DO_SACCategory obj);
        Task<DO_ReturnParameter> UpdateSACCategory(DO_SACCategory obj);
        Task<DO_ReturnParameter> DeleteSACCategory(int ISDCode, string SACClassID, string SACCategoryID);
        #endregion
    }
}
