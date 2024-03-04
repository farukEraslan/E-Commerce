namespace E_Commerce.EmailService
{
    public class EmailDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public object Body { get; set; }
    }
}
