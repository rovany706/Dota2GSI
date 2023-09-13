using System.Collections.Generic;
using System.IO;
using Dota2GSI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dota2GSI.Json;

public class Serializer
{
    private readonly JsonSerializer serializer = CreateSerializer();

    private static JsonSerializer CreateSerializer()
    {
        var serializer = JsonSerializer.Create(new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new DotaAbilitiesConverter(),
                new DotaEventsConverter(),
                new DotaItemsConverter()
            },
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        });

        return serializer;
    }

    public T Deserialize<T>(string json)
    {
        using var textReader = new StringReader(json);
        using var jsonReader = new JsonTextReader(textReader);
        
        return this.serializer.Deserialize<T>(jsonReader);
    }
}