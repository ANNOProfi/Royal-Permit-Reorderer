using Verse;
using UnityEngine;
using HarmonyLib;

namespace RoyalPermitReorderer;

public class RoyalPermitReordererMod : Mod
{
    public static Settings settings;

    public RoyalPermitReordererMod(ModContentPack content) : base(content)
    {
        Log.Message("RoyalPermitReorderer loaded successfully");

        // initialize settings
        settings = GetSettings<Settings>();
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("ANNOProfi.rimworld.RoyalPermitReorderer.main");	
        harmony.PatchAll();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "RoyalPermitReorderer_SettingsCategory".Translate();
    }
}
