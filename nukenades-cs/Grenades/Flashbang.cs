using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace nukenades_cs.Grenades;

public class Flashbang(Main plugin) {
    private readonly Dictionary<CCSPlayerController, CCSPlayerController> _pendingFlashKills = new();

    public HookResult OnFlashBangDetonate(EventFlashbangDetonate @event, GameEventInfo info) {
        CCSPlayerController? thrower = @event.Userid;
        if (thrower == null || !thrower.IsValid) return HookResult.Continue;
        
        plugin.AddTimer(0.1f, () => {
            List<CCSPlayerController> players = Utilities.GetPlayers();

            foreach (CCSPlayerController player in players) {
                if (!player.IsValid || player.PlayerPawn.Value == null) continue;

                CCSPlayerPawn? playerPawn = player.PlayerPawn.Value;

                if (playerPawn.FlashDuration > 0 && playerPawn.Health > 0) {
                    plugin.Logger.LogInformation("Player {PlayerPlayerName} was flashed by {ThrowerPlayerName} and will die", player.PlayerName, thrower.PlayerName);
                    
                    if (player != thrower) {
                        _pendingFlashKills[player] = thrower;
                    }
                    
                    playerPawn.CommitSuicide(false, true);
                }
            }
        });

        return HookResult.Continue;
    }

    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info) {
        CCSPlayerController? victim = @event.Userid;
        
        if (victim != null && _pendingFlashKills.ContainsKey(victim)) {
            CCSPlayerController thrower = _pendingFlashKills[victim];
            
            @event.Attacker = thrower;
            @event.Weapon = "flashbang";
            @event.Headshot = false;
            
            if (thrower.IsValid && thrower.ActionTrackingServices != null) {
                thrower.ActionTrackingServices.MatchStats.Kills++;
            }
            
            plugin.Logger.LogInformation("Fixed death event: {VictimPlayerName} killed by {ThrowerPlayerName}'s flashbang", victim.PlayerName, thrower.PlayerName);
            _pendingFlashKills.Remove(victim);
        }

        return HookResult.Continue;
    }
}