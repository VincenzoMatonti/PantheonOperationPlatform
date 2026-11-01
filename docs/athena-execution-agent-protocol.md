# Athena Execution Agent protocol (provisional)

Athena's process-management adapter communicates with one configured execution-agent endpoint. The wire protocol below describes the current implementation; it has not yet been confirmed against a production agent and must be treated as provisional.

Configure the endpoint with `Athena:ExecutionAgent:BaseUrl` (or `Athena__ExecutionAgent__BaseUrl`). It must be an absolute HTTP or HTTPS URL. The HTTP client has a 15-second timeout. If the endpoint is absent, application startup is permitted, but each agent operation fails explicitly with `ExecutionAgentNotConfiguredException`.

All paths below are relative to the configured base URL. Requests and successful responses use JSON. The API expects successful commands to return the response objects below. Non-success HTTP responses are surfaced as request failures; malformed or empty JSON responses are reported as invalid agent responses.

| Method | Path | Request | Response |
|---|---|---|---|
| `POST` | `system-instances` | `SystemInstanceExecutionCommandRequest` | `SystemInstanceExecutionStatus` |
| `POST` | `system-instances/register` | `SystemInstanceExecutionCommandRequest` | `SystemInstanceExecutionStatus` |
| `POST` | `system-instances/{id}/start` | none | `SystemInstanceExecutionStatus` |
| `POST` | `system-instances/{id}/stop` | none | `SystemInstanceExecutionStatus` |
| `POST` | `system-instances/{id}/restart` | none | `SystemInstanceExecutionStatus` |
| `DELETE` | `system-instances/{id}` | none | `SystemInstanceExecutionStatus` |
| `GET` | `system-instances/{id}/status` | none | `SystemInstanceExecutionStatus` |
| `GET` | `system-instances/{id}/health` | none | `SystemInstanceExecutionHealth` |
| `GET` | `system-instances/{id}/logs?tail={count}` | none | array of `SystemInstanceExecutionLogEntry` |

`SystemInstanceExecutionCommandRequest` contains `systemInstanceId`, optional `systemId`, `workloadId`, `name`, and `details`. Status responses contain `systemInstanceId`, `state`, `isRunning`, `isHealthy`, `observedAtUtc`, and optional `details`. `state` must match an Athena `SystemInstanceOperationalStatus`; the returned ID must match the requested instance, the timestamp must be UTC, and `isRunning` must agree with `state`. Health responses contain `systemInstanceId`, `isHealthy`, `status`, `checkedAtUtc`, and optional `details`. Logs contain `timestampUtc`, `stream`, and `message`.

Before using this protocol with a real agent, confirm path names, HTTP methods, request and response casing, status/error semantics, timeout expectations, and whether create/register should return a status synchronously. No production fake or container-runtime fallback is provided.
