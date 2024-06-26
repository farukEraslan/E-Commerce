﻿using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace E_Commerce.EmailService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EmailConfirmConsumer();
            OrderConfirmConsumer();
            CampaignConsumer();
            Console.ReadLine();
        }

        /// <summary>
        /// Bu metot RabbitMQ üstündeki "emailConfirmation" queue'sunu dinler, ilgili queue üstünden gelen email bilgisine email gönderir.
        /// </summary>
        public static void EmailConfirmConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "emailConfirmation";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // gelen json data atanacak.
                var emailDto = JsonConvert.DeserializeObject<EmailDto>(message);
                Console.WriteLine($"Mesaj alındı => toEmail: {emailDto.ToEmail} / subject: {emailDto.Subject} / body: {emailDto.Body.ToString()}");

                var result = SendEmail(emailDto);
                Console.WriteLine(result);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        /// <summary>
        /// Bu metot RabbitMQ üstündeki "orderConfirmation" queue'sunu dinler, ilgili queue üstünden gelen email bilgisine email gönderir.
        /// </summary>
        public static void OrderConfirmConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "orderConfirmation";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // gelen json data atanacak.
                var emailDto = JsonConvert.DeserializeObject<EmailDto>(message);
                Console.WriteLine($"Mesaj alındı => toEmail: {emailDto.ToEmail} / subject: {emailDto.Subject} / body: {emailDto.Body.ToString()}");

                var result = SendEmail(emailDto);
                Console.WriteLine(result);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        /// <summary>
        /// Bu metot RabbitMQ üstündeki "campaign" exchange'ini dinler, ilgili exchange üstünden gelen email bilgisine email gönderir.
        /// </summary>
        public static void CampaignConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            #region Fanout Queue Bind

            channel.ExchangeDeclare(exchange: "campaign", type: ExchangeType.Fanout);

            string queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "campaign", routingKey: queueName);

            #endregion

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // gelen json data atanacak.
                var emailDto = JsonConvert.DeserializeObject<CampaignDto>(message);
                Console.WriteLine($"Mesaj alındı => toEmail: {emailDto.ToEmails} / subject: {emailDto.Subject} / body: {emailDto.Body.ToString()}");

                foreach (var email in emailDto.ToEmails)
                {
                    EmailDto emailDto2 = new()
                    {
                        ToEmail = email,
                        Subject = emailDto.Subject,
                        Body = emailDto.Body
                    };
                    var result = SendEmail(emailDto2);
                    Console.WriteLine(result);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        /// <summary>
        /// Bu metot email oluşturur ve gönderir.
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string SendEmail(EmailDto emailDto)
        {
            string senderAddress = "booksellerproject@outlook.com";
            string recipientAddress = emailDto.ToEmail;
            string subject = emailDto.Subject;
            string body = emailDto.Body.ToString();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sender", senderAddress));
            message.To.Add(new MailboxAddress("Recipient", recipientAddress));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            // html body eklenecek.
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();
            client.Connect("smtp-mail.outlook.com", 587, false);
            client.Authenticate("booksellerproject@outlook.com", "bookSeller");
            client.Send(message);
            client.Disconnect(true);

            return $"E-posta başarıyla gönderildi:\n{message}";

        }
    }
}
