using eSya.ConfigStores.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.IF
{
    public interface ISACClassRepository
    {
        #region Define SAC Class
        Task<List<DO_SACClass>> GetSACClasses(int ISDCode);
        Task<DO_SACClass> GetSACClassByClassID(int ISDCode, string SACClassID);
        Task<DO_ReturnParameter> InsertIntoSACClass(DO_SACClass obj);
        Task<DO_ReturnParameter> UpdateSACClass(DO_SACClass obj);
        Task<DO_ReturnParameter> DeleteSACClass(int ISDCode, string SACClassID);
        #endregion
    }
}
