namespace BlazorProducts.Server.Context.Configuration
{
    public class RedisConfiguration
    {
        public bool Enabled { get; set; }
        public string RedisConnectionString { get; set; }
    }
}
