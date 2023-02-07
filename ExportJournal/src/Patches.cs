
using Il2Cpp;
using UnityEngine;

namespace ExportJournal
{
    internal class Patches
    {
        internal static System.Action GetActionDelegate() => new System.Action(Execute);
        private static GameObject? exportButton = null;

        internal static void Execute()
        {
            Main.PerformExport();
            HUDMessage.AddMessage("Journal Exported", 2, true, true);
        }

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

        [HarmonyPatch(typeof(Panel_Log), nameof(Panel_Log.OnBack))]
        private static class Panel_Log_OnBack
        {
            private static void Postfix()
            {
                if (exportButton != null)
                {
                    MelonLogger.Msg("Panel_Log_OnBack");
                    Utils.Destroy(exportButton);
                    exportButton = null;
                }
            }
        }

        [HarmonyPatch(typeof(Panel_Log), nameof(Panel_Log.EnterState), new Type[] { typeof(PanelLogState) })]
        private static class Panel_Log_EnterState
        {
            private static void Postfix()
            {
                SaveSlotInfo activeGame = SaveGameSystem.GetNewestSaveSlotForActiveGame();
                if (activeGame != null)
                {
                    //                    MelonLogger.Msg("Panel_Log_EnterState");
                    if (exportButton == null)
                    {
                        //                        MelonLogger.Msg("Panel_Log_EnterState add exportButton");
                        Panel_Log panel_log = InterfaceManager.GetPanel<Panel_Log>();

                        exportButton = GameObject.Instantiate<GameObject>(panel_log.m_Button_Back.gameObject, panel_log.m_Button_Back.gameObject.transform.parent, true);
                        exportButton.name = "Button_Export_Journal";
                        exportButton.transform.Translate(-2.73f, 0, 0);
                        Utils.GetComponentInChildren<UILabel>(exportButton).text = "Export";
                        if (Utils.GetComponentInChildren<UILocalize>(exportButton))
                        {
                            Utils.GetComponentInChildren<UILocalize>(exportButton).key = null;
                        }
                        Il2CppSystem.Collections.Generic.List<EventDelegate> placeHolderList = new Il2CppSystem.Collections.Generic.List<EventDelegate>();
                        placeHolderList.Add(new EventDelegate(new System.Action(Execute)));
                        Utils.GetComponentInChildren<UIButton>(exportButton).onClick = placeHolderList;

                        NGUITools.SetActive(exportButton, true);
                    }
                }

            }
        }




    }
}
