using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemManagement.Exceptions;

public abstract class SystemManagementApplicationException(
    SystemManagementApplicationErrorCode code,
    string message)
    : ApplicationException<SystemManagementApplicationErrorCode>(code, message);


public enum SystemManagementApplicationErrorCode
{
    InvalidDesiredState = 1,
    InvalidOperationalStatus = 2,
    InvalidOperationType = 3,
    InvalidOperationStatus = 4,

    SystemNotFound = 5,
    SystemLifecycleAlreadyExists = 6,
    SystemLifecycleNotFound = 7,
    SystemLifecycleOperationNotFound = 8
}