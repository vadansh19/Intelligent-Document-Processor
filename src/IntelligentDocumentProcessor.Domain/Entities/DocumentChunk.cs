namespace IntelligentDocumentProcessor.Domain.Entities;

public class DocumentChunk
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Placeholder for vector embedding. In production, this would be stored
    /// as a pgvector 'vector' type. For now we store a serialized float array.
    /// </summary>
    public string? Embedding { get; set; }

    // Navigation property
    public Document Document { get; set; } = null!;
}
