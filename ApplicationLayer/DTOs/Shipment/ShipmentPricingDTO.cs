using ApplicationLayer.DTOs;

public record DeliveryRange(int Min, 
    int Max);

public record Dimensions(int Height, 
    int Width, 
    int Length);

public record Package(string Price, 
    string Discount, 
    string Format, 
    string Weight, 
    string InsuranceValue, 
    Dimensions Dimensions);

public record AdditionalServices(bool Receipt, 
    bool OwnHand, 
    bool Collect);

public record Company(int Id, 
    string Name, 
    string Picture);

public record ShipmentPricingDTO(int Id, 
    string Name, 
    string Price, 
    string CustomPrice, 
    string Discount, 
    string Currency, 
    int DeliveryTime, 
    DeliveryRange DeliveryRange, 
    int CustomDeliveryTime, 
    DeliveryRange CustomDeliveryRange, 
    List<Package> Packages, 
    AdditionalServices AdditionalServices, 
    Company Company, string Error) : ServiceResponse(true, string.Empty, DateTime.UtcNow);
