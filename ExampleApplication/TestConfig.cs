using Newtonsoft.Json;
using RickSoft.Config;

namespace ExampleApplication
{
    public class TestConfig : JsonConfig
    {
        [JsonProperty("Test")]
        public string Test { get; set; } = "default value";

        [JsonProperty("Name")]
        public string Name { get; set; } = "Jan";
    }
}
