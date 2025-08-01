using SensitiveWordsAPI.Models;

namespace SensitiveWordsAPI.Services
{
    public interface ISensitiveWordsService
    {
        public Task<SanitizedStringResponse> SanitizeClientInputAsync(string input);
        public Task<ManageSensitiveWordsResponse> GetSensitiveWordByIdAsync(int id);
        public Task<IEnumerable<string>> GetAllSensitiveWordsAsync();
        public Task<ManageSensitiveWordsResponse> UpsertSensitiveWordAsync(string word);
        public Task<ManageSensitiveWordsResponse> DeleteSensitiveWordAsync(string word);
    }
}
