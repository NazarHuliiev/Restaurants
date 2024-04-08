namespace Restaurants.Domain.Exceptions;

public class NotFoundException(string resourceType, string resourceId)
    : Exception($"Resource {resourceType} with Id {resourceId} not found");