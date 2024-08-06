using eSya.ConfigStores.DL.Repository;
using eSya.ConfigStores.DO;
using eSya.ConfigStores.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigStores.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SACClassController : ControllerBase
    {
        private readonly ISACClassRepository _SACClassRepository;
        public SACClassController(ISACClassRepository SACClassRepository)
        {
            _SACClassRepository = SACClassRepository;
        }
        #region Define SAC Class
        /// <summary>
        /// Getting  SAC Class List.
        /// UI Reffered - SAC Class
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSACClasses(int ISDCode)
        {
            var ds = await _SACClassRepository.GetSACClasses(ISDCode);
            return Ok(ds);
        }
        /// <summary>
        /// Getting  SAC Class by ID.
        /// UI Reffered - SAC Class
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSACClassByClassID(int ISDCode, string SACClassID)
        {
            var ds = await _SACClassRepository.GetSACClassByClassID(ISDCode,SACClassID);
            return Ok(ds);
        }
        /// <summary>
        /// Insert Into SAC Class.
        /// UI Reffered -SAC Class
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoSACClass(DO_SACClass obj)
        {
            var msg = await _SACClassRepository.InsertIntoSACClass(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Update Into SAC Class.
        /// UI Reffered -SAC Class
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSACClass(DO_SACClass obj)
        {
            var msg = await _SACClassRepository.UpdateSACClass(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Delete  SAC Class by ID.
        /// UI Reffered - SAC Class
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteSACClass(int ISDCode, string SACClassID)
        {
            var ds = await _SACClassRepository.DeleteSACClass(ISDCode, SACClassID);
            return Ok(ds);
        }
        #endregion
    }
}
