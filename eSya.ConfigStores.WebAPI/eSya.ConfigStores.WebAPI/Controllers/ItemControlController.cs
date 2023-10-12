using eSya.ConfigStores.DO;
using eSya.ConfigStores.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigStores.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemControlController : ControllerBase
    {
        private readonly IItemControlRepository _ItemControlRepository;
        public ItemControlController(IItemControlRepository itemControlRepository)
        {
            _ItemControlRepository = itemControlRepository;
        }


        #region ItemGroup
        [HttpGet]
        public async Task<IActionResult> GetItemGroup()
        {
            var ac = await _ItemControlRepository.GetItemGroup();
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetItemGroupByID(int ItemGroupID)
        {
            var ac = await _ItemControlRepository.GetItemGroupByID(ItemGroupID);
            return Ok(ac);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateItemGroup(DO_ItemGroup obj)
        {
            var msg = await _ItemControlRepository.AddOrUpdateItemGroup(obj);
            return Ok(msg);
        }

        #endregion

        #region ItemCategory
        [HttpGet]
        public async Task<IActionResult> GetItemCategory()
        {
            var ac = await _ItemControlRepository.GetItemCategory();
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetItemCategoryByID(int ItemCategory)
        {
            var ac = await _ItemControlRepository.GetItemCategoryByID(ItemCategory);
            return Ok(ac);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateItemCategory(DO_ItemCategory obj)
        {
            var msg = await _ItemControlRepository.AddOrUpdateItemCategory(obj);
            return Ok(msg);
        }
        #endregion

        #region SubCategory
        [HttpGet]
        public async Task<IActionResult> GetItemSubCategoryByCateID(int ItemCategory)
        {
            var ac = await _ItemControlRepository.GetItemSubCategoryByCateID(ItemCategory);
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetItemSubCategoryByID(int ItemSubCategory)
        {
            var ac = await _ItemControlRepository.GetItemSubCategoryByID(ItemSubCategory);
            return Ok(ac);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateItemSubCategory(DO_ItemSubCategory obj)
        {
            var msg = await _ItemControlRepository.AddOrUpdateItemSubCategory(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> GetItemSubCategories()
        {
            var ac = await _ItemControlRepository.GetItemSubCategories();
            return Ok(ac);
        }
        #endregion

        #region ItemGroupCategoryMapping
        [HttpGet]
        public async Task<IActionResult> GetItemCategoryByItemGroupID(int ItemGroup)
        {

            var ac = await _ItemControlRepository.GetItemCategoryByItemGroupID(ItemGroup);
            return Ok(ac);

        }
        [HttpGet]
        public async Task<IActionResult> GetItemCategoryByItemGroupCategory(int ItemGroup, int ItemCategory)
        {
            var ac = await _ItemControlRepository.GetItemCategoryByItemGroupCategory(ItemGroup, ItemCategory);
            return Ok(ac);
        }
        [HttpPost]
        public async Task<IActionResult> ItemGroupCateSubCateMapping(DO_ItemGroupCategory obj)
        {
            var msg = await _ItemControlRepository.ItemGroupCateSubCateMapping(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> GetMappinRecord(int ItemGroupID, int ItemCategory, int ItemSubCategory)
        {
            var ac = await _ItemControlRepository.GetMappinRecord(ItemGroupID, ItemCategory, ItemSubCategory);
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetItemCategoriesByItemGroup()
        {
            var ac = await _ItemControlRepository.GetItemCategoriesByItemGroup();
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetItemSubCategoriesByItemGroupCategory()
        {
            var ac = await _ItemControlRepository.GetItemSubCategoriesByItemGroupCategory();
            return Ok(ac);
        }
        #endregion
    }
}
