
namespace ExportJournal
{
    internal class Stats
    {

        public static string statsFilename = "Stats";


        public static void Generate()
        {
            MelonLogger.Msg("Exporting Stats..");
            string wroteTo = "";

            string fileName = statsFilename;

            // should we add the timestamp prefix ?
            if (Settings.options.exportTimestampPrefix == true)
            {
                fileName = Main.currentDateTime + fileName;
            }

            // collect the stats
            Dictionary<string, string> stats = BuildStats();

            // write out text file
            if (Settings.options.exportToTxt == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".txt");
                string statsTxt = "";
                foreach (KeyValuePair<string, string> item in stats)
                {
                    statsTxt += item.Key + ": " + item.Value + "\n";
                }
                Main.WriteToFile(filePath, statsTxt);
                wroteTo += "TXT|";
            }

            // write out json file
            if (Settings.options.exportToJson == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".json");
                string json = JSON.Dump(stats, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
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

            //MelonLogger.Msg("- done |" + wroteTo + "");
        }

        private static Dictionary<string, string> BuildStats()
        {

            Panel_Log panel_log = InterfaceManager.GetPanel<Panel_Log>();
            Dictionary<string, string> list = new();

//            MelonLogger.Warning("Stats.BuildStats");

            if (panel_log.m_DetailedStatsView == null)
            {
                return list;
            }
            DetailedStatsView detailStatsView = panel_log.m_DetailedStatsView;

            if (detailStatsView.m_StatsOrderToShow == null)
            {
                return list;
            }
            StatID[] statsOrderToShow = detailStatsView.m_StatsOrderToShow;

            foreach (StatID statId in statsOrderToShow)
            {
                StatInfo statInfo = StatsManager.GetStatInfo(statId);
                if (!statInfo.m_Hidden)
                {
                    string label = detailStatsView.GetFormattedTitle(statInfo.m_LocId);
                    float value = StatsManager.m_CurrentSessionStatsContainer.GetValue(statId);
                    string newvalue = detailStatsView.GetFormattedValue(statInfo.m_Format, value);


                    label = label.Replace(" ", null).Replace("(","_").Replace(")",null);
                    list.Add(label, newvalue);
                }
            }
            return list;
        }
    }
}
