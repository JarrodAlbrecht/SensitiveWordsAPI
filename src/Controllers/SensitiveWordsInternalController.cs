using Microsoft.AspNetCore.Mvc;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Services;

namespace SensitiveWordsAPI.Controllers
{
    [Route("api/internal/[controller]")]
    [ApiController]
    public class SensitiveWordsInternalController : Controller
    {
        private ISensitiveWordsService _sensitiveWordsService;
        public SensitiveWordsInternalController(ISensitiveWordsService sensitiveWordsService)
        {
            _sensitiveWordsService = sensitiveWordsService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensitiveWordById(int id)
        {
            var response = await _sensitiveWordsService.GetSensitiveWordById(id);
            return Ok(response);
        }

        [HttpPost(), HttpPut()]
        public async Task<IActionResult> UpsertSensitiveWord([FromBody] ManageSensitiveWordsRequest manageSensitiveWords)
        {
            var response = await _sensitiveWordsService.UpsertSensitiveWord(manageSensitiveWords.ManageSensitiveWords);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSensitiveWord([FromBody] ManageSensitiveWordsRequest manualSensitiveWords)
        {
            var response = await _sensitiveWordsService.DeleteSensitiveWord(manualSensitiveWords.ManageSensitiveWords);
            return Ok(response);
        }
    }
}
