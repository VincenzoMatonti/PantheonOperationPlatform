namespace Hermes.Application.Common;

public abstract class ApplicationException<TCode>(TCode code, string message) : Exception(message) where TCode : Enum
{
    public TCode Code { get; } = code;
}