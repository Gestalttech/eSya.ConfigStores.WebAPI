using eSya.ConfigStores.DL.Repository;
using eSya.ConfigStores.DO;
using eSya.ConfigStores.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigStores.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SACCategoryController : ControllerBase
    {
        private readonly ISACCategoryRepository _SACCategoryRepository;
        public SACCategoryController(ISACCategoryRepository SACCategoryRepository)
        {
            _SACCategoryRepository = SACCategoryRepository;
        }
        #region SAC Category
        /// <summary>
        /// Getting  SAC Category List.
        /// UI Reffered - SAC Category
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSACCategories(int ISDCode)
        {
            var ds = await _SACCategoryRepository.GetSACCategories(ISDCode);
            return Ok(ds);
        }
        /// <summary>
        /// Getting  SAC Category by ID.
        /// UI Reffered - SAC Category
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSACCategoryByCategoryID(int ISDCode, string SACClassID, string SACCategoryID)
        {
            var ds = await _SACCategoryRepository.GetSACCategoryByCategoryID(ISDCode, SACClassID, SACCategoryID);
            return Ok(ds);
        }
        /// <summary>
        /// Insert Into SAC Category.
        /// UI Reffered -SAC Category
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoSACCategory(DO_SACCategory obj)
        {
            var msg = await _SACCategoryRepository.InsertIntoSACCategory(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Update Into SAC Category.
        /// UI Reffered -SAC Category
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSACCategory(DO_SACCategory obj)
        {
            var msg = await _SACCategoryRepository.UpdateSACCategory(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Delete  SAC Category by ID.
        /// UI Reffered - SAC Category
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteSACCategory(int ISDCode, string SACClassID, string SACCategoryID)
        {
            var ds = await _SACCategoryRepository.DeleteSACCategory(ISDCode, SACClassID, SACCategoryID);
            return Ok(ds);
        }
        #endregion
    }
}
