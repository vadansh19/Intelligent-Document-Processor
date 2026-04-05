using IntelligentDocumentProcessor.Application.DTOs;
using IntelligentDocumentProcessor.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntelligentDocumentProcessor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RagController : ControllerBase
{
    private readonly RagService _ragService;
    private readonly ILogger<RagController> _logger;

    public RagController(RagService ragService, ILogger<RagController> logger)
    {
        _ragService = ragService;
        _logger = logger;
    }

    /// <summary>
    /// Query the RAG pipeline with a question.
    /// </summary>
    [HttpPost("query")]
    [ProducesResponseType(typeof(RagResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Query([FromBody] QueryDto query)
    {
        if (string.IsNullOrWhiteSpace(query.Question))
        {
            _logger.LogWarning("RAG query received with empty question.");
            return BadRequest(new { error = "Question cannot be empty." });
        }

        var response = await _ragService.QueryAsync(query);
        return Ok(response);
    }
}
