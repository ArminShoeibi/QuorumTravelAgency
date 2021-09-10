using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using QuorumTravelAgency.Shared;
using QuorumTravelAgency.Shared.DTOs;
using QuorumTravelAgency.Shared.RabbitMQ.Enumerations;
using QuorumTravelAgency.Shared.RabbitMQ.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QuorumTravelAgency.API.RabbitMQPublishers;
public class AvailableFlightsV2Publisher
{
    private readonly ILogger<AvailableFlightsV2Publisher> _logger;
    private readonly IConnection _amqpConnection;
    private readonly IModel _amqpChannel;

    public AvailableFlightsV2Publisher(ILogger<AvailableFlightsV2Publisher> logger,
                                     IConnection amqpConnection)
    {
        _logger = logger;
        _amqpConnection = amqpConnection;
        _amqpChannel = _amqpConnection.CreateModel();

        _amqpChannel.DeclareAvailableFlightsExchangeAndQueues();

        //Enabling Publisher Confirms on Channel
        _amqpChannel.ConfirmSelect();

        /* asynchronous handling: best performance and use of resources,
           good control in case of error,
           but can be involved to implement correctly.
        */
        _amqpChannel.BasicAcks += HandleAmqpChannelBasicAcks;

        _amqpChannel.BasicNacks += HandleAmqpChannelBasicNacks;

    }

    private void HandleAmqpChannelBasicNacks(object sender, BasicNackEventArgs e)
    {
        _logger.LogWarning("Nacked: {DeliveryTag} {Multiple}", e.DeliveryTag, e.Multiple);
    }

    private void HandleAmqpChannelBasicAcks(object sender, BasicAckEventArgs e)
    {
        _logger.LogInformation("Acked: {DeliveryTag} {Multiple}", e.DeliveryTag, e.Multiple);
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
    }
}
