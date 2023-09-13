using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dota2GSI.Nodes.Events;

[JsonObject(Title = "events")]
public class DotaEvents : IEnumerable<DotaEvent>
{
    public List<DotaEvent> Events { get; } = new();

    public IEnumerator<DotaEvent> GetEnumerator()
    {
        return Events.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}