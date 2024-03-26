﻿using E_Commerce.OrderAPI.Models.Dto;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace E_Commerce.OrderAPI.Helpers
{
    public static class EmailSendHelper
    {
        public static string SendEmailProducer(string toEmail, string subject, string body)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "orderConfirmation";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // json data yollanacak.
            EmailDto emailDto = new()
            {
                ToEmail = toEmail,
                Subject = subject,
                Body = body,
            };
            var message =  JsonConvert.SerializeObject(emailDto);
            var content = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: content);

            return $"E-posta gönderim isteği başarıyla gönderildi: {message}";
        }
    }
}