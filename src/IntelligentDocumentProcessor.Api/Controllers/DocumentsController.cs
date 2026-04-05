using IntelligentDocumentProcessor.Application.DTOs;
using IntelligentDocumentProcessor.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntelligentDocumentProcessor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly DocumentService _documentService;
    private readonly ILogger<DocumentsController> _logger;

    public DocumentsController(DocumentService documentService, ILogger<DocumentsController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    /// <summary>
    /// Upload a document (PDF or image) for processing.
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(UploadDocumentResponseDto), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Upload attempted with no file.");
            return BadRequest(new { error = "No file was uploaded." });
        }

        // Validate file type
        var allowedExtensions = new[] { ".pdf", ".png", ".jpg", ".jpeg", ".tiff", ".bmp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            _logger.LogWarning("Upload rejected: unsupported file type {Extension}.", extension);
            return BadRequest(new { error = $"Unsupported file type '{extension}'. Allowed: {string.Join(", ", allowedExtensions)}" });
        }

        // Save file to local storage
        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        Directory.CreateDirectory(uploadsDir);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsDir, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        _logger.LogInformation("File {FileName} saved to {FilePath}.", file.FileName, filePath);

        var result = await _documentService.UploadDocumentAsync(file.FileName, filePath);

        return Accepted(result);
    }

    /// <summary>
    /// Get the processing status of a document.
    /// </summary>
    [HttpGet("{id:guid}/status")]
    [ProducesResponseType(typeof(DocumentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStatus(Guid id)
    {
        var document = await _documentService.GetDocumentStatusAsync(id);
        if (document == null)
            return NotFound(new { error = $"Document with ID '{id}' not found." });

        return Ok(document);
    }

    /// <summary>
    /// Get all uploaded documents.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DocumentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var documents = await _documentService.GetAllDocumentsAsync();
        return Ok(documents);
    }
}
