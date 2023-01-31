using HarmonyLib;
using Il2Cpp;
using MelonLoader;



namespace ExportJournal
{
    internal class Patches
    {

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.HandlePlayerDeath), new Type[] { typeof(string) })]
        private static class GameManager_HandlePlayerDeath
        {
            private static void Prefix(GameManager __instance, string overrideCauseOfDeath)
            {
                if (Settings.options.exportOnDeath == true)
                {
                    Main.PerformExport(Settings.options.exportDeathStats, overrideCauseOfDeath);
                }
            }
        }

        [HarmonyPatch(typeof(Log), nameof(Log.GetInfoForDay), new Type[] { typeof(int) })]
        private static class Log_GetInfoForDay
        {
            private static void Prefix(int dayNumber)
            {
                MelonLogger.Msg("Log_GetInfoForDay "+ dayNumber);
            }
        }




    }
}
