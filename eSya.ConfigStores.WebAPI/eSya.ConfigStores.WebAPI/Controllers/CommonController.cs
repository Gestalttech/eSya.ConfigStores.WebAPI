using eSya.ConfigStores.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigStores.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonRepository _commonRepository;
        public CommonController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        /// <summary>
        /// Getting  Item Group.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetItemGroup()
        {
            var ds = await _commonRepository.GetItemGroup();
            return Ok(ds);
        }

        /// <summary>
        /// Getting  Item Category.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetItemCategory(int ItemGroup)
        {
            var ds = await _commonRepository.GetItemCategory(ItemGroup);
            return Ok(ds);
        }

        /// <summary>
        /// Getting  Item Sub Category.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetItemSubCategory(int ItemCategory)
        {
            var ds = await _commonRepository.GetItemSubCategory(ItemCategory);
            return Ok(ds);
        }

        /// <summary>
        /// Getting  Item Category.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetItemCategoryForItemGroup(int ItemGroup)
        {
            var ds = await _commonRepository.GetItemCategoryForItemGroup(ItemGroup);
            return Ok(ds);
        }


        /// <summary>
        /// Getting  Business Key.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var ds = await _commonRepository.GetBusinessKey();
            return Ok(ds);
        }
    }
}
