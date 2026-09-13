namespace Hermes.Api.Clients.Validations;

public enum ClientValidationErrorCode
{
    ClientCodeRequired = 1,
    ClientCodeMaximumLength = 2,
    ClientNameRequired = 3,
    ClientNameMaximumLength = 4,
    ClientIdRequired = 5
}

public enum ClientOperationValidationErrorCode
{
    ClientIdRequired = 1,
    OperationTypeIdRequired = 2,
    ClientOperationIdRequired = 3
}