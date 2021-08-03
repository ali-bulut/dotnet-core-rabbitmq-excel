using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FileCreateWorkerService.Services
{
    public class RabbitMQClientService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        public static string QueueName = "queue-excel-file";
        private readonly ILogger<RabbitMQClientService> _logger;

        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            if (_channel?.IsOpen == true)
            {
                return _channel;
            }

            _channel = _connection.CreateModel();

            _logger.LogInformation("Connection has been established with RabbitMQ.");

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _channel = default;

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("Connection has been disposed with RabbitMQ.");
        }
    }
}
