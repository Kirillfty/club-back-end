namespace ClubsBack.Entities
{
    public class Users
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nickName { get; set; }
        public string password { get; set; }
        public required string Role { get; set; }
        public string? RefreshToken { get; set; }

    }
}
