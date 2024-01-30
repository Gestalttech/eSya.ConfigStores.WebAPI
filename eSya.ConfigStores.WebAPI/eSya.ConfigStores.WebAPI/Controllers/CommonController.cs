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
