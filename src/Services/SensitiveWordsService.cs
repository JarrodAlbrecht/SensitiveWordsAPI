using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Repositories;
using System.Text.RegularExpressions;

namespace SensitiveWordsAPI.Services
{
    public class SensitiveWordsService : ISensitiveWordsService
    {
        ISensitiveWordsRepository _sensitiveWordsRepository;
        IMemoryCache _memoryCache;
        private IEnumerable<string> _sensitiveWords;
        private const string MemoryKey = "MemoryKey";
        private IOptions<MemoryCacheConfiguration> _memoryCacheConfiguration;

        public SensitiveWordsService(ISensitiveWordsRepository sensitiveWordsRepository, IMemoryCache memoryCache, IOptions<MemoryCacheConfiguration> memoryCacheOptions)
        {
            _sensitiveWordsRepository = sensitiveWordsRepository;
            _memoryCache = memoryCache;
            _memoryCacheConfiguration = memoryCacheOptions;
        }

        public async Task<SanitizedStringResponse> SanitizeClientInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new SanitizedStringResponse() { SanitizedString = input };
            }

            if (!_memoryCache.TryGetValue(MemoryKey, out _sensitiveWords))
            {
                _sensitiveWords = await _sensitiveWordsRepository.GetAllSensitiveWords();

                _memoryCache?.Set(MemoryKey, _sensitiveWords, TimeSpan.FromMinutes(_memoryCacheConfiguration.Value.MemoryCacheExpiryInMinutes));
            }


            return await SanitizeString(input);
        }

        public async Task<ManageSensitiveWordsResponse> GetSensitiveWordById(int id)
        {
            if (id < 0)
                return new ManageSensitiveWordsResponse();

            var repositoryResponse = await _sensitiveWordsRepository.GetSensitiveWordById(id);

            if (repositoryResponse == null)
                return new ManageSensitiveWordsResponse();
            else
                return new ManageSensitiveWordsResponse()
                {
                    SensitiveWordsResponse = repositoryResponse
                };
        }

        public async Task<ManageSensitiveWordsResponse> UpsertSensitiveWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return new ManageSensitiveWordsResponse();

            return new ManageSensitiveWordsResponse
            {
                SensitiveWordsResponse = await _sensitiveWordsRepository.UpsertSensitiveWord(word)
            };
        }

        public async Task<ManageSensitiveWordsResponse> DeleteSensitiveWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return new ManageSensitiveWordsResponse();

            return new ManageSensitiveWordsResponse {
                SensitiveWordsResponse = await _sensitiveWordsRepository.DeleteSensitiveWord(word)
            };
        }

        private async Task<SanitizedStringResponse> SanitizeString(string input)
        {
            string sanitized = input;

            foreach (var word in _sensitiveWords)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                string escapedWord = Regex.Escape(word);
                string pattern = $@"\b{escapedWord}\b"; // \b ensures whole-word matches
                string replacement = new string('*', word.Length);

                sanitized = Regex.Replace(sanitized, pattern, replacement, RegexOptions.IgnoreCase);
            }

            return await Task.FromResult(new SanitizedStringResponse
            {
                SanitizedString = sanitized
            });
        }
    }
}
