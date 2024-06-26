namespace E_Commerce.OrderAPI.Models.Dto
{
    public class ApproveEmailDto
    {
        public string ToEmail { get; set; }
        public Guid OrderId { get; set; }
        public string Subject
        {
            get { return "Sipariş Onayı - BookSeller Project"; }
        }
        public object Body
        {
            get
            {
                return @$"
                    <div style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">
                        <table role=""presentation"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
                            <tr>
                                <td style=""background-color: #f8f8f8; padding: 40px 0; text-align: center;"">
                                    <h2 style=""color: #333333;"">Sipariş Onayı</h2>
                                    <p style=""color: #666666;""><strong>{OrderId}<strong/> numaralı siparişini onaylanmıştır.</p>
                                </td>
                            </tr>
                            <tr>
                                <td style=""background-color: #333333; color: #ffffff; padding: 20px 0; text-align: center;"">
                                    <p style=""font-size: 14px; margin: 0;"">Bu e-posta otomatik olarak gönderilmiştir. Lütfen cevaplamayınız.</p>
                                </td>
                            </tr>
                        </table>
                    </div>
                ";
            }
        }

    }
}
