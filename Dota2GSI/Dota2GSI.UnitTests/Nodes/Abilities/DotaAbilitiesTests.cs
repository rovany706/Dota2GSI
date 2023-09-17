using Dota2GSI.Json;
using Dota2GSI.Nodes.Abilities;

namespace Dota2GSI.UnitTests.Nodes.Abilities;

public class DotaAbilitiesTests : JsonDeserializeTests
{
    private readonly IEnumerable<DotaAbility> testAbilities = new[]
    {
        new DotaAbility("pudge_meat_hook", 2, true, false, true, 0, false),
        new DotaAbility("pudge_rot", 2, true, false, true, 0, false),
        new DotaAbility("pudge_flesh_heap", 1, true, false, true, 0, false),
        new DotaAbility("pudge_dismember", 1, true, false, true, 0, true),
        new DotaAbility("plus_high_five", 1, true, false, true, 0, false),
        new DotaAbility("plus_guild_banner", 1, true, false, true, 0, false),
        new DotaAbility("seasonal_10th_anniversary_party_hat", 1, true, false, true, 0, false),
    };

    [Test]
    public void Deserialize_Always_ReturnExpectedAbilities()
    {
        var json = LoadFile("abilities.json");
        var serializer = new Serializer();

        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Abilities, Is.EquivalentTo(this.testAbilities));
    }
}