using IntelligentDocumentProcessor.Domain.Enums;

namespace IntelligentDocumentProcessor.Domain.Entities;

public class Document
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
    public DocumentStatus Status { get; set; } = DocumentStatus.Uploaded;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public ICollection<DocumentChunk> Chunks { get; set; } = new List<DocumentChunk>();
}
