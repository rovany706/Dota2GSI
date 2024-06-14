using System;
using Dota2GSI.Nodes.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dota2GSI.Json.Converters;

public class DotaEventsConverter : BaseDotaConverter<DotaEvent>
{
    public override DotaEvent ReadJson(JsonReader reader, Type objectType, DotaEvent existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        var eventType = obj["event_type"]!.ToObject<DotaEventType>();
        var outputDotaEvent = CreateByEventType(eventType);

        if (outputDotaEvent is UnknownEvent unknownEvent)
        {
            return unknownEvent with { Json = obj.ToString(Formatting.None) };
        }

        using var subReader = obj.CreateReader();
        serializer.Populate(subReader, outputDotaEvent);

        return outputDotaEvent;
    }

    private static DotaEvent CreateByEventType(DotaEventType eventType)
    {
        return eventType switch
        {
            DotaEventType.BountyPickup => new BountyPickupEvent(default, default, default, default, default, default),
            DotaEventType.RoshanKilled => new RoshanKilledEvent(default, default, default, default),
            DotaEventType.AegisPickup => new AegisPickupEvent(default, default, default, default),
            DotaEventType.AegisDenied => new AegisDeniedEvent(default, default, default),
            DotaEventType.Tip => new TipEvent(default, default, default, default, default),
            _ => new UnknownEvent(default, default, default)
        };
    }
}