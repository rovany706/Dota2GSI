#nullable enable

using System.Collections.Generic;
using Dota2GSI.Nodes;
using Dota2GSI.Nodes.Abilities;
using Dota2GSI.Nodes.Events;
using Dota2GSI.Nodes.Items;

namespace Dota2GSI;

public record GameState(
    Provider Provider,
    Map? Map,
    Player? Player,
    DotaAbilities? Abilities,
    Hero? Hero,
    Auth Auth,
    DotaItems? Items,
    List<DotaEvent> Events
);