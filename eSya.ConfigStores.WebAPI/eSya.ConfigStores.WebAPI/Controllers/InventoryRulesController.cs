using eSya.ConfigStores.DO;
using eSya.ConfigStores.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigStores.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryRulesController : ControllerBase
    {
        private readonly IInventoryRulesRepository _rulesRepository;
        public InventoryRulesController(IInventoryRulesRepository rulesRepository)
        {
            _rulesRepository = rulesRepository;
        }

        #region Inventory Rules
        /// <summary>
        /// Getting  Inventory Rules List.
        /// UI Reffered - Inventory Rules Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetInventoryRules()
        {
            var inv_rules = await _rulesRepository.GetInventoryRules();
            return Ok(inv_rules);
        }
        /// <summary>
        /// Insert Inventory Rules .
        /// UI Reffered -Inventory Rules
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertInventoryRule(DO_InventoryRules inventoryRule)
        {
            var msg = await _rulesRepository.InsertInventoryRule(inventoryRule);
            return Ok(msg);

        }
        /// <summary>
        /// Update Inventory Rules .
        /// UI Reffered -Inventory Rules
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateInventoryRule(DO_InventoryRules inventoryRule)
        {
            var msg = await _rulesRepository.UpdateInventoryRule(inventoryRule);
            return Ok(msg);

        }
        /// <summary>
        /// Active Or De Active Inventory Rule.
        /// UI Reffered - Inventory Rule
        /// </summary>
        /// <param name="status-InventoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveInventoryRules(bool status, string InventoryId)
        {
            var msg = await _rulesRepository.ActiveOrDeActiveInventoryRules(status, InventoryId);
            return Ok(msg);
        }
        #endregion
    }
}
