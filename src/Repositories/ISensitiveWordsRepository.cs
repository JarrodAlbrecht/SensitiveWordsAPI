namespace SensitiveWordsAPI.Repositories
{
    public interface ISensitiveWordsRepository
    {
        public Task<IEnumerable<string>> GetAllSensitiveWords();
        public Task<string> GetSensitiveWordById(int id);
        public Task<string> UpsertSensitiveWord(string word);
        public Task<string> DeleteSensitiveWord(string word);
    }
}