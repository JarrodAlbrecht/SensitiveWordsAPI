using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Services;
using System.Diagnostics.CodeAnalysis;

namespace SensitiveWordsAPI.Controllers
{
    /// <summary>
    /// Provides API endpoints for sanitizing client input by removing or replacing sensitive words.
    /// </summary>
    /// <remarks>This controller is responsible for handling client requests to sanitize input data.  It uses
    /// the <see cref="ISensitiveWordsService"/> to process the input and return sanitized results.</remarks>
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SensitiveWordsClientController : ControllerBase
    {
        private ISensitiveWordsService _sensitiveWordsService;
        public SensitiveWordsClientController(ISensitiveWordsService sensitiveWordsService)
        {
            _sensitiveWordsService = sensitiveWordsService;
        }

        /// <summary>
        /// Processes and sanitizes the client-provided input to remove sensitive or restricted content.
        /// </summary>
        /// <remarks>This method uses the <c>_sensitiveWordsService</c> to sanitize the input text by
        /// removing or replacing sensitive words. The sanitized content is returned to the client for further
        /// use.</remarks>
        /// <param name="clientInput">The client input request containing the text to be sanitized. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the sanitized version of the client input. The result is returned
        /// as an HTTP 200 OK response with the sanitized content.</returns>
        [HttpPost]
        public async Task<IActionResult> SanitizeClientRequest([FromBody] ClientInputRequest clientInput)
        {
            var result = await _sensitiveWordsService.SanitizeClientInputAsync(clientInput.ClientInput);
            return Ok(result);
        }
    }
}
