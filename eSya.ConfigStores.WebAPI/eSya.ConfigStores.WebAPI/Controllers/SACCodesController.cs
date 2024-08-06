using eSya.ConfigStores.DO;
using eSya.ConfigStores.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigStores.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SACCodesController : ControllerBase
    {
        private readonly ISACCodesRepository _SACCodesRepository;
        public SACCodesController(ISACCodesRepository SACCodesRepository)
        {
            _SACCodesRepository = SACCodesRepository;
        }
        #region SAC Category
        /// <summary>
        /// Getting  SAC Codes List.
        /// UI Reffered - SAC Codes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSACCodes(int ISDCode)
        {
            var ds = await _SACCodesRepository.GetSACCodes(ISDCode);
            return Ok(ds);
        }
        /// <summary>
        /// Getting  SAC Codes by ID.
        /// UI Reffered - SAC Codes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSACCodeByCode(int ISDCode, string SACClassID, string SACCategoryID, string SACCodeID)
        {
            var ds = await _SACCodesRepository.GetSACCodeByCode(ISDCode, SACClassID, SACCategoryID, SACCodeID);
            return Ok(ds);
        }
        /// <summary>
        /// Insert Into SAC Codes.
        /// UI Reffered -SAC Codes
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoSACCode(DO_SACCodes obj)
        {
            var msg = await _SACCodesRepository.InsertIntoSACCode(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Update Into SAC Codes.
        /// UI Reffered -SAC Codes
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSACSACCode(DO_SACCodes obj)
        {
            var msg = await _SACCodesRepository.UpdateSACSACCode(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Delete  SAC Codes by ID.
        /// UI Reffered - SAC Codes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteSACCode(int ISDCode, string SACClassID, string SACCategoryID, string SACCodeID)
        {
            var ds = await _SACCodesRepository.DeleteSACCode(ISDCode, SACClassID, SACCategoryID, SACCodeID);
            return Ok(ds);
        }
        #endregion
    }
}
