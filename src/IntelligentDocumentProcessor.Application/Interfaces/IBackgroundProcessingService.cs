namespace IntelligentDocumentProcessor.Application.Interfaces;

public interface IBackgroundProcessingService
{
    /// <summary>
    /// Enqueues a document for background processing (OCR, chunking, embedding).
    /// </summary>
    /// <param name="documentId">The ID of the document to process.</param>
    Task EnqueueDocumentProcessingAsync(Guid documentId);
}
