using System;
using Dota2GSI.Nodes.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dota2GSI.Json.Converters;

public class DotaItemsConverter : BaseDotaConverter<DotaItems, DotaItem>
{
    private const string RootKey = "items";
    private const string SlotPrefix = "slot";
    private const string StashPrefix = "stash";
    private const string TeleportKey = "teleport0";
    private const string NeutralKey = "neutral0";

    public override DotaItems ReadJson(JsonReader reader, Type objectType, DotaItems existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var rootJObject = JObject.Load(reader);
        var items = GetItems(SlotPrefix, serializer, rootJObject);
        var stashItems = GetItems(StashPrefix, serializer, rootJObject);
        var teleport = GetItem(TeleportKey, rootJObject, serializer);
        var neutral = GetItem(NeutralKey, rootJObject, serializer);
        
        return new DotaItems(items, stashItems, teleport, neutral);
    }
}