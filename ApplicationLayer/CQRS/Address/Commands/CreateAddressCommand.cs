using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.Address.Commands;
public class CreateAddressCommand
{
    [Required, RegularExpression(@"^[a-zA-Z0-9\s]{2,50}$")]
    public string? Street { get; set; }

    [Required, RegularExpression(@"^[a-zA-Z\s]{2,50}$")]
    public string? City { get; set; }

    [Required, RegularExpression(@"^[a-zA-Z\s]{2,50}$")]
    public string? State { get; set; }

    [Required, RegularExpression(@"^\d{2,50}$")]
    public string? ZipCode { get; set; }
}
