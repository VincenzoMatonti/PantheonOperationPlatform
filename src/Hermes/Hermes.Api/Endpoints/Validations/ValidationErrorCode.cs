namespace Hermes.Api.Endpoints.Validations;

public enum EndpointValidationErrorCode
{
    EndpointCodeRequired = 1,
    EndpointCodeMaximumLength = 2,
    EndpointTypeRequired = 3,
    EndpointIdRequired = 4,
}

public enum EndpointOperationValidationErrorCode
{
    EndpointIdRequired = 1,
    OperationTypeIdRequired = 2,
    EndpointOperationIdRequired = 3
}
