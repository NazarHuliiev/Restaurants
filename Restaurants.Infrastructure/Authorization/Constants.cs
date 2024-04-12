namespace Restaurants.Infrastructure.Authorization;

public static class PolicyNames
{
    public const string HasCurrentCountry = "HasCurrentCountry";
}

public static class AppClaimTypes
{
    public const string CurrentCountry = "CurrentCountry";
    public const string DateOfBirth = "DateOfBirth";
}