using E_Commerce.Web.Models.Dto.Campaign;
using E_Commerce.Web.Services.IServices;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace E_Commerce.Web.Controllers
{
    public class CampaignController : Controller
    {
        private readonly IAuthService _authService;

        public CampaignController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View(new CampaignDto());
        }

        /// <summary>
        /// Kampanya duyurularını yollayan metot.
        /// </summary>
        /// <param name="campaignDto"></param>
        /// <returns></returns>
        public async Task<IActionResult> Send(CampaignDto campaignDto)
        {
            if (campaignDto is null)
            {
                return View();
            }

            var customerEmails = await _authService.GetCustomerEmails();
            campaignDto.ToEmails = JsonConvert.DeserializeObject<List<string>>(customerEmails.Result.ToString());

            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "campaign";
            channel.ExchangeDeclare(exchange: "campaign", type: ExchangeType.Fanout);

            var message = JsonConvert.SerializeObject(campaignDto);
            var content = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "campaign", routingKey: queueName, basicProperties: null, body: content);

            TempData["success"] = $"E-posta gönderim isteği başarıyla gönderildi: {message}";
            return RedirectToAction("Index", "Campaign");
        }
    }
}
