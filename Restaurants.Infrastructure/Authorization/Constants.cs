namespace Restaurants.Infrastructure.Authorization;

public static class PolicyNames
{
    public const string HasCurrentCountry = "HasCurrentCountry";
    public const string AlLeast20 = "AlLeast20";
}

public static class AppClaimTypes
{
    public const string CurrentCountry = "CurrentCountry";
    public const string DateOfBirth = "DateOfBirth";
}