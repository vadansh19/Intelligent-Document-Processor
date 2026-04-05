using IntelligentDocumentProcessor.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace IntelligentDocumentProcessor.Infrastructure.Services;

/// <summary>
/// Mock OCR service that returns placeholder text.
/// Replace with AWS Textract implementation when ready.
/// </summary>
public class MockOcrService : IOcrService
{
    private readonly ILogger<MockOcrService> _logger;

    public MockOcrService(ILogger<MockOcrService> logger)
    {
        _logger = logger;
    }

    public Task<string> ExtractTextAsync(string filePath)
    {
        _logger.LogInformation("MockOcrService: Extracting text from {FilePath}", filePath);

        // Simulate OCR extraction with mock text
        var mockText = $"""
            This is mock-extracted text from document: {Path.GetFileName(filePath)}.

            Section 1: Introduction
            This document contains important information about the project requirements,
            technical specifications, and implementation guidelines.

            Section 2: Technical Details
            The system uses a microservices architecture with event-driven communication.
            Key components include the document processor, embedding generator, and query engine.

            Section 3: Conclusion
            The implementation follows best practices for scalability and maintainability.
            Future enhancements will include real-time processing and advanced analytics.
            """;

        _logger.LogInformation("MockOcrService: Successfully extracted {Length} characters.", mockText.Length);

        return Task.FromResult(mockText);
    }
}
