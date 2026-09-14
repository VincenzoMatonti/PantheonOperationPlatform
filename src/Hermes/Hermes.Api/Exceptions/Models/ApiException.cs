namespace Hermes.Api.Exceptions.Models;

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

public class ApiExceptionMapperNotFoundException(string exceptionType)
    : Exception($"No exception mapper found for exception type '{exceptionType}'.")
{
    public string ExceptionType { get; } = exceptionType;
}

public class ApiExceptionMapperConfigurationException(string mapperType, string exceptionType)
    : Exception($"Exception mapper '{mapperType}' cannot handle exception type '{exceptionType}'.")
{
    public string MapperType { get; } = mapperType;
    public string ExceptionType { get; } = exceptionType;
}

public class ApiInternalException()
    : Exception("An internal server error occurred.")
{
}