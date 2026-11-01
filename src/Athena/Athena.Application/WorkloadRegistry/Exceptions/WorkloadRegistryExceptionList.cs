namespace Athena.Application.WorkloadRegistry.Exceptions;

public sealed class WorkloadNotFoundException(Guid workloadId) : WorkloadRegistryException(WorkloadRegistryErrorCode.WorkloadNotFound, $"Workload with ID '{workloadId}' was not found.");

public sealed class WorkloadNotFoundByNameException(Guid blockId, string name) : WorkloadRegistryException(WorkloadRegistryErrorCode.WorkloadNotFoundByName, $"Workload '{name}' was not found in block '{blockId}'.");

public sealed class WorkloadNameAlreadyExistsException(Guid blockId, string name) : WorkloadRegistryException(WorkloadRegistryErrorCode.WorkloadNameAlreadyExists, $"Workload '{name}' already exists in block '{blockId}'.");