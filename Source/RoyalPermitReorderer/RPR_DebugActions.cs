using LudeonTK;
using Verse;

namespace RoyalPermitReorderer
{
    public static class RPR_DebugActions
    {
        [DebugAction("Royal Permit Reorderer", "ReorderPermits", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void ReorderPermits()
        {
            RPR_GameComponent_PermitReorderer component = Current.Game.GetComponent<RPR_GameComponent_PermitReorderer>();

            component.PrepareReordering();
        }
    }
}