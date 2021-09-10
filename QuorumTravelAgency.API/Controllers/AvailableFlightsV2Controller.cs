using Microsoft.AspNetCore.Mvc;
using QuorumTravelAgency.API.RabbitMQPublishers;
using QuorumTravelAgency.Shared.DTOs;

namespace QuorumTravelAgency.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AvailableFlightsV2Controller : ControllerBase
{
    private readonly AvailableFlightsV2Publisher _availableFlightsPublisher;

    public AvailableFlightsV2Controller(AvailableFlightsV2Publisher availableFlightsPublisher)
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
