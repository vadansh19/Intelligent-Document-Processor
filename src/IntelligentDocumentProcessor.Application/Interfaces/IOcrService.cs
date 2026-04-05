namespace IntelligentDocumentProcessor.Application.Interfaces;

public interface IOcrService
{
    /// <summary>
    /// Extracts text from a document file at the given path.
    /// </summary>
    /// <param name="filePath">The path to the document file.</param>
    /// <returns>The extracted text content.</returns>
    Task<string> ExtractTextAsync(string filePath);
}
