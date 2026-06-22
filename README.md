[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />

<h3 align="center">AI Core Enterprise</h3>

<p align="center">
  Enterprise-grade .NET 10 AI Assistant Web API with modern architecture.
  <br />
  <a href="https://github.com/necmettincimen/ai-core"><strong>Explore the docs »</strong></a>
  <br />
  <br />
  <a href="https://github.com/necmettincimen/ai-core/issues">Report Bug</a>
  ·
  <a href="https://github.com/necmettincimen/ai-core/issues">Request Feature</a>
</p>

---

## Table of Contents
- [About The Project](#about-the-project)
  - [Built With](#built-with)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Roadmap](#roadmap)
- [Architecture](#architecture)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)
- [Acknowledgements](#acknowledgements)

---

## About The Project
This project is an enterprise-grade .NET 10 Web API that demonstrates modern AI integration using Ollama with Llama3. It showcases clean architecture principles, enterprise best practices, and production-ready features including health checks, caching, security headers, API versioning, and comprehensive monitoring. The application is designed for learning, demonstration, and integration into enterprise environments.

### Built With
- [.NET 10](https://dotnet.microsoft.com/) - Web framework
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core/) - Web API framework
- [Microsoft.Extensions.AI](https://learn.microsoft.com/dotnet/ai) - AI integration
- [Ollama](https://ollama.com/) - AI model hosting
- [Llama3](https://llama.meta.com/llama3/) - AI model
- [HybridCache](https://learn.microsoft.com/dotnet/core/extensions/caching) - Modern caching
- [Redis](https://redis.io/) - Distributed caching
- [Scalar.AspNetCore](https://github.com/scalar/scalar) - API documentation
- [Docker](https://www.docker.com/) - Containerization
- [Docker Compose](https://docs.docker.com/compose/) - Multi-container orchestration

---

## Getting Started
To get a local copy up and running follow these simple steps.

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop) (for containerized deployment)
- [Docker Compose](https://docs.docker.com/compose/install/) (for multi-container setup)

### Installation

1. **Clone the repository**
   ```sh
   git clone https://github.com/necmettincimen/ai-core.git
   cd ai-core
   ```

2. **Run with Docker Compose (Recommended)**
   ```sh
   docker compose up -d
   ```
   
   This will start:
   - AI Core Web API on port 8080
   - Ollama AI service on port 11434
   - Redis cache on port 6379

3. **Run locally (Development)**
   ```sh
   # Restore dependencies
   dotnet restore
   
   # Run the application
   dotnet run --project AiCore.WebApi
   ```

---

## Usage
Once the application is running, you can access:

- **API Documentation**: http://localhost:8080/scalar
- **Health Check**: http://localhost:8080/health
- **Application Info**: http://localhost:8080/info
- **AI Assistant Endpoint**: 
  ```bash
  POST http://localhost:8080/api/assistant/ask
  Content-Type: application/json
  {
    "question": "What is enterprise architecture?"
  }
  ```

### Example API Calls

```bash
# Health Check
curl http://localhost:8080/health

# Ask AI Assistant
curl -X POST http://localhost:8080/api/assistant/ask \
  -H "Content-Type: application/json" \
  -d '{"question":"Explain microservices architecture"}'

# Application Info
curl http://localhost:8080/info
```

### 🚀 Advanced AI Features

#### **Streaming AI (Real-time yanıt)**
```bash
# Streaming AI Chat - Real-time response flow
curl -X POST http://localhost:8080/api/assistant/ask-streaming \
  -H "Content-Type: application/json" \
  -d '{"question":"Explain microservices"}'
```

#### **Embedding Generation (Vektör temsili)**
```bash
# Generate embeddings for semantic search
curl -X POST http://localhost:8080/api/assistant/embedding \
  -H "Content-Type: application/json" \
  -d '{"text":"enterprise architecture"}'
```

#### **Text Generation (Parametrik üretim)**
```bash
# Generate text with specific parameters
curl -X POST http://localhost:8080/api/assistant/generate-text \
  -H "Content-Type: application/json" \
  -d '{"prompt":"Write about AI","maxTokens":150}'
```

#### **Mülakat İçin Gösterebileceğiniz Özellikler**

```bash
# Streaming AI (Real-time yanıt)
POST /api/assistant/ask-streaming
{"question": "Explain microservices"}
 
# Embedding Generation (Vektör temsili)
POST /api/assistant/embedding  
{"text": "enterprise architecture"}
 
# Text Generation (Parametrik üretim)
POST /api/assistant/generate-text
{"prompt": "Write about AI", "maxTokens": 150}
```

---

## Roadmap
- [x] Clean Architecture implementation
- [x] Enterprise health checks
- [x] Distributed caching with Redis
- [x] API versioning and documentation
- [x] Security headers and middleware
- [x] Docker containerization
- [x] Request correlation and logging
- [ ] Authentication and authorization (JWT)
- [ ] Rate limiting and throttling
- [ ] Comprehensive unit and integration tests
- [ ] CI/CD pipeline configuration
- [ ] Kubernetes deployment manifests
- [ ] Monitoring and metrics (Prometheus)

See the [open issues](https://github.com/necmettincimen/ai-core/issues) for more.

---

## Architecture
This project follows Clean Architecture principles with enterprise-grade patterns:

```
AiCore/
├── AiCore.Domain/           # Core business logic and entities
├── AiCore.Application/      # Application services and interfaces
├── AiCore.Infrastructure/   # External dependencies (AI services, caching)
├── AiCore.WebApi/          # Web API layer and controllers
├── docker-compose.yml      # Multi-container orchestration
├── Dockerfile             # Container build configuration
└── README.md              # This documentation
```

**Key Architectural Patterns:**

- **Clean Architecture**: Separation of concerns with dependency inversion
- **Dependency Injection**: Modern DI patterns throughout the application
- **Repository Pattern**: Abstraction for data access
- **Service Layer**: Business logic encapsulation
- **DTO Pattern**: Data transfer objects for API contracts
- **Middleware Pipeline**: Cross-cutting concerns (logging, security, etc.)

**Enterprise Features:**

- **Health Checks**: AI service, memory, and system monitoring
- **Caching Strategy**: Hybrid local + distributed Redis caching
- **API Versioning**: Enterprise API version management
- **Security**: XSS protection, CSRF protection, security headers
- **Logging**: Structured logging with request correlation
- **Documentation**: OpenAPI 3.1 with Scalar UI
- **Containerization**: Docker multi-stage builds

---

## Contributing
Contributions are welcome! Please fork the repo and submit a pull request.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## License
Distributed under the MIT License. See `LICENSE` for more information.

---

## Contact
Necmettin Çimen - [@Necmettin Cimen](https://necmettincimen.github.io) - [necmettin.dev@gmail.com](mailto:necmettin.dev@gmail.com)

Project Link: [https://github.com/necmettincimen/ai-core](https://github.com/necmettincimen/ai-core)

---

## Acknowledgements
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Microsoft.Extensions.AI Documentation](https://learn.microsoft.com/dotnet/ai)
- [Ollama Documentation](https://github.com/ollama/ollama)
- [Docker Documentation](https://docs.docker.com/)
- [Redis Documentation](https://redis.io/documentation)
- [Scalar API Documentation](https://scalar.com/)
- [Clean Architecture Principles](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

<!-- MARKDOWN LINKS & IMAGES -->

[contributors-shield]: https://img.shields.io/github/contributors/necmettincimen/ai-core.svg?style=for-the-badge
[contributors-url]: https://github.com/necmettincimen/ai-core/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/necmettincimen/ai-core.svg?style=for-the-badge
[forks-url]: https://github.com/necmettincimen/ai-core/network/members
[stars-shield]: https://img.shields.io/github/stars/necmettincimen/ai-core.svg?style=for-the-badge
[stars-url]: https://github.com/necmettincimen/ai-core/stargazers
[issues-shield]: https://img.shields.io/github/issues/necmettincimen/ai-core.svg?style=for-the-badge
[issues-url]: https://github.com/necmettincimen/ai-core/issues
[license-shield]: https://img.shields.io/github/license/necmettincimen/ai-core.svg?style=for-the-badge
[license-url]: https://github.com/necmettincimen/ai-core/blob/master/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/necmettincimen

---
