namespace Restaurants.Domain.Entities;

public class ContactInformation
{
    public string? MainEmail { get; set; }

    public string? MainPhoneNumber { get; set; }
    
    public List<string>? ExtraEmails { get; set; }
    
    public List<string>? ExtraPhoneNumbers { get; set; }
}