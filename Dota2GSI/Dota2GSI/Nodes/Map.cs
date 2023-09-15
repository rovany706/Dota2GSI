using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dota2GSI.Nodes;

/// <summary>
/// Enum list for each Game State
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum DotaGameState
{
    /// <summary>
    /// Undefined
    /// </summary>
    Undefined,

    /// <summary>
    /// Disconnected
    /// </summary>
    DOTA_GAMERULES_STATE_DISCONNECT,

    /// <summary>
    /// Game is in progress
    /// </summary>
    DOTA_GAMERULES_STATE_GAME_IN_PROGRESS,

    /// <summary>
    /// Players are currently selecting heroes
    /// </summary>
    DOTA_GAMERULES_STATE_HERO_SELECTION,

    /// <summary>
    /// Game is starting
    /// </summary>
    DOTA_GAMERULES_STATE_INIT,

    /// <summary>
    /// Game is ending
    /// </summary>
    DOTA_GAMERULES_STATE_LAST,

    /// <summary>
    /// Game has ended, post game scoreboard
    /// </summary>
    DOTA_GAMERULES_STATE_POST_GAME,

    /// <summary>
    /// Game has started, pre game preparations
    /// </summary>
    DOTA_GAMERULES_STATE_PRE_GAME,

    /// <summary>
    /// Players are selecting/banning heroes
    /// </summary>
    DOTA_GAMERULES_STATE_STRATEGY_TIME,

    /// <summary>
    /// Waiting for everyone to connect and load
    /// </summary>
    DOTA_GAMERULES_STATE_WAIT_FOR_PLAYERS_TO_LOAD,

    /// <summary>
    /// Game is a custom game
    /// </summary>
    DOTA_GAMERULES_STATE_CUSTOM_GAME_SETUP,
    
    DOTA_GAMERULES_STATE_WAIT_FOR_MAP_TO_LOAD
}

/// <summary>
/// Enum list for each player team
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum PlayerTeam
{
    /// <summary>
    /// Undefined
    /// </summary>
    Undefined,

    /// <summary>
    /// No team
    /// </summary>
    None,

    /// <summary>
    /// Dire team
    /// </summary>
    Dire,

    /// <summary>
    /// Radiant team
    /// </summary>
    Radiant
}

public record Map(
    string Name,
    string Matchid,
    int GameTime,
    int ClockTime,
    bool Daytime,
    bool NightstalkerNight,
    int RadiantScore,
    int DireScore,
    DotaGameState GameState,
    bool Paused,
    PlayerTeam WinTeam,
    string Customgamename,
    int WardPurchaseCooldown
);