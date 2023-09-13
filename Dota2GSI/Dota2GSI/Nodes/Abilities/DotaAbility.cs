namespace Dota2GSI.Nodes.Abilities;

public record DotaAbility(
    string Name,
    int Level,
    bool CanCast,
    bool Passive,
    bool AbilityActive,
    int Cooldown,
    bool Ultimate
);