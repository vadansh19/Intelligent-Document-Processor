# Intelligent Document Processor

An advanced, AI-driven documentation processing pipeline designed to extract, classify, and analyze data from various unstructured document formats (PDFs, images, scanned documents, docs). 

## 📑 Table of Contents
- [Features](#-features)
- [Architecture](#-architecture)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Usage](#-usage)
- [Configuration](#-configuration)
- [API Reference](#-api-reference)
- [Roadmap](#-roadmap)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Features

- **Multi-format Support**: Process PDFs, Word documents, TIFF, JPEG, and PNG files.
- **Intelligent OCR**: Built-in Optical Character Recognition (OCR) to extract text from scanned images and un-selectable PDFs.
- **Document Classification**: Automatically categorize incoming documents into predefined classes (e.g., Invoices, Receipts, Contracts, Resumes).
- **Entity Extraction**: Extract key data points using Named Entity Recognition (NER), including names, dates, amounts, and custom fields.
- **Tabular Data Extraction**: Identify and extract tabular structures into structured formats like CSV or JSON.
- **RESTful API**: Easily integrate into existing workflows using fully documented API endpoints.
- **Scalable Architecture**: Designed to handle high-throughput processing using asynchronous task queues.

## 🏗 Architecture

The Intelligent Document Processor is built around a modular architecture:

1.  **Ingestion Layer**: Handles file uploads and input validation.
2.  **Processing Service**: Manages document routing, text extraction (OCR), and layout analysis.
3.  **NLP/ML Engine**: Performs classification and named entity extraction using state-of-the-art language models.
4.  **Storage Layer**: Handles document artifacts and structured output storage.
5.  **API Gateway**: Exposes processing capabilities to client applications.

## 🛠 Prerequisites

Before you begin, ensure you have met the following requirements:
*   Python 3.8+
*   Tesseract OCR (System dependency)
*   Redis (For task queuing)
*   PostgreSQL (Optional, for storing processing metadata)

## 🚀 Installation

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/yourusername/Intelligent-Document-Processor.git
    cd Intelligent-Document-Processor
    ```

2.  **Create a virtual environment:**
    ```bash
    python -m venv venv
    source venv/bin/activate  # On Windows use `venv\Scripts\activate`
    ```

3.  **Install dependencies:**
    ```bash
    pip install -r requirements.txt
    ```

4.  **Set up environment variables:**
    Copy the example `.env` file and populate your specific keys.
    ```bash
    cp .env.example .env
    ```

## 💻 Usage

### Starting the Application

You can start the main processing server locally by running:

```bash
python main.py
```

### Example Usage via Python

```python
from idp import DocumentProcessor

processor = DocumentProcessor(config_path="config.yaml")
result = processor.process("path/to/invoice-001.pdf")

print(f"Document Type: {result.document_type}")
print(f"Extracted Entities: {result.entities}")
```

## ⚙️ Configuration

The processor can be fine-tuned using the `config.yaml` file. Key configurable parameters include:
*   `ocr_engine`: Choose between Tesseract, cloud OCR providers, etc.
*   `confidence_threshold`: Set minimum confidence scores for classification and extraction.
*   `supported_languages`: Define language codes for OCR processing.

## 🔌 API Reference

Detailed API documentation is available via Swagger/OpenAPI. Once the server is running, navigate to:

`http://localhost:8000/docs`

### Sample Endpoint: `POST /api/v1/process`
Uploads a document and returns the extracted structured data.

## 🗺 Roadmap

- [ ] Add support for hand-written text recognition (HTR).
- [ ] Integrate LLM-based zero-shot extraction.
- [ ] Develop a user-friendly dashboard for reviewing uncertain extractions (Human-in-the-loop).
- [ ] Provide pre-built Docker images for easier deployment.

## 🤝 Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request
