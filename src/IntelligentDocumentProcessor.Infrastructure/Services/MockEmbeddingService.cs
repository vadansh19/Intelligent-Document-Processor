using IntelligentDocumentProcessor.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace IntelligentDocumentProcessor.Infrastructure.Services;

/// <summary>
/// Mock embedding service that returns random vectors.
/// Replace with OpenAI / Azure OpenAI embedding implementation when ready.
/// </summary>
public class MockEmbeddingService : IEmbeddingService
{
    private readonly ILogger<MockEmbeddingService> _logger;
    private static readonly Random _random = new();

    public MockEmbeddingService(ILogger<MockEmbeddingService> logger)
    {
        _logger = logger;
    }

    public Task<float[]> GenerateEmbeddingAsync(string text)
    {
        _logger.LogInformation("MockEmbeddingService: Generating embedding for text of length {Length}.", text.Length);

        // Generate a mock 1536-dimensional embedding (same as OpenAI text-embedding-ada-002)
        const int dimensions = 1536;
        var embedding = new float[dimensions];
        for (int i = 0; i < dimensions; i++)
        {
            embedding[i] = (float)(_random.NextDouble() * 2 - 1); // Range: -1 to 1
        }

        _logger.LogInformation("MockEmbeddingService: Generated {Dimensions}-dimensional embedding.", dimensions);

        return Task.FromResult(embedding);
    }
}
