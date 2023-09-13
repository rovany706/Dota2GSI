using Newtonsoft.Json;

namespace Dota2GSI.Nodes;

public record Hero(
    int Xpos,
    int Ypos,
    int Id,
    string Name,
    int Level,
    int Xp,
    bool Alive,
    int RespawnSeconds,
    int BuybackCost,
    int BuybackCooldown,
    int Health,
    int MaxHealth,
    int HealthPercent,
    int Mana,
    int MaxMana,
    int ManaPercent,
    bool Silenced,
    bool Stunned,
    bool Disarmed,
    bool Magicimmune,
    bool Hexed,
    bool Muted,
    bool Break,
    bool AghanimsScepter,
    bool AghanimsShard,
    bool Smoked,
    bool HasDebuff,
    [JsonProperty("talent_1")]
    bool Talent1,
    [JsonProperty("talent_2")]
    bool Talent2,
    [JsonProperty("talent_3")]
    bool Talent3,
    [JsonProperty("talent_4")]
    bool Talent4,
    [JsonProperty("talent_5")]
    bool Talent5,
    [JsonProperty("talent_6")]
    bool Talent6,
    [JsonProperty("talent_7")]
    bool Talent7,
    [JsonProperty("talent_8")]
    bool Talent8,
    int AttributesLevel
);