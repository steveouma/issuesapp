using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace IssuesApp.Services
{
    public class RabbitMQProducer : IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "steveoyugi",
                Password = "steve@rabbit#TT"
            };

            using var connection = factory.CreateConnection();

            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare("issues",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "", 
                    routingKey: "issues",
                    basicProperties: properties,
                    body: body
                );
            }
        }
    }
}
