# Hermes.Api

> The HTTP layer of Hermes: it exposes the application use cases through a REST API, validates requests, converts request and response models, and translates application and domain exceptions into coherent HTTP responses.

## Table of contents

- [Overview](#overview)
- [API layer map](#api-layer-map)
- [Project structure](#project-structure)
- [Controllers](#controllers)
- [Requests and responses](#requests-and-responses)
- [Mappings](#mappings)
- [Validation with FluentValidation](#validation-with-fluentvalidation)
- [Exception handling](#exception-handling)
- [Common responses](#common-responses)
- [Configuration and dependency injection](#configuration-and-dependency-injection)
- [HTTP flow](#http-flow)
- [Dependencies and responsibilities](#dependencies-and-responsibilities)
- [Principles](#principles)

---

## Overview

`Hermes.Api` is the HTTP entry layer of the application. It receives REST requests, applies input validation, converts them into commands and queries for `Hermes.Application`, invokes the corresponding use case, and returns a uniform HTTP response.

The layer does not implement business rules and does not access the database directly. Application rules remain in `Hermes.Application`, while invariants and state transitions remain in `Hermes.Domain`.

| Area | Responsibility |
|---|---|
| **Controllers** | Expose HTTP endpoints and coordinate mapping and use cases |
| **Requests** | Define the API input contracts |
| **Responses** | Define the API output contracts |
| **Mappings** | Translate requests into commands/queries and application DTOs into responses |
| **Validations** | Validate requests with FluentValidation |
| **Exceptions** | Translate application, domain, and technical errors into HTTP errors |
| **Filters** | Execute validation before the MVC action |
| **Common** | Contains shared response wrappers and models |

The project uses `net10.0`, nullable reference types, and ASP.NET Core MVC. OpenAPI and Swagger UI documentation are available in Development.

---

## API layer map

```mermaid
flowchart LR
    Client[HTTP Client] --> Controller[API Controller]
    Controller --> Filter[FluentValidationFilter]
    Filter --> Validator[FluentValidation validators]
    Controller --> RequestMapper[Request mapper]
    RequestMapper --> Application[Hermes.Application]
    Application --> ResponseMapper[Response mapper]
    ResponseMapper --> Response[ApiResponse]
    Response --> Client
    Application -.-> AppException[Application exception]
    Application -.-> DomainException[Domain exception]
    AppException --> Resolver[ExceptionMapperResolver]
    DomainException --> Resolver
    Resolver --> ExceptionMapper[IExceptionMapper]
    ExceptionMapper --> Builder[ExceptionResponseBuilder]
    Builder --> Handler[GlobalExceptionHandler]
    Handler --> Client
```

### Main relationships

| Relationship | Responsibility |
|---|---|
| `Controller` — `RequestMapper` | Converts the HTTP request into an application command or query |
| `Controller` — `UseCaseHandler` | Invokes the correct use case with the request `CancellationToken` |
| `UseCaseHandler` — `ResponseMapper` | Converts the application DTO into the HTTP response |
| `FluentValidationFilter` — `IValidator<T>` | Finds and executes the validator associated with the request type |
| `ExceptionMapperResolver` — `IExceptionMapper` | Selects the mapper capable of handling the exception |
| `GlobalExceptionHandler` — `ExceptionResponseBuilder` | Builds a uniform HTTP response for errors |

---

## Project structure

```text
Hermes.Api/
├── Clients/
│   ├── Controllers/
│   ├── Mappings/
│   │   ├── Commands/
│   │   ├── Exceptions/
│   │   └── Queries/
│   ├── Requests/
│   ├── Responses/
│   └── Validations/
├── Common/
│   ├── ApiResponse.cs
│   └── PropertyApiResponse.cs
├── Exceptions/
│   ├── Builders/
│   ├── Handlers/
│   ├── Mappings/
│   └── Models/
├── Filters/
│   └── FluentValidationFilter.cs
├── Program.cs
└── Hermes.Api.csproj
```

The features are organized by application area. Currently, the API exposes the `Clients` and `ClientOperation` areas; the same structure can be extended with `Operations` and `OperationTypes`.

---

## Controllers

Controllers are the HTTP entry point. They do not contain domain logic and do not build application entities directly: they receive a request, use the appropriate mapper, invoke the use case, and return the API response.

### Client command

`ClientCommandApiController` exposes the endpoints under `api/client/command`:

| Method | Endpoint | Operation |
|---|---|---|
| `POST` | `/create` | Creates a client |
| `PUT` | `/rename` | Renames a client |
| `PUT` | `/renameCode` | Changes the client code |
| `PUT` | `/activate` | Activates a client |
| `PUT` | `/deactivate` | Deactivates a client |
| `DELETE` | `/delete` | Logically deletes a client |
| `PUT` | `/restore` | Restores a client |

### Client query

`ClientQueryApiController` exposes the endpoints under `api/client/query`:

| Method | Endpoint | Operation |
|---|---|---|
| `GET` | `/all` | Retrieves all clients |
| `POST` | `/byId` | Retrieves a client by id |
| `POST` | `/byCode` | Retrieves a client by code |
| `POST` | `/codeById` | Retrieves the code of a client |
| `GET` | `/allCodes` | Retrieves all codes |
| `GET` | `/active` | Retrieves active clients |
| `GET` | `/nonActive` | Retrieves inactive clients |
| `GET` | `/deleted` | Retrieves deleted clients |

### ClientOperation command

`ClientOperationCommandApiController` exposes the endpoints under `api/clientOperation/command` to create, enable, disable, logically delete, and restore the association between client and operation type.

### ClientOperation query

`ClientOperationQueryApiController` exposes the endpoints under `api/clientOperation/query` to find an association by id, client and operation type, client id, operation type id, and status.

All asynchronous actions receive and propagate the HTTP request `CancellationToken` to the application layer.

---

## Requests and responses

### Requests

Requests represent the inbound HTTP contracts and are specific to the API. They are not passed directly to the domain or application layer.

Examples:

- `CreateClientRequest`;
- `RenameClientRequest`;
- `RenameClientCodeRequest`;
- `GetClientByIdRequest`;
- `GetClientByCodeRequest`;
- `CreateClientOperationRequest`;
- `EnableClientOperationRequest`;
- `GetClientOperationByIdRequest`.

### Responses

Responses represent the outbound HTTP contracts and do not directly expose domain entities or value objects.

Examples:

- `ClientResponse` and `ClientCodeResponse`;
- `CreateClientResponse`, `RenameClientResponse`, and `RenameClientCodeResponse`;
- `ActivateClientResponse`, `DeactivateClientResponse`, and `DeleteClientResponse`;
- `RestoreClientResponse`;
- `ClientOperationResponse` and the responses for related operations.

The result is normally returned with a wrapper `ApiResponse<T>` to keep a uniform shape across endpoints.

---

## Mappings

Mappers isolate the boundary between the HTTP model and the application model. They are split into mappers for commands, queries, and exceptions.

### Request mapper

Request mappers convert API requests into commands or queries for `Hermes.Application`:

```text
CreateClientRequest
  → ClientCommandRequestMapper
  → CreateClientCommand
  → ClientUseCaseHandler
```

```text
GetClientByIdRequest
  → ClientQueryRequestMapper
  → GetClientByIdQuery
  → ClientUseCaseHandler
```

Separate mappers exist for `Client` and `ClientOperation` to keep the contracts of different areas separate.

### Response mapper

Response mappers convert the DTOs returned by the application layer into HTTP responses:

```text
ClientDto
  → ClientCommandResponseMapper / ClientQueryResponseMapper
  → ClientResponse
  → ApiResponse<ClientResponse>
```

Conversions support both single results and lists and prevent application DTOs from becoming part of the public HTTP contract.

### Exception mapper

Exception mappers implement `IExceptionMapper` and transform exceptions into `ExceptionMappingResult`, containing the status code and a list of `ApiErrorResponse`.

Current mappers include:

- `ValidationExceptionMapper` for validation errors;
- `ClientExceptionMapper` for client application and domain exceptions;
- `ClientOperationExceptionMapper` for client-operation application and domain exceptions;
- `InternalExceptionMapper` for technical or configuration errors in the API.

---

## Validation with FluentValidation

Input validation in the HTTP layer is implemented with FluentValidation and executed by `FluentValidationFilter` before the action runs.

The filter:

1. inspects the action arguments;
2. finds the registered `IValidator<T>` for the concrete request type;
3. executes `ValidateAsync` using `HttpContext.RequestAborted`;
4. collects all errors;
5. reads the error code enum from `CustomState`;
6. raises `ApiValidationException` if errors exist.

Rules use `WithState(...)` to associate a stable code with the error, in addition to the message intended for the caller.

Examples of existing rules:

- required ids with `NotEmpty()`;
- required client code with a maximum length of 100 characters;
- required client name with a maximum length of 200 characters;
- required client id and operation type id for associations.

Registration is done through:

```csharp
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientRequestValidator>();
```

If a rule does not define a valid enum code, the filter raises `ApiValidationConfigurationException`, which is treated as an internal configuration error.

---

## Exception handling

The global exception handler is registered with `AddExceptionHandler<GlobalExceptionHandler>()` and activated via `app.UseExceptionHandler()`.

### Exception pipeline

```text
Exception
  → ExceptionMapperResolver
  → IExceptionMapper
  → ExceptionMappingResult
  → ExceptionResponseBuilder
  → GlobalExceptionHandler
  → HTTP status + ApiErrorResponse[]
```

`ExceptionMapperResolver` looks for the first mapper whose `CanHandle(exception)` returns `true`. If no mapper is available, it raises `ApiExceptionMapperNotFoundException`.

### Validation errors

`ValidationExceptionMapper` converts `ApiValidationException` into:

- HTTP `400 Bad Request`;
- `ApiErrorType.Validation`;
- one error per invalid property;
- enum code and message defined by the validator.

### Application and domain errors

Area-specific mappers distinguish application errors from domain errors:

| Error type | Typical response |
|---|---|
| Resource not found | `404 Not Found` |
| Resource already exists or application conflict | `409 Conflict` |
| Domain rule not respected | `409 Conflict` |
| Internal error or wrong configuration | `500 Internal Server Error` |

Errors expose type, entity, code, and message through `ApiErrorResponse`.

### Internal errors

`InternalExceptionMapper` handles configuration and infrastructure errors in the API, including:

- `ApiInternalException`;
- `ApiValidationConfigurationException`;
- `ApiExceptionMapperNotFoundException`;
- `ApiExceptionMapperConfigurationException`.

The message returned to the client is generic so as not to expose internal application details.

---

## Common responses

`ApiResponse<T>` provides the common wrapper for successful responses, while `PropertyApiResponse` supports details associated with a specific property.

Errors are represented by `ApiErrorResponse` and classified via:

- `ApiErrorType`, e.g. `Validation`, `NotFound`, `Conflict`, and `Internal`;
- `ApiErrorEntity`, e.g. `Client`, `ClientOperation`, and `Unknown`.

This structure allows clients to interpret errors through code and type without depending only on the message text.

---

## Configuration and dependency injection

`Program.cs` configures:

- MVC controllers;
- `FluentValidationFilter` as a global action filter;
- FluentValidation validators by assembly scanning;
- `GlobalExceptionHandler`;
- exception mappers and `ExceptionMapperResolver`;
- `ExceptionResponseBuilder`;
- request and response mappers;
- application handlers for `Client` and `ClientOperation`;
- OpenAPI and Swagger UI in Development.

Service registration keeps controllers thin and allows mapper, validator, and handler replacements or extensions without changing the application flow.

---

## HTTP flow

```mermaid
sequenceDiagram
    participant Client as HTTP Client
    participant Controller as Controller
    participant Filter as FluentValidationFilter
    participant Mapper as RequestMapper
    participant UC as Application UseCase
    participant ResponseMapper as ResponseMapper
    participant Handler as GlobalExceptionHandler

    Client->>Controller: HTTP request
    Controller->>Filter: Action arguments
    Filter->>Filter: ValidateAsync(request)
    alt Request invalid
        Filter-->>Handler: ApiValidationException
        Handler-->>Client: 400 ApiErrorResponse[]
    else Request valid
        Controller->>Mapper: ToCommand / ToQuery
        Mapper-->>Controller: Application command/query
        Controller->>UC: Execute use case
        UC-->>Controller: Application DTO
        Controller->>ResponseMapper: ToResponse
        ResponseMapper-->>Controller: API response
        Controller-->>Client: 200 ApiResponse<T>
    end

    alt Application or domain exception
        UC-->>Handler: Exception
        Handler-->>Client: 404 / 409 / 500 ApiErrorResponse[]
    end
```

The API layer does not commit a transaction and does not handle persistence directly. Its task is to adapt the HTTP protocol to the application contracts.

---

## Dependencies and responsibilities

### Allowed dependencies

```text
Hermes.Api
  → Hermes.Application
  → Hermes.Infrastructure
```

The API may use:

- commands, queries, DTOs, and use cases from the application layer;
- infrastructure services registered in the container;
- ASP.NET Core, FluentValidation, and OpenAPI;
- HTTP-specific models from the API itself.

### Responsibilities of the API

- define endpoints and HTTP verbs;
- validate the format and required data of requests;
- convert HTTP and application contracts;
- propagate request cancellation;
- standardize success and error responses;
- hide internal exception details from the client.

### Responsibilities that do not belong to the API

The API must not:

- implement invariants or domain transitions;
- duplicate the logic of use cases;
- access repositories or `DbContext` directly;
- build SQL queries;
- contain Entity Framework mapping;
- return domain entities or value objects directly;
- manage every exception manually inside controllers.

---

## Principles

1. **Thin controllers** — the controller coordinates mapping and use cases without containing business logic.
2. **Separate HTTP contracts** — requests and responses are not the same as application commands, queries, or DTOs.
3. **Validation at the boundary** — inputs are validated before reaching the application layer.
4. **Stable error codes** — errors use enum-based, readable codes for clients.
5. **Centralized exceptions** — error handling passes through the resolver, mapper, builder, and global handler.
6. **Explicit mapping** — requests, responses, and exceptions are converted by dedicated components.
7. **Asynchrony and cancellation** — action methods propagate the `CancellationToken` to the use case.
8. **Uniform responses** — both successes and errors follow common API models.
9. **No domain logic in the API** — invariants and transitions belong to the domain and application layers.
10. **Extensibility by area** — each new area can add controllers, mappers, validators, and exception mappers without altering existing ones.

## Summary

```text
Controller             = HTTP entry and coordination
Request                = inbound HTTP contract
Response               = outbound HTTP contract
RequestMapper          = HTTP request → application command/query
ResponseMapper         = application DTO → HTTP response
FluentValidation       = validation at the edge
IExceptionMapper       = exception → status code and ApiErrorResponse
GlobalExceptionHandler = uniform error handling
ApiResponse            = common wrapper for all responses
```
