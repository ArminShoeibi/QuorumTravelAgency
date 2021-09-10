using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection;
public static class RabbitMQServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQConnection(this IServiceCollection services)
    {
        ConnectionFactory amqpConnectionFactory = new()
        {
            DispatchConsumersAsync = true,
            AutomaticRecoveryEnabled = true,
            TopologyRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
            Password = ConnectionFactory.DefaultPass,
            UserName = ConnectionFactory.DefaultUser,
            HostName = "localhost",
        };

        IConnection amqpConnection = amqpConnectionFactory.CreateConnection();
        services.AddSingleton(amqpConnection);
        return services;
    }
}
