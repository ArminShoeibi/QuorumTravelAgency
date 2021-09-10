using System.ComponentModel.DataAnnotations;

namespace QuorumTravelAgency.Shared.DTOs;
public record AvailableFlightsRequestDto
{
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Origin { get; init; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Destination { get; init; }

    public DateTime DepartureDate { get; init; }
}
