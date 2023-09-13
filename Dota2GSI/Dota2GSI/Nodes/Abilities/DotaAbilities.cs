using System.Collections;
using System.Collections.Generic;

namespace Dota2GSI.Nodes.Abilities;

public class DotaAbilities : IEnumerable<DotaAbility>
{
    private readonly List<DotaAbility> abilities;

    public DotaAbilities()
    {
        this.abilities = new List<DotaAbility>();
    }
    
    public DotaAbilities(IEnumerable<DotaAbility> abilities)
    {
        this.abilities = new List<DotaAbility>(abilities);
    }
    
    public IEnumerator<DotaAbility> GetEnumerator()
    {
        return this.abilities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}