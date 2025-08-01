using Microsoft.AspNetCore.Mvc;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Services;
using System.Diagnostics.CodeAnalysis;

namespace SensitiveWordsAPI.Controllers
{
    /// <summary>
    /// Provides internal API endpoints for managing sensitive words.
    /// </summary>
    /// <remarks>This controller is intended for internal use only and exposes operations for retrieving, 
    /// creating, updating, and deleting sensitive words. It interacts with the <see cref="ISensitiveWordsService"/>  to
    /// perform these operations.</remarks>
    [Route("api/internal/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SensitiveWordsInternalController : Controller
    {
        private ISensitiveWordsService _sensitiveWordsService;
        public SensitiveWordsInternalController(ISensitiveWordsService sensitiveWordsService)
        {
            _sensitiveWordsService = sensitiveWordsService;
        }

        /// <summary>
        /// Retrieves a sensitive word by its unique identifier.
        /// </summary>
        /// <remarks>This method returns an HTTP 200 response with the sensitive word data if the word is
        /// found. If the specified <paramref name="id"/> does not correspond to an existing sensitive word,  an
        /// appropriate HTTP error response (e.g., 404 Not Found) is returned.</remarks>
        /// <param name="id">The unique identifier of the sensitive word to retrieve. Must be a positive integer.</param>
        /// <returns>An <see cref="IActionResult"/> containing the sensitive word if found, or an appropriate HTTP response if
        /// not.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensitiveWordByIdAsync(int id)
        {
            var response = await _sensitiveWordsService.GetSensitiveWordByIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all sensitive words from the system.
        /// </summary>
        /// <remarks>This method returns a collection of sensitive words managed by the system.  The
        /// response is typically used for administrative or moderation purposes.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of sensitive words.  The response is returned with an
        /// HTTP 200 status code if successful.</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllSensitiveWordsAsync()
        {
            var response = await _sensitiveWordsService.GetAllSensitiveWordsAsync();
            return Ok(response);
        }

        /// <summary>
        /// Adds a new sensitive word or updates an existing one in the system.
        /// </summary>
        /// <remarks>This method supports both HTTP POST and PUT operations. Use POST to add a new
        /// sensitive word  and PUT to update an existing one. The behavior is determined by the content of the
        /// request.</remarks>
        /// <param name="manageSensitiveWords">The request containing the sensitive word details to be added or updated.  This parameter cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation.  Typically, this will be an HTTP 200
        /// response with the operation result.</returns>
        [HttpPost(), HttpPut()]
        public async Task<IActionResult> UpsertSensitiveWordAsync([FromBody] ManageSensitiveWordsRequest manageSensitiveWords)
        {
            var response = await _sensitiveWordsService.UpsertSensitiveWordAsync(manageSensitiveWords.ManageSensitiveWords);
            return Ok(response);
        }

        /// <summary>
        /// Deletes a sensitive word or a list of sensitive words from the system.
        /// </summary>
        /// <remarks>This method is used to remove sensitive words from the system. The request body must
        /// include the word(s) to be deleted. Ensure that the provided data is valid and formatted correctly.</remarks>
        /// <param name="manualSensitiveWords">An object containing the sensitive word(s) to be deleted. This parameter cannot be null and must include
        /// valid data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically returns an HTTP 200 OK
        /// response with the operation result.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteSensitiveWordAsync([FromBody] ManageSensitiveWordsRequest manualSensitiveWords)
        {
            var response = await _sensitiveWordsService.DeleteSensitiveWordAsync(manualSensitiveWords.ManageSensitiveWords);
            return Ok(response);
        }
    }
}
