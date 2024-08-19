using eSya.ConfigStores.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.IF
{
    public interface ISACCodesRepository
    {
        #region SAC Codes
        Task<List<DO_SACCodes>> GetSACCodes(int ISDCode);
        Task<DO_SACCodes> GetSACCodeByCode(int ISDCode, string SACCodeID);
        Task<DO_ReturnParameter> InsertIntoSACCode(DO_SACCodes obj);
        Task<DO_ReturnParameter> UpdateSACSACCode(DO_SACCodes obj);
        Task<DO_ReturnParameter> DeleteSACCode(int ISDCode, string SACCodeID);
        #endregion
    }
}
