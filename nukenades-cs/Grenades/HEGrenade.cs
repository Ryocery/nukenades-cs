using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace nukenades_cs.Grenades;

public class HeGrenade {
    private const float NukeRadius = 8000.0f;
    private const int Damage = 99999;

    public HookResult OnHEGrenadeDetonate(EventHegrenadeDetonate @event, GameEventInfo info) {
        CHEGrenadeProjectile? grenadeEntity = Utilities.GetEntityFromIndex<CHEGrenadeProjectile>(@event.Entityid);
        if (grenadeEntity == null || !grenadeEntity.IsValid) return HookResult.Continue;
        
        grenadeEntity.Damage = Damage;
        grenadeEntity.DmgRadius = NukeRadius;
        
        return HookResult.Continue;
    }
}