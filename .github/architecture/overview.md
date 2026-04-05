# 🧠 Intelligent Document Processing System

## Overview

This system processes documents and enables semantic querying using RAG.

### Core Components

- API Gateway (ASP.NET Core)
- Document Service
- OCR Service (AWS Textract)
- Processing Service
- RAG Service

### Storage

- PostgreSQL (metadata)
- pgvector (embeddings)
- S3 (documents)

---

## Data Flow

1. Upload document
2. Store metadata
3. Send to OCR
4. Extract text
5. Chunk + embed
6. Store vectors
7. Query via RAG