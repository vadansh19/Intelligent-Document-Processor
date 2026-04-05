using IntelligentDocumentProcessor.Application.DTOs;
using IntelligentDocumentProcessor.Application.Interfaces;
using IntelligentDocumentProcessor.Domain.Entities;
using IntelligentDocumentProcessor.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace IntelligentDocumentProcessor.Application.Services;

public class DocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IBackgroundProcessingService _backgroundProcessingService;
    private readonly ILogger<DocumentService> _logger;

    public DocumentService(
        IDocumentRepository documentRepository,
        IBackgroundProcessingService backgroundProcessingService,
        ILogger<DocumentService> logger)
    {
        _documentRepository = documentRepository;
        _backgroundProcessingService = backgroundProcessingService;
        _logger = logger;
    }

    public async Task<UploadDocumentResponseDto> UploadDocumentAsync(string fileName, string storagePath)
    {
        _logger.LogInformation("Registering document upload: {FileName}", fileName);

        var document = new Document
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            StorageUrl = storagePath,
            Status = DocumentStatus.Uploaded,
            CreatedAt = DateTime.UtcNow
        };

        await _documentRepository.AddAsync(document);

        _logger.LogInformation("Document {DocumentId} registered. Enqueuing for background processing.", document.Id);

        // Fire-and-forget background processing
        await _backgroundProcessingService.EnqueueDocumentProcessingAsync(document.Id);

        return new UploadDocumentResponseDto
        {
            DocumentId = document.Id,
            FileName = document.FileName,
            Status = document.Status.ToString(),
            Message = "Document uploaded successfully and queued for processing."
        };
    }

    public async Task<DocumentDto?> GetDocumentStatusAsync(Guid documentId)
    {
        _logger.LogInformation("Fetching status for document: {DocumentId}", documentId);

        var document = await _documentRepository.GetByIdAsync(documentId);
        if (document == null)
        {
            _logger.LogWarning("Document {DocumentId} not found.", documentId);
            return null;
        }

        return new DocumentDto
        {
            Id = document.Id,
            FileName = document.FileName,
            StorageUrl = document.StorageUrl,
            Status = document.Status,
            CreatedAt = document.CreatedAt
        };
    }

    public async Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync()
    {
        var documents = await _documentRepository.GetAllAsync();
        return documents.Select(d => new DocumentDto
        {
            Id = d.Id,
            FileName = d.FileName,
            StorageUrl = d.StorageUrl,
            Status = d.Status,
            CreatedAt = d.CreatedAt
        });
    }
}
