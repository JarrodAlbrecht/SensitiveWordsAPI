using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Services;

namespace SensitiveWordsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensitiveWordsClientController : ControllerBase
    {
        private ISensitiveWordsService _sensitiveWordsService;
        public SensitiveWordsClientController(ISensitiveWordsService sensitiveWordsService)
        {
            _sensitiveWordsService = sensitiveWordsService;
        }

        [HttpPost]
        public async Task<IActionResult> SanitizeClientRequest([FromBody] ClientInputRequest clientInput)
        {
            var result = await _sensitiveWordsService.SanitizeClientInput(clientInput.ClientInput);
            return Ok(result);
        }
    }
}
