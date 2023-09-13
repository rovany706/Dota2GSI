using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dota2GSI.Nodes.Items;

[JsonObject(Title = "items")]
public class DotaItems
{
    private readonly List<DotaItem> mainItems;
    private readonly List<DotaItem> stashItems;

    public DotaItems()
    {
        this.mainItems = new List<DotaItem>();
        this.stashItems = new List<DotaItem>();
    }

    public DotaItems(IEnumerable<DotaItem> items, IEnumerable<DotaItem> stashItems, DotaItem teleport, DotaItem neutral)
    {
        Teleport = teleport;
        Neutral = neutral;
        this.mainItems = new List<DotaItem>(items);
        this.stashItems = new List<DotaItem>(stashItems);
    }

    public DotaItem Teleport { get; }

    public DotaItem Neutral { get; }

    public IReadOnlyList<DotaItem> MainItems => this.mainItems;

    public IReadOnlyList<DotaItem> StashItems => this.stashItems;
}