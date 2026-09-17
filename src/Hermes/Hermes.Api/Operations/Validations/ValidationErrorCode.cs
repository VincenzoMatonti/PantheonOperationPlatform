namespace Hermes.Api.Operations.Validations;

public enum OperationValidationErrorCode
{
    OperationIdRequired = 1,
    ExecutionIdRequired = 2,
    CorrelationIdRequired = 3,
    ExternalIdRequired = 4,
    ExternalIdMaximumLength = 5,
    StatusRequired = 6,
    InvalidStatus = 7
}

public enum OperationTypeValidationErrorCode
{
    OperationTypeIdRequired = 1, 
    CodeRequired = 2, 
    CodeMaximumLength = 3, 
    NameRequired = 4, 
    NameMaximumLength = 5
}