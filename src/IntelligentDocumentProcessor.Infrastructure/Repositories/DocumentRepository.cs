using IntelligentDocumentProcessor.Application.Interfaces;
using IntelligentDocumentProcessor.Domain.Entities;
using IntelligentDocumentProcessor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IntelligentDocumentProcessor.Infrastructure.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly ApplicationDbContext _context;

    public DocumentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Document> AddAsync(Document document)
    {
        await _context.Documents.AddAsync(document);
        await _context.SaveChangesAsync();
        return document;
    }

    public async Task<Document?> GetByIdAsync(Guid id)
    {
        return await _context.Documents
            .Include(d => d.Chunks)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Document>> GetAllAsync()
    {
        return await _context.Documents
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(Document document)
    {
        _context.Documents.Update(document);
        await _context.SaveChangesAsync();
    }

    public async Task AddChunksAsync(IEnumerable<DocumentChunk> chunks)
    {
        await _context.DocumentChunks.AddRangeAsync(chunks);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<DocumentChunk>> GetChunksByDocumentIdAsync(Guid documentId)
    {
        return await _context.DocumentChunks
            .Where(c => c.DocumentId == documentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<DocumentChunk>> GetAllChunksAsync()
    {
        return await _context.DocumentChunks.ToListAsync();
    }
}
