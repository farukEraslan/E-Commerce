namespace E_Commerce.Web.Models.Dto
{
    public class ConnectedUser
    {
        public static Dictionary<string, ConnectedUserInfo> UsersId { get; set; } = new Dictionary<string, ConnectedUserInfo>();
    }

    public class ConnectedUserInfo
    {
        public string Browser { get; set; }
        public string UserId { get; set; }
    }
}
