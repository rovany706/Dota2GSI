using Dota2GSI.Json;
using Dota2GSI.Nodes;
using Dota2GSI.Nodes.Abilities;
using Dota2GSI.Nodes.Events;
using Dota2GSI.Nodes.Items;

namespace Dota2GSI.UnitTests.Nodes;

public class GameStateTests : JsonDeserializeTests
{
    private readonly GameState testGameState = new(
        new Provider("Dota 2", 570, 47, 1694530163),
        new Map("start", "7331674997", 126, 25, true, false, 0, 0, DotaGameState.DOTA_GAMERULES_STATE_GAME_IN_PROGRESS, false,
            PlayerTeam.None, "", 0),
        new Player("11111111111111111", "11111111", "ayylmao", "playing", 0, 0, 0, 1, 0, 0, 122, "radiant", 1, 0,
            70127, 38, 70089, 0, 0, 38, 0, 2372369, 152723),
        new DotaAbilities(new[]
        {
            new DotaAbility("tusk_ice_shards", 4, true, false, true, 0, false),
            new DotaAbility("tusk_snowball", 4, true, false, true, 0, false),
            new DotaAbility("tusk_tag_team", 4, true, false, true, 0, false),
            new DotaAbility("tusk_walrus_punch", 3, true, false, true, 0, true),
            new DotaAbility("plus_high_five", 1, true, false, true, 0, false),
            new DotaAbility("plus_guild_banner", 1, true, false, true, 0, false),
            new DotaAbility("seasonal_10th_anniversary_party_hat", 1, true, false, true, 0, false),
        }),
        new Hero(7711, -7878, 100, "npc_dota_hero_tusk", 30, 64400, true, 0, 7929, 0, 3736, 3745, 99, 1047, 1047, 100,
            false, false, false, false, false, false, false, false, false, false, false, true, true, true, true, true,
            true, true, true, 7),
        new Auth("token"),
        new DotaItems(new[]
            {
                new DotaItem("item_octarine_core", 0, 1, false, 0, true, 0),
                new DotaItem("item_blink", 0, 1, true, 0, false, 0),
                new DotaItem("item_travel_boots", 0, 1, false, 0, true, 0),
                new DotaItem("item_black_king_bar", 0, 1, true, 0, false, 0),
                new DotaItem("item_pipe", 0, 1, true, 0, false, 0),
                new DotaItem("empty", 0, 0, false, 0, false, 0),
                new DotaItem("item_tango", 0, 1, true, 0, false, 3),
                new DotaItem("item_magic_wand", 0, 1, false, 0, false, 0),
                new DotaItem("item_enchanted_mango", 0, 1, true, 0, false, 1)
            },
            new[]
            {
                new DotaItem("item_heavens_halberd", 0, 1, true, 0, false, 0),
                new DotaItem("item_crimson_guard", 0, 1, true, 0, false, 0),
                new DotaItem("item_lotus_orb", 0, 1, true, 0, false, 0),
                new DotaItem("item_silver_edge", 0, 1, true, 0, false, 0),
                new DotaItem("empty", 0, 0, false, 0, false, 0),
                new DotaItem("empty", 0, 0, false, 0, false, 0)
            },
            new DotaItem("item_tpscroll", 0, 1, false, 62, false, 1),
            new DotaItem("item_timeless_relic", 0, 1, false, 0, true, 0)),
        new List<DotaEvent>
        {
            new AegisPickupEvent(121, DotaEventType.AegisPickup, 0, false),
            new RoshanKilledEvent(117, DotaEventType.RoshanKilled, "radiant", 0)
        }
    );

    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateProvider()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Provider, Is.EqualTo(this.testGameState.Provider));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateMap()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Map, Is.EqualTo(this.testGameState.Map));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStatePlayer()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Player, Is.EqualTo(this.testGameState.Player));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateHero()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Hero, Is.EqualTo(this.testGameState.Hero));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateAuth()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Auth, Is.EqualTo(this.testGameState.Auth));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateAbilities()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Abilities, Is.EquivalentTo(this.testGameState.Abilities));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateEvents()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Events, Is.EquivalentTo(this.testGameState.Events));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateDotaMainItems()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Items.MainItems, Is.EquivalentTo(this.testGameState.Items.MainItems));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateDotaStashItems()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Items.StashItems, Is.EquivalentTo(this.testGameState.Items.StashItems));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateItemsTeleport()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Items.Teleport, Is.EqualTo(this.testGameState.Items.Teleport));
    }
    
    [Test]
    public void Deserialize_Always_ReturnExpectedGameStateItemsNeutral()
    {
        var json = LoadFile("gamestate.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Items.Neutral, Is.EqualTo(this.testGameState.Items.Neutral));
    }
}