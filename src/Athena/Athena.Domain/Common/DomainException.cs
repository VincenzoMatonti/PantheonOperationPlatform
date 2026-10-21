namespace Athena.Domain.Common;

public abstract class DomainException(
    string message,
    DomainErrorType errorType)
    : Exception(message)
{
    public DomainErrorType ErrorType { get; } = errorType;

    public abstract string CodeName { get; }
}

public abstract class DomainException<TCode>(
    TCode code,
    string message,
    DomainErrorType errorType)
    : DomainException(message, errorType)
    where TCode : Enum
{
    public TCode Code { get; } = code;

    public override string CodeName => Code.ToString();
}