namespace IntelligentDocumentProcessor.Application.Interfaces;

public interface IEmbeddingService
{
    /// <summary>
    /// Generates a vector embedding for the given text.
    /// </summary>
    /// <param name="text">The text to generate an embedding for.</param>
    /// <returns>A float array representing the embedding vector.</returns>
    Task<float[]> GenerateEmbeddingAsync(string text);
}
