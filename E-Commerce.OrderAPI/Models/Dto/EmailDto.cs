using E_Commerce.OrderAPI.Models.Dto.Cart;

namespace E_Commerce.OrderAPI.Models.Dto
{
    public class EmailDto
    {
        public string ToEmail { get; set; }
        public string Subject
        {
            get { return "Siparişiniz - BookSeller Project"; }
        }
        public CartDto? Cart { get; set; }
        public object Body
        {
            get
            {
                return @$"
                        <!DOCTYPE html>
                        <html lang=""tr"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Sipariş Tablosu</title>
                            <style>
                                table {{
                                    border-collapse: collapse;
                                    width: 100%;
                                    margin-bottom: 20px;
                                }}
                                th, td {{
                                    border: 1px solid #dddddd;
                                    text-align: left;
                                    padding: 8px;
                                }}
                                th {{
                                    background-color: #f2f2f2;
                                }}
                                img {{
                                    width: 75px;
                                    height: 100px;
                                }}
                                .btn {{
                                    display: inline-block;
                                    padding: 4px 10px;
                                    background-color: #007bff;
                                    color: #ffffff;
                                    text-align: center;
                                    text-decoration: none;
                                    border-radius: 3px;
                                }}
                                .btn-warning {{
                                    background-color: #ffc107;
                                }}
                                .disabled {{
                                    pointer-events: none;
                                    opacity: 0.65;
                                }}
                            </style>
                        </head>
                        <body>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Kitap Adı</th>
                                        <th>Sipariş Adedi</th>
                                        <th>Birim Fiyat</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    @foreach (var product in Model.CartLines)
                                    {{
                                        <tr>
                                            <td>@product.Product.ProductName</td>
                                            <td>@product.Quantity Adet</td>
                                            <td>@product.Product.UnitPrice TL</td>
                                        </tr>
                                    }}
                                </tbody>
                            </table>
                        </body>
                        </html>

                ";
            }

            set
            {
                Body = Cart;
            }
        }
    }
}
