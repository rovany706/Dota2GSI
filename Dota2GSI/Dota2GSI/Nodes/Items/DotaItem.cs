namespace Dota2GSI.Nodes.Items;

public record DotaItem(
    string Name,
    int Purchaser,
    int ItemLevel,
    bool CanCast,
    int Cooldown,
    bool Passive,
    int Charges
);