using Dota2GSI.Json;
using Dota2GSI.Nodes.Items;

namespace Dota2GSI.UnitTests.Nodes.Items;

public class DotaItemsTests : JsonDeserializeTests
{
    private readonly IEnumerable<DotaItem> testMainItems = new[]
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
    };

    private readonly IEnumerable<DotaItem> testStashItems = new[]
    {
        new DotaItem("item_heavens_halberd", 0, 1, true, 0, false, 0),
        new DotaItem("item_crimson_guard", 0, 1, true, 0, false, 0),
        new DotaItem("item_lotus_orb", 0, 1, true, 0, false, 0),
        new DotaItem("item_silver_edge", 0, 1, true, 0, false, 0),
        new DotaItem("empty", 0, 0, false, 0, false, 0),
        new DotaItem("empty", 0, 0, false, 0, false, 0)
    };

    [Test]
    public void Deserialize_Always_ReturnExpectedMainItems()
    {
        var json = LoadFile("items.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Items.MainItems, Is.EquivalentTo(this.testMainItems));
    }

    [Test]
    public void Deserialize_Always_ReturnExpectedStashItems()
    {
        var json = LoadFile("items.json");
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Items.StashItems, Is.EquivalentTo(this.testStashItems));
    }

    [Test]
    public void Deserialize_Always_ReturnExpectedNeutral()
    {
        var json = LoadFile("items.json");
        var expected = new DotaItem("item_timeless_relic", 0, 1, false, 0, true, 0);
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Items.Neutral, Is.EqualTo(expected));
    }

    [Test]
    public void Deserialize_Always_ReturnExpectedTeleport()
    {
        var json = LoadFile("items.json");
        var expected = new DotaItem("item_tpscroll", 0, 1, false, 62, false, 1);
        var serializer = new Serializer();
        
        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Items.Teleport, Is.EqualTo(expected));
    }
}