namespace E_Commerce.AuthAPI.Models.Dto
{
    public class EmailDto
    {
        public string ToEmail { get; set; }
        public string Subject
        {
            get { return "Hesabınızı Aktifleştirin - BookSeller Project"; }
        }
        public object Body
        {
            get { return @$"
                    <div style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">
                        <table role=""presentation"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
                            <tr>
                                <td style=""background-color: #f8f8f8; padding: 40px 0; text-align: center;"">
                                    <h2 style=""color: #333333;"">E-posta Onayı</h2>
                                    <p style=""color: #666666;"">Hesabınızı aktifleştirmek için lütfen aşağıdaki butona tıklayın:</p>
                                    <p style=""margin-top: 30px;""><a href=""https://localhost:7284/auth/activate?userEmail={ToEmail}"" style=""background-color: #4CAF50; border:  none;      color:     white;   padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px;"">Onayla</ a></p>
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
