using IntelligentDocumentProcessor.Application.DTOs;
using IntelligentDocumentProcessor.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace IntelligentDocumentProcessor.Application.Services;

public class RagService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IEmbeddingService _embeddingService;
    private readonly ILogger<RagService> _logger;

    public RagService(
        IDocumentRepository documentRepository,
        IEmbeddingService embeddingService,
        ILogger<RagService> logger)
    {
        _documentRepository = documentRepository;
        _embeddingService = embeddingService;
        _logger = logger;
    }

    public async Task<RagResponseDto> QueryAsync(QueryDto query)
    {
        _logger.LogInformation("Processing RAG query: {Question}", query.Question);

        // Step 1: Generate embedding for the question (mock)
        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(query.Question);
        _logger.LogInformation("Generated query embedding with {Dimensions} dimensions.", queryEmbedding.Length);

        // Step 2: Retrieve all chunks (in production, do vector similarity search)
        var allChunks = await _documentRepository.GetAllChunksAsync();
        var relevantChunks = allChunks
            .Take(3) // Mock: take first 3 chunks as "most relevant"
            .Select(c => c.Text)
            .ToList();

        _logger.LogInformation("Found {ChunkCount} relevant chunks.", relevantChunks.Count);

        // Step 3: Generate response (mock - in production, call LLM with context)
        var answer = relevantChunks.Count > 0
            ? $"Based on the available documents, here is a summary related to your question: '{query.Question}'. " +
              $"The system found {relevantChunks.Count} relevant chunk(s). " +
              "This is a mock response — integrate an LLM (OpenAI/Azure OpenAI) for real answers."
            : $"No relevant documents found for your question: '{query.Question}'. Please upload documents first.";

        return new RagResponseDto
        {
            Question = query.Question,
            Answer = answer,
            SourceChunks = relevantChunks
        };
    }
}
