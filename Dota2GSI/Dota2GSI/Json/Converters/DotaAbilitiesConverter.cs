using System;
using Dota2GSI.Nodes.Abilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dota2GSI.Json.Converters;

public class DotaAbilitiesConverter : BaseDotaConverter<DotaAbilities, DotaAbility>
{
    private const string Prefix = "ability";
    private const string RootKey = "abilities";

    public override DotaAbilities ReadJson(JsonReader reader, Type objectType, DotaAbilities existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var rootJObject = JObject.Load(reader);
        var abilities = GetItems(Prefix, serializer, rootJObject);
        
        return new DotaAbilities(abilities);
    }
}