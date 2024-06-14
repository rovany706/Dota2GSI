using Dota2GSI.Json;
using Dota2GSI.Nodes.Events;

namespace Dota2GSI.UnitTests.Nodes.Events;

public class DotaEventsTests : JsonDeserializeTests
{
    private readonly IEnumerable<DotaEvent> testDotaEvents = new DotaEvent[]
    {
        new BountyPickupEvent(841, DotaEventType.BountyPickup, 5, "dire", 45, 225),
        new AegisPickupEvent(121, DotaEventType.AegisPickup, 0, false),
        new RoshanKilledEvent(117, DotaEventType.RoshanKilled, "radiant", 0),
        new AegisDeniedEvent(150, DotaEventType.AegisDenied, 5),
        new TipEvent(539, DotaEventType.Tip, 7, 1, 50)
    };

    [Test]
    public void Deserialize_Always_ReturnExpectedEvents()
    {
        var json = LoadFile("events.json");
        var serializer = new Serializer();

        var actual = serializer.Deserialize<GameState>(json);

        Assert.That(actual.Events, Is.EquivalentTo(this.testDotaEvents));
    }
}