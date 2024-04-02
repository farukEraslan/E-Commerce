namespace E_Commerce.EmailService
{
    public class CampaignDto
    {
        public List<string> ToEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
