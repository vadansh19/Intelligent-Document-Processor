using IntelligentDocumentProcessor.Domain.Entities;

namespace IntelligentDocumentProcessor.Application.Interfaces;

public interface IDocumentRepository
{
    Task<Document> AddAsync(Document document);
    Task<Document?> GetByIdAsync(Guid id);
    Task<IEnumerable<Document>> GetAllAsync();
    Task UpdateAsync(Document document);
    Task AddChunksAsync(IEnumerable<DocumentChunk> chunks);
    Task<IEnumerable<DocumentChunk>> GetChunksByDocumentIdAsync(Guid documentId);
    Task<IEnumerable<DocumentChunk>> GetAllChunksAsync();
}
