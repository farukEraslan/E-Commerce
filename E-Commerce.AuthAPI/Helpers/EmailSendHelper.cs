using E_Commerce.AuthAPI.Models.Dto;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace E_Commerce.AuthAPI.Helpers
{
    public static class EmailSendHelper
    {
        public static string SendEmailProducer(string toEmail)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "emailConfirmation";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // json data yollanacak.
            EmailDto emailDto = new()
            {
                ToEmail = toEmail
            };
            var message =  JsonConvert.SerializeObject(emailDto);
            var content = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: content);

            return $"E-posta gönderim isteği başarıyla gönderildi: {message}";
        }
    }
}
