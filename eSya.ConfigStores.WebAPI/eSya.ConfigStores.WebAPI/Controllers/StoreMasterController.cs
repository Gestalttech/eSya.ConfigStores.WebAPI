using eSya.ConfigStores.DO;
using eSya.ConfigStores.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigStores.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreMasterController : ControllerBase
    {
        private readonly IStoreMasterRepository _StoreMasterRepository;
        private readonly ICommonRepository _ICommonRepository;
        public StoreMasterController(IStoreMasterRepository StoreMasterRepository, ICommonRepository ICommonRepository)
        {
            _StoreMasterRepository = StoreMasterRepository;
            _ICommonRepository = ICommonRepository;
        }

        #region Store Master
        /// <summary>
        /// Getting  Store Codes List.
        /// UI Reffered - Store Codes Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStoreCodes()
        {
            var store_master = await _StoreMasterRepository.GetStoreCodes();
            return Ok(store_master);
        }
        /// <summary>
        /// Getting  Store Parameter List.
        /// UI Reffered - Store Master
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStoreParameterList(int StoreCode)
        {
            var sp_Param = await _StoreMasterRepository.GetStoreParameterList(StoreCode);
            return Ok(sp_Param);
        }
        /// <summary>
        /// Insert Or Update Store Codes .
        /// UI Reffered -Store Codes
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateStoreCodes(DO_StoreMaster storecodes)
        {
            var msg = await _StoreMasterRepository.InsertOrUpdateStoreCodes(storecodes);
            return Ok(msg);

        }
        [HttpGet]
        public async Task<IActionResult> DeleteStoreCode(int Storecode)
        {
            var msg = await _StoreMasterRepository.DeleteStoreCode(Storecode);
            return Ok(msg);

        }
        /// <summary>
        /// Getting  Store Codes for dropdown.
        /// UI Reffered - Business Store
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveStoreCodes()
        {
            var astore_master = await _StoreMasterRepository.GetActiveStoreCodes();
            return Ok(astore_master);
        }
        /// <summary>
        /// Active Or De Active Store Code.
        /// UI Reffered - Store Code
        /// </summary>
        /// <param name="status-storetype-storecode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveStoreCode(bool status, string storetype, int storecode)
        {
            var msg = await _StoreMasterRepository.ActiveOrDeActiveStoreCode(status, storetype, storecode);
            return Ok(msg);
        }
        #endregion  Store Master

        #region Store Business Link

        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var ds = _ICommonRepository.GetBusinessKey();
            return Ok(ds);
        }

        /// <summary>
        /// Getting  Store Business Link Tree.
        /// UI Reffered - Store Business Link
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStoreList(int BusinessKey)
        {
            var str_lst = await _StoreMasterRepository.GetStoreList(BusinessKey);
            return Ok(str_lst);

        }

        /// <summary>
        /// Getting  Store Business Link Info.
        /// UI Reffered - Store Business Link
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStoreBusinessLinkInfo(int BusinessKey, int StoreCode)
        {
            var storelinks = await _StoreMasterRepository.GetStoreBusinessLinkInfo(BusinessKey, StoreCode);
            return Ok(storelinks);

        }

        /// <summary>
        /// Insert Or Update Store Business Link .
        /// UI Reffered -Store Business Link

        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateStoreBusinessLink(DO_StoreBusinessLink obj)
        {
            var msg = await _StoreMasterRepository.InsertOrUpdateStoreBusinessLink(obj);
            return Ok(msg);
        }
        #endregion  Store Business Link

        #region Store Form Link
        [HttpGet]
        public async Task<IActionResult> GetFormForStorelinking()
        {
            var ds = await _StoreMasterRepository.GetFormForStorelinking();
            return Ok(ds);
        }

        [HttpGet]
        public async Task<IActionResult> GetStoreFormLinked(int formId)
        {
            var ds = await _StoreMasterRepository.GetStoreFormLinked(formId);
            return Ok(ds);
        }

        [HttpPost]
        public async Task<IActionResult> InsertIntoFormStoreLink(List<DO_StoreFormLink> l_obj)
        {
            var msg = await _StoreMasterRepository.InsertIntoFormStoreLink(l_obj);
            return Ok(msg);
        }
        #endregion
    }
}
