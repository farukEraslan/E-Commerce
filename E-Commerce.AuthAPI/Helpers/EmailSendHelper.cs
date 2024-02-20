using RabbitMQ.Client;
using System.Text;

namespace E_Commerce.AuthAPI.Helpers
{
    public static class EmailSendHelper
    {
        public static string SendEmailProducer(string newUserEmail)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "emailConfirmation";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            string message = $"{newUserEmail}";
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            return $"E-posta gönderim isteği başarıyla gönderildi: {message}";
        }
    }
}
