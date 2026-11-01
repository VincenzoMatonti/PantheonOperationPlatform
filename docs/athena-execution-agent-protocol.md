# Athena Execution Agent protocol (provisional)

Athena's process-management adapter communicates with one configured execution-agent endpoint. The repository contains the Athena client adapter and correlated request/result contracts, but no Agent server implementation; correlated routes below are therefore a required protocol contract, not verified interoperability.

Configure the endpoint with `Athena:ExecutionAgent:BaseUrl` (or `Athena__ExecutionAgent__BaseUrl`). It must be an absolute HTTP or HTTPS URL. The HTTP client has a 15-second timeout. If the endpoint is absent, application startup is permitted, but each agent operation fails explicitly with `ExecutionAgentNotConfiguredException`.

All paths below are relative to the configured base URL. Requests and successful responses use JSON. The API expects successful commands to return the response objects below. Non-success HTTP responses are surfaced as request failures; malformed or empty JSON responses are reported as invalid agent responses.

| Method | Path | Request | Response |
|---|---|---|---|
| `POST` | `system-instances/{id}/operations` | `SystemInstanceExecutionCommand` | correlated `SystemInstanceExecutionResult` |
| `GET` | `system-instances/{id}/operations/{operationId}` | none | correlated `SystemInstanceExecutionResult` (404 maps to `NotFound`) |
| `GET` | `system-instances/{id}/status` | none | `SystemInstanceExecutionStatus` |
| `GET` | `system-instances/{id}/health` | none | `SystemInstanceExecutionHealth` |
| `GET` | `system-instances/{id}/logs?tail={count}` | none | array of `SystemInstanceExecutionLogEntry` |

`SystemInstanceExecutionCommand` carries a stable `operationId`, `correlationId`, `systemInstanceId`, command, contract version, optional System/Workload/name and JSON payload. Repeated POSTs with the same OperationId must return the current or terminal result and must not execute a second command. Results must echo OperationId/SystemInstanceId and may echo CorrelationId. Result states are Accepted, Pending, Running, Completed, Failed, NotFound, and OutcomeUnknown. Completed is valid only after the Agent has durably saved the result. `observedState` is a separate observation and is not implied by command completion.

The legacy routes may remain available to old clients, but they do not carry OperationId and cannot provide correlation or safe retry guarantees. Legacy commands with ambiguous results remain unresolved; unsupported legacy Configure/Unregister operations are failed before dispatch.

Status responses contain `systemInstanceId`, `state`, `isRunning`, `isHealthy`, `observedAtUtc`, and optional `details`. `state` must match an Athena `SystemInstanceOperationalStatus`; the returned ID must match the requested instance, the timestamp must be UTC, and `isRunning` must agree with `state`. Health responses contain `systemInstanceId`, `isHealthy`, `status`, `checkedAtUtc`, and optional `details`. Logs contain `timestampUtc`, `stream`, and `message`.

Athena persists Create, Register, Configure, Start, Stop, Restart, Remove, and Unregister requests as Operations and returns HTTP 202. Provisioning details are carried in the persisted operation payload; secrets must be represented by references and must not be sent as plaintext.

The asynchronous Athena endpoints are `POST /api/system-instances/management`, `POST /api/system-instances/management/register`, `POST /api/system-instances/management/{id}/{start|stop|restart|configure|unregister}`, and `DELETE /api/system-instances/management/{id}`. Responses include the Operation identifier and a Location pointing to `GET /api/system-instances/management/{id}/operations/{operationId}`. Configure accepts a `configurationReference`; inline details are rejected for queued requests to avoid persisting secret material. The database enforces at most one active SystemInstance Operation, and each Operation defaults to eight delivery attempts before an uncertain result is closed as Indeterminate.

This repository still has no Agent API host, durable Agent execution store, process supervisor, or Docker execution provider. Therefore the Agent endpoint definitions are not an installable server and must not be treated as a working process-execution deployment.
