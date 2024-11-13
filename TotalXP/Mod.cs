using GDWeave;

namespace TotalXP;

public class Mod : IMod {
    public Config Config;

    public Mod(IModInterface modInterface) {
        this.Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new GrabTotalXP());
        modInterface.Logger.Information("TotalXP mod has loaded");
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
