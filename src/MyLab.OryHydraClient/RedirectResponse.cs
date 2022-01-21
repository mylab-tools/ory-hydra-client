using Newtonsoft.Json;

namespace MyLab.OryHydraClient
{
    public class RedirectResponse
    {
        [JsonProperty("redirect_to")]
        public string RedirectTo { get; set; }
    }
}