using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SensitiveWordsAPI.Helpers;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Repositories;
using System.Text;
using System.Text.RegularExpressions;

namespace SensitiveWordsAPI.Services
{
    public class SensitiveWordsService : ISensitiveWordsService
    {
        ISensitiveWordsRepository _sensitiveWordsRepository;
        private IEnumerable<string> _sensitiveWords;
        private const string MemoryKey = "MemoryKey";
        private IOptions<MemoryCacheConfiguration> _memoryCacheConfiguration;
        MemoryCacheHelper _memoryCache;

        public SensitiveWordsService(ISensitiveWordsRepository sensitiveWordsRepository, MemoryCacheHelper memoryCache, IOptions<MemoryCacheConfiguration> memoryCacheOptions)
        {
            _sensitiveWordsRepository = sensitiveWordsRepository;
            _memoryCacheConfiguration = memoryCacheOptions;
            _memoryCache = memoryCache;
        }

        public async Task<SanitizedStringResponse> SanitizeClientInputAsync(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new SanitizedStringResponse() { SanitizedString = input };
            }

            if (!_memoryCache.TryGetValue(MemoryKey, out _sensitiveWords))
            {
                _sensitiveWords = await _sensitiveWordsRepository.GetAllSensitiveWordsAsync();

                _memoryCache.SetMemoryCache(MemoryKey, _sensitiveWords, _memoryCacheConfiguration.Value.MemoryCacheExpiryInMinutes);
            }


            return await SanitizeStringAsync(input);
        }

        public async Task<ManageSensitiveWordsResponse> GetSensitiveWordByIdAsync(int id)
        {
            if (id < 0)
                return new ManageSensitiveWordsResponse();

            var repositoryResponse = await _sensitiveWordsRepository.GetSensitiveWordByIdAsync(id);

            if (repositoryResponse == null)
                return new ManageSensitiveWordsResponse();
            else
                return new ManageSensitiveWordsResponse()
                {
                    SensitiveWordsResponse = repositoryResponse
                };
        }

        public async Task<IEnumerable<string>> GetAllSensitiveWordsAsync()
        {
            if (!_memoryCache.TryGetValue(MemoryKey, out _sensitiveWords))
            {
                _sensitiveWords = await _sensitiveWordsRepository.GetAllSensitiveWordsAsync();
                _memoryCache.SetMemoryCache(MemoryKey, _sensitiveWords, _memoryCacheConfiguration.Value.MemoryCacheExpiryInMinutes);
            }

            if (_sensitiveWords == null || !_sensitiveWords.Any())
                return Enumerable.Empty<string>();
            else
                return _sensitiveWords;
        }

        public async Task<ManageSensitiveWordsResponse> UpsertSensitiveWordAsync(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return new ManageSensitiveWordsResponse();

            var respositoryResponse = await _sensitiveWordsRepository.UpsertSensitiveWordAsync(word);

            if (!string.IsNullOrEmpty(respositoryResponse))
            {
                var allWords = await _sensitiveWordsRepository.GetAllSensitiveWordsAsync();
                _memoryCache.SetMemoryCache(MemoryKey, allWords, _memoryCacheConfiguration.Value.MemoryCacheExpiryInMinutes);
            }

            return new ManageSensitiveWordsResponse
            {
                SensitiveWordsResponse = respositoryResponse
            };
        }

        public async Task<ManageSensitiveWordsResponse> DeleteSensitiveWordAsync(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return new ManageSensitiveWordsResponse();

            var respositoryResponse = await _sensitiveWordsRepository.DeleteSensitiveWordAsync(word);

            if (!string.IsNullOrEmpty(respositoryResponse))
            {
                var allWords = await _sensitiveWordsRepository.GetAllSensitiveWordsAsync();
                _memoryCache.SetMemoryCache(MemoryKey, allWords, _memoryCacheConfiguration.Value.MemoryCacheExpiryInMinutes);
            }

            return new ManageSensitiveWordsResponse {
                SensitiveWordsResponse = respositoryResponse
            };
        }

        private async Task<SanitizedStringResponse> SanitizeStringAsync(string input)
        {
            string sanitized = input;

            var orderedWords = _sensitiveWords.OrderByDescending(p => p.Length).ToList();
            string pattern = string.Join("|", orderedWords.Select(Regex.Escape));
            string regexPattern = $@"\b({pattern})\b";

            sanitized = Regex.Replace(sanitized, regexPattern, match =>
            {
                var original = match.Value;
                var replaced = new StringBuilder();

                foreach (char c in original)
                {
                    replaced.Append(char.IsWhiteSpace(c) ? c : '*');
                }

                return replaced.ToString();
            }, RegexOptions.IgnoreCase);

            return await Task.FromResult(new SanitizedStringResponse
            {
                SanitizedString = sanitized
            });
        }
    }
}
