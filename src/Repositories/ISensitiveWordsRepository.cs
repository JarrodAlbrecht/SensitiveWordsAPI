namespace SensitiveWordsAPI.Repositories
{
    public interface ISensitiveWordsRepository
    {
        public Task<IEnumerable<string>> GetAllSensitiveWordsAsync();
        public Task<string> GetSensitiveWordByIdAsync(int id);
        public Task<string> UpsertSensitiveWordAsync(string word);
        public Task<string> DeleteSensitiveWordAsync(string word);
    }
}