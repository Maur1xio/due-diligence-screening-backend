namespace EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

public class Provider
{
    public long ProviderId { get; set; }
    public long UserId { get; set; }
    public string BusinessName { get; set; } = null!;
    public string TradeName { get; set; } = null!;
    public string TaxId { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Website { get; set; } = string.Empty;
    public string PhysicalAddress { get; set; } = null!;
    public string Country { get; set; } = null!;
    public decimal AnnualBillingUsd { get; set; }
    public DateTime LastEditedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public ScreeningHistory? ScreeningHistory { get; set; }

    public Provider() { }

    public Provider(
        long userId,
        string businessName,
        string tradeName,
        string taxId,
        string phoneNumber,
        string email,
        string website,
        string physicalAddress,
        string country,
        decimal annualBillingUsd)
    {
        UserId = userId;
        BusinessName = businessName;
        TradeName = tradeName;
        TaxId = taxId;
        PhoneNumber = phoneNumber;
        Email = email;
        Website = website;
        PhysicalAddress = physicalAddress;
        Country = country;
        AnnualBillingUsd = annualBillingUsd;
        CreatedAt = DateTime.UtcNow;
        LastEditedAt = DateTime.UtcNow;
    }

    public void Update(
        string businessName,
        string tradeName,
        string taxId,
        string phoneNumber,
        string email,
        string website,
        string physicalAddress,
        string country,
        decimal annualBillingUsd)
    {
        BusinessName = businessName;
        TradeName = tradeName;
        TaxId = taxId;
        PhoneNumber = phoneNumber;
        Email = email;
        Website = website;
        PhysicalAddress = physicalAddress;
        Country = country;
        AnnualBillingUsd = annualBillingUsd;
        LastEditedAt = DateTime.UtcNow;
    }
}

