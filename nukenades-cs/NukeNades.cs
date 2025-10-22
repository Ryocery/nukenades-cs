using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using Microsoft.Extensions.Logging;

namespace nukenades_cs;

[MinimumApiVersion(80)]
public class NukeNades : BasePlugin {
    public override string ModuleName => "NukeNades";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Ryocery";
    public override string ModuleDescription => "Makes HE and Flashbang grenades instantly kill.";
    
    private const float NukeRadius = 8000.0f;
    private const int Damage = 99999;

    [GameEventHandler]
    public HookResult OnHEGrenadeDetonate(EventHegrenadeDetonate @event, GameEventInfo info) {
        CHEGrenadeProjectile? grenadeEntity = Utilities.GetEntityFromIndex<CHEGrenadeProjectile>(@event.Entityid);
        if (grenadeEntity == null || !grenadeEntity.IsValid) return HookResult.Continue;
        grenadeEntity.Damage = Damage;
        grenadeEntity.DmgRadius = NukeRadius;
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnFlashBangDetonate(EventFlashbangDetonate @event, GameEventInfo info) {
        CCSPlayerController? thrower = @event.Userid;
        if (thrower == null) return HookResult.Continue;
        
        AddTimer(0.1f, () => {
            List<CCSPlayerController> players = Utilities.GetPlayers();

            foreach (CCSPlayerController player in players) {
                if (!player.IsValid || player.PlayerPawn.Value == null) continue;

                CCSPlayerPawn? playerPawn = player.PlayerPawn.Value;

                if (playerPawn.FlashDuration > 0) {
                    Logger.LogInformation($"Player {player.PlayerName} was flashed and will die");
                    playerPawn.CommitSuicide(false, true);
                }
            }
        });

        return HookResult.Continue;
    }
    
    public override void Load(bool hotReload) {
        base.Load(hotReload);
    
        if (hotReload) {
            Logger.LogInformation("Plugin hot reloaded successfully!");
        }
    }
    
    public override void Unload(bool hotReload) {
        base.Unload(hotReload);
    
        if (hotReload) {
            Logger.LogInformation("Plugin unloaded for hot reload.");
        }
    }
}