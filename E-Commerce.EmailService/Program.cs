using MailKit.Net.Smtp;
using MimeKit;
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
            Console.ReadLine();
        }

        /// <summary>
        /// Bu metot RabbitMQ üstündeki "emailConfirmation" queue'sunu dinler, ilgili queue üstünden gelene email bilgisine email gönderir.
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

                var newUserEmail = message;
                Console.WriteLine($"Mesaj alındı => Email: {newUserEmail}");

                var result = SendEmail(newUserEmail);
                Console.WriteLine(result);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        /// <summary>
        /// Bu metot email oluşturur ve gönderir.
        /// </summary>
        /// <param name="newUserEmail"></param>
        /// <returns>Durum mesajı döner.</returns>
        public static string SendEmail(string newUserEmail)
        {
            string senderAddress = "booksellerproject@outlook.com";
            string recipientAddress = newUserEmail;
            string subject = "Email Onayı";
            string body = $"<p>Hesabınızı aktifleştirmek için <a href=\"https://localhost:7240/api/auth/user-activate?userEmail={newUserEmail}\">buraya</a> tıklayın.</p>";

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
