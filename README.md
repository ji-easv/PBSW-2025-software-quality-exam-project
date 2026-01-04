# BoxFactory - Software Quality Exam Project

A full-stack box manufacturing management system demonstrating software quality practices including comprehensive testing, CI/CD pipelines, and clean architecture.
This is an exam project for the 2nd semester of the Software Development Top-Up at EASV.

## Testing Strategy

### Unit Tests (`Core.UnitTests`)

- Service layer testing with mocking
- AutoMapper profile validation
- Business logic verification
- Coverage reporting with Coverlet

### Integration Tests (`BoxFactory.IntegrationTests`)

- API endpoint testing with WebApplicationFactory
- Database integration with test containers
- HTTP request/response validation
- Test data builders for complex scenarios

### BDD Tests (Behavior-Driven Development)

- SpecFlow feature files in Gherkin syntax
- Located in `BoxFactory.IntegrationTests/BDD/Features`
- Step definitions for readable acceptance tests
- Example: `CreateBox.feature` for box creation scenarios

### E2E Tests

- Playwright tests for frontend workflows
- Full user journey validation
- Located in `Frontend/tests/`

## Quick Start

### Prerequisites

- .NET 10 SDK
- Node.js 18+
- Docker & Docker Compose
- PostgreSQL (if running locally without Docker)

### Run with Docker Compose

```bash
docker-compose up --build
```

**Services**:

- Backend API: http://localhost:5133
- Frontend: http://localhost:8080
- PostgreSQL: localhost:5432

### Run Backend Locally

```bash
# Run API
dotnet run --project BoxFactoryAPI/BoxFactoryAPI.csproj
```

### Run Frontend Locally

```bash
cd Frontend
npm install
npm start
```

### Run Tests

```bash
# Backend tests with coverage
make test-backend

# Unit tests only
dotnet test Core.UnitTests/Core.UnitTests.csproj

# Integration tests
dotnet test BoxFactory.IntegrationTests/BoxFactory.IntegrationTests.csproj

# Frontend E2E tests
cd Frontend
npx playwright test
```

## Quality Assurance

### Test Coverage

Comprehensive test suite with unit tests for business logic and integration tests for API endpoints. Code coverage metrics are tracked using Coverlet, generating OpenCover format reports for both Core and Integration test projects. Coverage reports help identify untested code paths and maintain quality standards.

### CI/CD

Automated GitHub Actions pipelines run on every push and pull request. The pipelines execute all test suites (unit, integration, BDD, and E2E), build Docker images, and validate code quality. This ensures early detection of issues and maintains deployment readiness.

### Code Quality

Dependency injection for loose coupling and testability. SonarQube performs static code analysis to detect code smells, security vulnerabilities, bugs, and maintainability issues with automated quality gates.
