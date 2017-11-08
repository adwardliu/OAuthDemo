namespace OAuthDemo.Domain
{
    public class Client
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
    }
}