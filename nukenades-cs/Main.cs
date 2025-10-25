using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using Microsoft.Extensions.Logging;
using nukenades_cs.Grenades;

namespace nukenades_cs;

[MinimumApiVersion(80)]
public class Main : BasePlugin {
    public override string ModuleName => "NukeNades";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Ryocery";
    public override string ModuleDescription => "Makes HE and Flashbang grenades instantly kill.";
    
    private HeGrenade? _heGrenade;
    private Flashbang? _flashbang;

    public override void Load(bool hotReload) {
        base.Load(hotReload);
        
        _heGrenade = new HeGrenade();
        _flashbang = new Flashbang(this);
        
        RegisterEventHandler<EventHegrenadeDetonate>(_heGrenade.OnHEGrenadeDetonate);
        RegisterEventHandler<EventFlashbangDetonate>(_flashbang.OnFlashBangDetonate);
        RegisterEventHandler<EventPlayerDeath>(_flashbang.OnPlayerDeath);
    
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