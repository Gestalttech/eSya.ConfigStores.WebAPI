using eSya.ConfigStores.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.IF
{
    public interface IStoreMasterRepository
    {
        #region Store Master

        Task<List<DO_StoreMaster>> GetStoreCodes();

        Task<DO_StoreMaster> GetStoreParameterList(int StoreCode);

        Task<DO_ReturnParameter> InsertOrUpdateStoreCodes(DO_StoreMaster storecodes);

        Task<DO_ReturnParameter> DeleteStoreCode(int Storecode);

        Task<DO_ReturnParameter> ActiveOrDeActiveStoreCode(bool status, string storetype, int storecode);

        Task<List<DO_StoreMaster>> GetActiveStoreCodes();

        #endregion

        #region Store Business Link

        Task<List<DO_StoreMaster>> GetStoreList(int BusienssKey);

        Task<List<DO_StoreBusinessLink>> GetStoreBusinessLinkInfo(int BusinessKey, int StoreCode);

        //Task<DO_ReturnParameter> InsertUpdateStoreBusinessLink(DO_StoreBusinessLink obj);

        Task<DO_ReturnParameter> InsertOrUpdateStoreBusinessLink(DO_StoreBusinessLink obj);
        #endregion

        #region Store Form Link

        Task<List<DO_Forms>> GetFormForStorelinking();

        Task<List<DO_StoreMaster>> GetStoreFormLinked(int formId);

        Task<DO_ReturnParameter> InsertIntoFormStoreLink(List<DO_StoreFormLink> l_obj);

        #endregion
    }
}
