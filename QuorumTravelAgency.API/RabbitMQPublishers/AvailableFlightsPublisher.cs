using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using QuorumTravelAgency.Shared;
using QuorumTravelAgency.Shared.DTOs;
using QuorumTravelAgency.Shared.RabbitMQ.Enumerations;
using QuorumTravelAgency.Shared.RabbitMQ.Extensions;
using RabbitMQ.Client;

namespace QuorumTravelAgency.API.RabbitMQPublishers;
public class AvailableFlightsPublisher
{
    private readonly ILogger<AvailableFlightsPublisher> _logger;
    private readonly IConnection _amqpConnection;
    private readonly IModel _amqpChannel;

    public AvailableFlightsPublisher(ILogger<AvailableFlightsPublisher> logger,
                                     IConnection amqpConnection)
    {
        _logger = logger;
        _amqpConnection = amqpConnection;
        _amqpChannel = _amqpConnection.CreateModel();

        _amqpChannel.DeclareAvailableFlightsExchangeAndQueues();

        //Enabling Publisher Confirms on Channel
        _amqpChannel.ConfirmSelect();
    }


    public void PublishAvailableFlightRequest(AvailableFlightsRequestDto availableFlightsRequestDto)
    {
        AvailableFlightsRequest availableFlightsRequest = new()
        {
            Origin = availableFlightsRequestDto.Origin,
            Destination = availableFlightsRequestDto.Destination,
            DepartureDate = Timestamp.FromDateTime(availableFlightsRequestDto.DepartureDate.Date)
        };

        _logger.LogInformation($"Publishing Available Flights Request: {availableFlightsRequest}");

        _amqpChannel.BasicPublish(nameof(ExchangeName.AvailableFlightsExchange),
                                  "",
                                  null,
                                  availableFlightsRequest.ToByteArray());


        // publishing messages individually, waiting for the confirmation synchronously: simple, but very limited throughput
        _amqpChannel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(20));
    }
}
