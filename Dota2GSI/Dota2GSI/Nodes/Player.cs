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
    int Gold,
    int GoldReliable,
    int GoldUnreliable,
    int GoldFromHeroKills,
    int GoldFromCreepKills,
    int GoldFromIncome,
    int GoldFromShared,
    int Gpm,
    int Xpm
);