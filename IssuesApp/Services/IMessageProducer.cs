using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace IssuesApp.Services
{
    public interface IMessageProducer
    {
        public void SendMessage<T>(T message);
    }
}
