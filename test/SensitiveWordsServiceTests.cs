using AwesomeAssertions;
using AwesomeAssertions.Execution;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NSubstitute;
using SensitiveWordsAPI.Helpers;
using SensitiveWordsAPI.Models;
using SensitiveWordsAPI.Repositories;
using SensitiveWordsAPI.Services;

namespace test;

public class SensitiveWordsServiceTests
{
    private ISensitiveWordsRepository _repository;
    private IMemoryCache _memoryCacheMock;
    private MemoryCacheHelper _memoryCache;
    private IOptions<MemoryCacheConfiguration> _options;
    private SensitiveWordsService _service;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<ISensitiveWordsRepository>();
        _memoryCacheMock = Substitute.For<IMemoryCache>();
        _memoryCache = new MemoryCacheHelper(_memoryCacheMock);
        _options = Options.Create(new MemoryCacheConfiguration { MemoryCacheExpiryInMinutes = 10 });
        _service = new SensitiveWordsService(_repository, _memoryCache, _options);
    }

    [Test]
    public async Task SanitizeClientInputAsync_Should_Sanitize_Matching_SensitiveWords()
    {
        // Arrange
        var input = "select * from pudding";
        var sensitiveWords = new List<string> { "select * from" };

        _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<IEnumerable<string>>())
            .Returns(x =>
            {
                x[1] = null!;
                return false;
            });

        _repository.GetAllSensitiveWordsAsync().Returns(sensitiveWords);

        // Act
        var result = await _service.SanitizeClientInputAsync(input);

        // Assert
        using (new AssertionScope())
        {
            result.SanitizedString.Should().Be("****** * **** pudding");
        }
    }

    [Test]
    public async Task GetSensitiveWordByIdAsync_Should_Return_Valid_Response()
    {
        // Arrange
        var expected = "SELECT";
        _repository.GetSensitiveWordByIdAsync(1).Returns(expected);

        // Act
        var result = await _service.GetSensitiveWordByIdAsync(1);

        // Assert
        using (new AssertionScope())
        {
            result.SensitiveWordsResponse.Should().Be("SELECT");
        }
    }

    [Test]
    public async Task GetAllSensitiveWordsAsync_Should_Load_From_Repository_If_Not_Cached()
    {
        // Arrange
        var words = new List<string> { "select", "drop" };

        _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<IEnumerable<string>>())
            .Returns(x =>
            {
                x[1] = null!;
                return false;
            });

        _repository.GetAllSensitiveWordsAsync().Returns(words);

        // Act
        var result = await _service.GetAllSensitiveWordsAsync();

        // Assert
        using (new AssertionScope())
        {
            result.Should().HaveCount(2);
            result.Should().Contain("select");
            result.Should().Contain("drop");
        }
    }

    [Test]
    public async Task UpsertSensitiveWordAsync_Should_Update_Cache_And_Return_Response()
    {
        // Arrange
        var word = "truncate";
        var updated = "truncate";
        var allWords = new List<string> { "truncate", "delete" };

        _repository.UpsertSensitiveWordAsync(word).Returns(updated);
        _repository.GetAllSensitiveWordsAsync().Returns(allWords);

        // Act
        var result = await _service.UpsertSensitiveWordAsync(word);

        // Assert
        using (new AssertionScope())
        {
            result.SensitiveWordsResponse.Should().Be(updated);
        }
    }

    [Test]
    public async Task DeleteSensitiveWordAsync_Should_Update_Cache_And_Return_Response()
    {
        // Arrange
        var word = "truncate";
        var deleted = "truncate";
        var allWords = new List<string> { "delete" };

        _repository.DeleteSensitiveWordAsync(word).Returns(deleted);
        _repository.GetAllSensitiveWordsAsync().Returns(allWords);

        // Act
        var result = await _service.DeleteSensitiveWordAsync(word);

        // Assert
        using (new AssertionScope())
        {
            result.SensitiveWordsResponse.Should().Be(deleted);
        }
    }

    [TearDown]
    public void TearDown()
    {
        _repository = null!;
        _memoryCacheMock.Dispose();
        _memoryCacheMock = null!;
        _memoryCache = null!;
        _options = null!;
        _service = null!;
    }
}
