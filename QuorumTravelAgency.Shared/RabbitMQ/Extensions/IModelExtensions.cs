using QuorumTravelAgency.Shared.RabbitMQ.Enumerations;
using RabbitMQ.Client;

namespace QuorumTravelAgency.Shared.RabbitMQ.Extensions;

public static class IModelExtensions
{
    public static void DeclareAvailableFlightsExchangeAndQueues(this IModel amqpChannel)
    {
        string availableFlightsExchange = nameof(ExchangeName.AvailableFlightsExchange);
        string availableFlightsQueue = nameof(QueueName.AvailableFlightsQueue);

        amqpChannel.ExchangeDeclare(availableFlightsExchange, ExchangeType.Fanout);

        Dictionary<string, object> queueArguments = new();
        queueArguments.Add("x-queue-type", "quorum");


        //  Quorum queues by design are replicated and durable,
        //  therefore the exclusive property makes no sense in their context.
        //  Therefore quorum queues cannot be exclusive.
        amqpChannel.QueueDeclare($"IranAir{availableFlightsQueue}",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 queueArguments);

        amqpChannel.QueueDeclare($"IranAirtour{availableFlightsQueue}",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 queueArguments);

        amqpChannel.QueueBind($"IranAir{availableFlightsQueue}", availableFlightsExchange, "");
        amqpChannel.QueueBind($"IranAirtour{availableFlightsQueue}", availableFlightsExchange, "");
    }
}
