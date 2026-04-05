using System.Text.Json;
using IntelligentDocumentProcessor.Application.Interfaces;
using IntelligentDocumentProcessor.Domain.Entities;
using IntelligentDocumentProcessor.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntelligentDocumentProcessor.Infrastructure.Services;

/// <summary>
/// Mock background processing service using Task.Run for development.
/// Replace with Hangfire, RabbitMQ, or Azure Service Bus in production.
/// </summary>
public class MockBackgroundProcessingService : IBackgroundProcessingService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MockBackgroundProcessingService> _logger;

    public MockBackgroundProcessingService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<MockBackgroundProcessingService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public Task EnqueueDocumentProcessingAsync(Guid documentId)
    {
        _logger.LogInformation("Enqueuing document {DocumentId} for background processing.", documentId);

        // Fire-and-forget using Task.Run (mock implementation)
        // In production, this would enqueue to a message broker or job scheduler
        _ = Task.Run(async () =>
        {
            try
            {
                await ProcessDocumentAsync(documentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Background processing failed for document {DocumentId}.", documentId);
            }
        });

        return Task.CompletedTask;
    }

    private async Task ProcessDocumentAsync(Guid documentId)
    {
        // Create a new scope for DI resolution (background tasks need their own scope)
        using var scope = _serviceScopeFactory.CreateScope();
        var documentRepository = scope.ServiceProvider.GetRequiredService<IDocumentRepository>();
        var ocrService = scope.ServiceProvider.GetRequiredService<IOcrService>();
        var embeddingService = scope.ServiceProvider.GetRequiredService<IEmbeddingService>();

        var document = await documentRepository.GetByIdAsync(documentId);
        if (document == null)
        {
            _logger.LogWarning("Document {DocumentId} not found for processing.", documentId);
            return;
        }

        try
        {
            // Step 1: Update status to Processing
            document.Status = DocumentStatus.Processing;
            await documentRepository.UpdateAsync(document);
            _logger.LogInformation("Document {DocumentId}: Status set to Processing.", documentId);

            // Simulate processing delay
            await Task.Delay(1000);

            // Step 2: OCR - Extract text
            var extractedText = await ocrService.ExtractTextAsync(document.StorageUrl);
            _logger.LogInformation("Document {DocumentId}: OCR extracted {Length} characters.", documentId, extractedText.Length);

            // Step 3: Split text into chunks
            var textChunks = SplitTextIntoChunks(extractedText, maxChunkSize: 500);
            _logger.LogInformation("Document {DocumentId}: Split into {ChunkCount} chunks.", documentId, textChunks.Count);

            // Step 4: Generate embeddings and create chunk entities
            var documentChunks = new List<DocumentChunk>();
            foreach (var chunkText in textChunks)
            {
                var embedding = await embeddingService.GenerateEmbeddingAsync(chunkText);
                documentChunks.Add(new DocumentChunk
                {
                    Id = Guid.NewGuid(),
                    DocumentId = documentId,
                    Text = chunkText,
                    Embedding = JsonSerializer.Serialize(embedding)
                });
            }

            // Step 5: Save chunks
            await documentRepository.AddChunksAsync(documentChunks);
            _logger.LogInformation("Document {DocumentId}: Saved {ChunkCount} chunks with embeddings.", documentId, documentChunks.Count);

            // Step 6: Update status to Completed
            document.Status = DocumentStatus.Completed;
            await documentRepository.UpdateAsync(document);
            _logger.LogInformation("Document {DocumentId}: Processing completed successfully.", documentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing document {DocumentId}. Setting status to Failed.", documentId);

            // Retry strategy: for now, just mark as failed
            // In production, implement exponential backoff with max retries
            document.Status = DocumentStatus.Failed;
            await documentRepository.UpdateAsync(document);
        }
    }

    /// <summary>
    /// Splits text into chunks of a maximum size, trying to break at sentence boundaries.
    /// </summary>
    private static List<string> SplitTextIntoChunks(string text, int maxChunkSize)
    {
        var chunks = new List<string>();
        if (string.IsNullOrWhiteSpace(text))
            return chunks;

        var sentences = text.Split(new[] { ". ", ".\n", ".\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        var currentChunk = string.Empty;

        foreach (var sentence in sentences)
        {
            var trimmedSentence = sentence.Trim();
            if (string.IsNullOrEmpty(trimmedSentence))
                continue;

            if (currentChunk.Length + trimmedSentence.Length + 2 > maxChunkSize && currentChunk.Length > 0)
            {
                chunks.Add(currentChunk.Trim());
                currentChunk = string.Empty;
            }

            currentChunk += trimmedSentence + ". ";
        }

        if (!string.IsNullOrWhiteSpace(currentChunk))
            chunks.Add(currentChunk.Trim());

        return chunks;
    }
}
