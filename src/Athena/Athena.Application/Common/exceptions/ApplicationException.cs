namespace Athena.Application.Common.exceptions;

public abstract class ApplicationException(
    string errorSource, string message,
    ApplicationCoreErrorType errorType) : Exception(message)
{
    public string ErrorSource { get; } = errorSource;
    public ApplicationCoreErrorType ErrorType { get; } = errorType;
    public abstract string CodeName { get; }
}

public abstract class ApplicationException<TErrorCode>(
    TErrorCode code, string errorSource, string message,
    ApplicationCoreErrorType errorType) : ApplicationException(errorSource, message, errorType)
    where TErrorCode : Enum
{
    public TErrorCode Code { get; } = code;
    public override string CodeName => $"{ErrorSource}.{Code}";
}