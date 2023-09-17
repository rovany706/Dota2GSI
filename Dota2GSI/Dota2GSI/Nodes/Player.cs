namespace Dota2GSI.Nodes;

public record Player(
    string Steamid,
    string Accountid,
    string Name,
    string Activity,
    int Kills,
    int Deaths,
    int Assists,
    int LastHits,
    int Denies,
    int KillStreak,
    int CommandsIssued,
    string TeamName,
    int PlayerSlot,
    int TeamSlot,
    uint Gold,
    uint GoldReliable,
    uint GoldUnreliable,
    uint GoldFromHeroKills,
    uint GoldFromCreepKills,
    uint GoldFromIncome,
    uint GoldFromShared,
    uint Gpm,
    uint Xpm
);