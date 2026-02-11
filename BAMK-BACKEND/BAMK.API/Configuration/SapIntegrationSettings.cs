namespace BAMK.API.Configuration
{
    public class SapIntegrationSettings
    {
        public const string SectionName = "SapIntegration";

        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string OrderSyncEndpoint { get; set; } = "/orders/sync";
        public int TimeoutSeconds { get; set; } = 30;
    }
}
