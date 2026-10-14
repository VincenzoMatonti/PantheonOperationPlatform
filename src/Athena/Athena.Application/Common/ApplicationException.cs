namespace Athena.Application.Common;

public abstract class ApplicationException<TErrorCode>(
    TErrorCode code,
    string message)
    : Exception(message)
    where TErrorCode : Enum
{
    public TErrorCode Code { get; } = code;
}