namespace Hermes.Api.Common.Exceptions;

public class ApiValidationException(List<ValidationError> errors)
    : Exception("One or more validation errors occurred.")
{
    public List<ValidationError> Errors { get; } = errors;
}

public class ApiValidationConfigurationException(string propertyName)
    : Exception($"Validation rule for property '{propertyName}' does not define a valid validation error code.")
{
    public string PropertyName { get; } = propertyName;
}