using Microsoft.AspNetCore.Mvc;
using QuorumTravelAgency.API.RabbitMQPublishers;
using QuorumTravelAgency.Shared.DTOs;

namespace QuorumTravelAgency.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AvailableFlightsController : ControllerBase
{
    private readonly AvailableFlightsPublisher _availableFlightsPublisher;

    public AvailableFlightsController(AvailableFlightsPublisher availableFlightsPublisher)
    {
       _availableFlightsPublisher = availableFlightsPublisher;
    }


    [HttpPost]
    public IActionResult PublishAvailableFlightsRequest(AvailableFlightsRequestDto availableFlightsRequestDto)
    {
        _availableFlightsPublisher.PublishAvailableFlightRequest(availableFlightsRequestDto);
        return Accepted();
    }
}
