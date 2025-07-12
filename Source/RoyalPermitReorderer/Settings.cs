using UnityEngine;
using Verse;

namespace RoyalPermitReorderer;

public class Settings : ModSettings
{
    //Use Mod.settings.setting to refer to this setting.
    public bool horizontal = false;

    public int permitsPerRow = 3;

    private string buffer;

    public void DoWindowContents(Rect wrect)
    {
        var options = new Listing_Standard();
        options.Begin(wrect);

        options.Label("RoyalPermitReorderer_Settings_RowLength".Translate());
        options.TextFieldNumeric<int>(ref permitsPerRow, ref buffer);

        options.Gap();
        
        options.CheckboxLabeled("RoyalPermitReorderer_Settings_Horizontal".Translate(), ref horizontal);
        options.Gap();

        if(options.ButtonText("RoyalPermitReorderer_Settings_Reorder".Translate()))
        {
            Current.Game?.GetComponent<RPR_GameComponent_PermitReorderer>().PrepareReordering();
        }

        options.End();
    }
    
    public override void ExposeData()
    {
        Scribe_Values.Look(ref horizontal, "horizontal", false);
        Scribe_Values.Look(ref permitsPerRow, "permitsPerRow", 3);
    }
}
