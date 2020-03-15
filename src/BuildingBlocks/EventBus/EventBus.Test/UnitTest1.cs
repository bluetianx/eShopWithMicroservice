using System;
using System.Text;
using System.Threading;
using Autofac;
using EventBus.Abstractions;
using EventBus.Test.Event;
using EventBus.Test.EventHandle;
using Xunit;
using Microsoft.Extensions.Logging;
using EventBusRabbitMQ;
using RabbitMQ.Client;
using EventBusRabbitMQ = EventBusRabbitMQ.EventBusRabbitMQ;

namespace EventBus.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            
            ILogger<DefaultRabbitMQPersistentConnection> logger = 
                loggerFactory.CreateLogger<DefaultRabbitMQPersistentConnection>();
            
            IConnectionFactory connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            
            IRabbitMQPersistentConnection  rabbitMqPersistentConnection = new 
                DefaultRabbitMQPersistentConnection(connectionFactory,logger);

            rabbitMqPersistentConnection.TryConnect();
            var isconnect = rabbitMqPersistentConnection.IsConnected;
            using (var channel = rabbitMqPersistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "testEX5",
                    type: "direct");

                channel.QueueDeclare(queue: "testQueue5",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                
                

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "testEX5",
                    routingKey: "testEX5",
                    basicProperties: null,
                    body: body);
            }
        }

        [Fact]
        public void Test2()
        {
            var manager = new InMemoryEventBusSubscriptionsManager();
            
            
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            
            ILogger<DefaultRabbitMQPersistentConnection> logger = 
                loggerFactory.CreateLogger<DefaultRabbitMQPersistentConnection>();
            
            IConnectionFactory connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            
            IRabbitMQPersistentConnection  rabbitMqPersistentConnection = new 
                DefaultRabbitMQPersistentConnection(connectionFactory,logger);


            var result = manager.HasSubscriptionsForEvent("TestEvent");
            
            ILogger<global::EventBusRabbitMQ.EventBusRabbitMQ> loggers = 
                loggerFactory.CreateLogger<global::EventBusRabbitMQ.EventBusRabbitMQ>();
            
            var builder = new ContainerBuilder();
            var container = builder.Build();
            var scope = container.BeginLifetimeScope();
            IEventBus eventBus= new global::EventBusRabbitMQ.EventBusRabbitMQ(rabbitMqPersistentConnection, loggers,
                scope,manager,"TestEvent");

            TestEvent testEvent = new TestEvent
            {
                Msg = "hello world"
            };
            
            eventBus.Subscribe<TestEvent,TestIntegrationEventHandler>();
            
        }
    }
}