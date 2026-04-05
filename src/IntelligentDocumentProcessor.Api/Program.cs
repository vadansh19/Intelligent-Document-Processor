using IntelligentDocumentProcessor.Application.Interfaces;
using IntelligentDocumentProcessor.Application.Services;
using IntelligentDocumentProcessor.Infrastructure.Data;
using IntelligentDocumentProcessor.Infrastructure.Repositories;
using IntelligentDocumentProcessor.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ----- Database -----
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----- Dependency Injection -----

// Repositories
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

// Infrastructure services (mocks — swap for real implementations later)
builder.Services.AddScoped<IOcrService, MockOcrService>();
builder.Services.AddScoped<IEmbeddingService, MockEmbeddingService>();
builder.Services.AddSingleton<IBackgroundProcessingService, MockBackgroundProcessingService>();

// Application services
builder.Services.AddScoped<DocumentService>();
builder.Services.AddScoped<RagService>();

// ----- Controllers & Swagger -----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Intelligent Document Processor API",
        Version = "v1",
        Description = "API for document processing with OCR and RAG capabilities."
    });
});

// ----- Logging -----
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// ----- Middleware Pipeline -----

// Global exception handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            error = "An unexpected error occurred. Please try again later.",
            statusCode = 500
        });
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IDP API v1");
        c.RoutePrefix = string.Empty; // Swagger at root URL
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
