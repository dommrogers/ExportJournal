
namespace ExportJournal
{
    internal class DeathStats
    {

        public static string deathStatsFilename = "DeathStats";


        public static void Generate(string? overrideCauseOfDeath = null)
        {
            MelonLogger.Msg("Exporting Death Stats..");
            string wroteTo = "";

            string fileName = deathStatsFilename;

            // should we add the timestamp prefix ?
            if (Settings.options.exportTimestampPrefix == true)
            {
                fileName = Main.currentDateTime + fileName;
            }

            // collect the death stats
            Dictionary<string, string> deathStats = BuildDeathStats(overrideCauseOfDeath);

            // write out text file
            if (Settings.options.exportToTxt == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".txt");
                string deathStatsTxt = "";
                foreach (KeyValuePair<string, string> item in deathStats)
                {
                    deathStatsTxt += item.Key + ": " + item.Value + "\n";
                }
                Main.WriteToFile(filePath, deathStatsTxt);
                wroteTo += "TXT|";
            }

            // write out json file
            if (Settings.options.exportToJson == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".json");
                string json = JSON.Dump(deathStats, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
                Main.WriteToFile(filePath, json);
                wroteTo += "JSON|";
            }

            // write out yaml file
            //if (Settings.options.exportToYaml == true)
            //{
            //    string filePath = Path.Combine(saveToFolder, fileName + ".yaml");
            //    WriteToFile(filePath, "TODO - :)");
            //    wroteTo += "YAML|";
            //}

            MelonLogger.Msg("- done |" + wroteTo + "");
        }

        private static Dictionary<string, string> BuildDeathStats(string? overrideCauseOfDeath = null)
        {

            SaveSlotInfo activeGame = SaveGameSystem.GetNewestSaveSlotForActiveGame();

            if (overrideCauseOfDeath == null)
            {
                overrideCauseOfDeath = GameManager.m_OverridenCauseOfDeath;
            }

            Dictionary<string, string> list = new();
            list.Add("SaveSlotName", activeGame.m_SaveSlotName);
            list.Add("UserDefinedName", activeGame.m_UserDefinedName);
            list.Add("GameMode", activeGame.m_XPMode.ToString());
            list.Add("Region", activeGame.m_Region);
            list.Add("Location", activeGame.m_Location);
            list.Add("LocationOverride", activeGame.m_LocationOverride);
            list.Add("CauseOfDeath", GameManager.m_Condition.GetCauseOfDeathString());
            list.Add("OverridenCauseOfDeath", overrideCauseOfDeath);
            list.Add("IsSleeping", GameManager.m_Rest.IsSleeping().ToString());
            list.Add("TimeSurvived", Utils.GetTimeSurvivedString());
            return list;
        }
    }
}
