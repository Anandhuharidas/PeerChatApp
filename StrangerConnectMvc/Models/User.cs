namespace StrangerConnectMvc.Models
{
    public class User
    {
        public string ConnectionId { get; set; }
        public string NickName { get; set; }
        public bool IsAvailable { get; set; }
        public string? PartnerConnectionId { get; set; }
    }
}
