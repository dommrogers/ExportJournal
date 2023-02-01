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

        [HarmonyPatch(typeof(SkillsManager), nameof(SkillsManager.GetSkillFromIndex), new Type[] { typeof(int) })]
        private static class SkillManager_GetSkillFromIndex
        {
            private static void Prefix(int index)
            {
                MelonLogger.Msg("SkillManager_GetSkillFromIndex " + index);
            }
        }




    }
}
