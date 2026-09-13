namespace Hermes.Api.Common;

public enum ApiErrorType
{
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    Internal = 6,
    External = 7
}

public enum ApiErrorEntity
{
    Unknown = 0,
    Client = 1,
    Endpoint = 2,
    Operation = 3,
    OperationType = 4,
    ClientOperation = 5,
    EndpointOperation = 6,
    Route = 7,
    Execution = 8,
    ExecutionStep = 9,
    ExecutionType = 10,
    Package = 11,
    Data = 12,
    Metadata = 13,
    Header = 14
}