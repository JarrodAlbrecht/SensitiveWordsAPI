using SensitiveWordsAPI.Models;

namespace SensitiveWordsAPI.Services
{
    public interface ISensitiveWordsService
    {
        public Task<SanitizedStringResponse> SanitizeClientInput(string input);
        public Task<ManageSensitiveWordsResponse> GetSensitiveWordById(int id);
        public Task<ManageSensitiveWordsResponse> UpsertSensitiveWord(string word);
        public Task<ManageSensitiveWordsResponse> DeleteSensitiveWord(string word);
    }
}
