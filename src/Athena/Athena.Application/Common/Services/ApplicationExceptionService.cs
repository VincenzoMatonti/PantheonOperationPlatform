using System.Runtime.ExceptionServices;
using Athena.Application.Common.exceptions;
using Athena.Application.Common.Interfaces;

namespace Athena.Application.Common.Services;

public class ApplicationExceptionService(IEnumerable<IApplicationExceptionMapper> mappers)
{
    private readonly IEnumerable<IApplicationExceptionMapper> _mappers = mappers;

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (ApplicationCoreException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw MapException(exception);
        }
    }

    public async Task ExecuteAsync(Func<Task> operation)
    {
        try
        {
            await operation();
        }
        catch (ApplicationCoreException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw MapException(exception);
        }
    }

    public async Task<T> ExecuteWithRecoveryAsync<T>(Func<Task<T>> operation, Func<Exception, Task> recovery)
    {
        try
        {
            return await operation();
        }
        catch (Exception originalException)
        {
            await TryExecuteCleanupAsync(() => recovery(originalException), "Failed to execute exception recovery.");
            if (originalException is ApplicationCoreException) throw;
            throw MapException(originalException);
        }
    }

    public static async Task TryExecuteCleanupAsync(Func<Task> operation, string failureMessage)
    {
        try
        {
            await operation();
        }
        catch (Exception cleanupException)
        {
            System.Diagnostics.Trace.TraceError($"{failureMessage} {cleanupException}");
        }
    }

    private ApplicationCoreException MapException(Exception exception)
    {
        var mapper = _mappers.FirstOrDefault(candidate => candidate.CanMap(exception));
        if (mapper is null)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
            throw new InvalidOperationException("Codice non raggiungibile.");
        }

        return mapper.Map(exception);
    }
}