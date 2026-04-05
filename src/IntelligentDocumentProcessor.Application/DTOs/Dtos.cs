using IntelligentDocumentProcessor.Domain.Enums;

namespace IntelligentDocumentProcessor.Application.DTOs;

public class DocumentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
    public DocumentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UploadDocumentResponseDto
{
    public Guid DocumentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class QueryDto
{
    public string Question { get; set; } = string.Empty;
}

public class RagResponseDto
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public List<string> SourceChunks { get; set; } = new();
}
