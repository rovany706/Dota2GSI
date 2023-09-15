using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dota2GSI.Nodes.Events;

[JsonConverter(typeof(StringEnumConverter))]
public enum DotaEventType
{
    [EnumMember(Value = "bounty_rune_pickup")]
    BountyPickup,
    [EnumMember(Value = "roshan_killed")] 
    RoshanKilled,
    [EnumMember(Value = "aegis_picked_up")]
    AegisPickup
}

public abstract record DotaEvent(
    int GameTime,
    DotaEventType EventType
);

public record BountyPickupEvent(
    int GameTime,
    DotaEventType EventType,
    int PlayerId,
    string Team,
    int BountyValue,
    int TeamGold
) : DotaEvent(GameTime, EventType);

public record RoshanKilledEvent(
    int GameTime,
    DotaEventType EventType,
    string KilledByTeam,
    int KillerPlayerId
) : DotaEvent(GameTime, EventType);

public record AegisPickupEvent(
    int GameTime,
    DotaEventType EventType,
    int PlayerId,
    bool Snatched
) : DotaEvent(GameTime, EventType);