
namespace ExportJournal;

internal sealed class Main : MelonMod
{
    public static string saveToFolder = "";
    public static string subFolder = "ExportJournal";
    public static string? currentDateTime = null;

    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("ExportJournal initializing..");
        // Enable settings
        Settings.OnLoad();
    }


    //public override void OnUpdate()
    //{
    //    if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.B))
    //    {

    //        if (InterfaceManager.GetInstance().AnyOverlayPanelEnabled())
    //        {
    //            MelonLogger.Msg("Overlay/Panel Open..");
    //            return;
    //        }

    //        // perform the export
    //        PerformExport();

    //    }
    //}


    public static void PerformExport(bool writeDeathStats = false, string? overrideCauseOfDeath = null)
    {
        MelonLogger.Msg("Exporting journal...");
        SaveSlotInfo activeGame = SaveGameSystem.GetNewestSaveSlotForActiveGame();
        if (activeGame == null)
        {
            return;
        }
        string saveGameName = activeGame.m_SaveSlotName;
        if (saveGameName == null || saveGameName == "")
        {
            return;
        }
        if (activeGame.m_UserDefinedName != null)
        {
            saveGameName += "-" + activeGame.m_UserDefinedName;
        }

        // get current system datetime for timestamp
        currentDateTime = DateTime.Now.ToString("yyyyMMdd_HHmm_"); ;

        saveToFolder = Path.Combine(MelonEnvironment.ModsDirectory, subFolder);
        if (!Directory.Exists(saveToFolder))
        {
            Directory.CreateDirectory(saveToFolder);
        }

        saveToFolder = Path.Combine(saveToFolder, saveGameName);
        if (!Directory.Exists(saveToFolder))
        {
            Directory.CreateDirectory(saveToFolder);
        }

        // should we write out the daily logs ?
        if (Settings.options.exportDailyLog == true)
        {
            DailyLogs.Generate();
        }

        // should we write out the skills ?
        if (Settings.options.exportSkills == true)
        {
            Skills.Generate();
        }

        // should we write out the collections ?
        if (Settings.options.exportCollections == true)
        {
            Collections.Generate();
        }

        // should we write out the general notes ?
        if (Settings.options.exportGeneralNotes == true)
        {
            GeneralNotes.Generate();
        }

        // should we export the death stats
        if (writeDeathStats == true)
        {
            DeathStats.Generate(overrideCauseOfDeath);
        }

        HUDMessage.AddMessage("Journal Exported", 2, true, true);

    }

    

    

    

    public static void WriteToFile(string filePath, string contents)
    {
        MelonLogger.Msg("writing to " + filePath);
        File.WriteAllText(filePath, contents, Encoding.UTF8);
    }
}
