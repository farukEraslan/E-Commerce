using E_Commerce.OrderAPI.Models.Dto;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace E_Commerce.OrderAPI.Helpers
{
    public static class EmailSendHelper
    {
        public static string SendEmailProducer(string toEmail, Guid orderId, bool isApproved)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            string message = string.Empty;
            byte[] content;

            string queueName = "orderConfirmation";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            if (isApproved)
            {
                // json data yollanacak.
                ApproveEmailDto emailDto = new()
                {
                    ToEmail = toEmail,
                    OrderId = orderId
                };
                message = JsonConvert.SerializeObject(emailDto);
                content = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: content);
            }
            else if (!isApproved)
            {
                // json data yollanacak.
                RejectedEmailDto emailDto = new()
                {
                    ToEmail = toEmail,
                    OrderId = orderId
                };
                message = JsonConvert.SerializeObject(emailDto);
                content = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: content);
            }
            //var message =  JsonConvert.SerializeObject(emailDto);
            //var content = Encoding.UTF8.GetBytes(message);
            //channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: content);

            return $"E-posta gönderim isteği başarıyla gönderildi: {message}";
        }
    }
}
