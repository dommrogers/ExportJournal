
namespace ExportJournal
{
    internal class DailyLogs
    {

        private static string dailyLogsFilename = "DailyLogs";


        public static void Generate()
        {
            MelonLogger.Msg("Exporting Daily Logs..");
            string wroteTo = "";
            string fileName = dailyLogsFilename;

            // should we add the timestamp prefix ?
            if (Settings.options.exportTimestampPrefix == true)
            {
                fileName = Main.currentDateTime + fileName;
            }

            // collect the daily logs
            List<DailyEntry> dailyLogEntries = BuildDailyLogs();
            MelonLogger.Msg("dailyLogEntries = " + dailyLogEntries.Count);

            if (dailyLogEntries.Count == 0)
            {
                MelonLogger.Msg("- nothing to export");
                return;
            }


            // write out text file
            if (Settings.options.exportToTxt == true)
            {
                string dailyLogsTxt = "";
                foreach (DailyEntry dailyEntry in dailyLogEntries)
                {
                    dailyLogsTxt += BuildDailyEntryText(dailyEntry);
                }

                string filePath = Path.Combine(Main.saveToFolder, fileName + ".txt"); ;
                Main.WriteToFile(filePath, dailyLogsTxt);
                wroteTo += "TXT|";
            }

            // write out json file
            if (Settings.options.exportToJson == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".json");
                string json = JSON.Dump(dailyLogEntries, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
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

        private static List<DailyEntry> BuildDailyLogs()
        {
            List<DailyEntry> list = new();

            int currentDay = GameManager.GetTimeOfDayComponent().GetDayNumber();
            MelonLogger.Msg("current day - " + currentDay);

            // get the game entries
            Log logComponent = GameManager.GetLogComponent();
            for (int day = 1; day <= currentDay; day++)
            {
                if (currentDay == day)
                {
                    logComponent.LockInTodaysValues();
                }
                LogDayInfo dailyLog = logComponent.GetInfoForDay(day);
                list.Add(BuildDailyEntry(dailyLog));
            }

            return list;
        }

        public static DailyEntry BuildDailyEntry(LogDayInfo dailyLog)
        {
            DailyEntry newEntry = new();

            newEntry.Day = dailyLog.m_DayNumber;
            if (dailyLog.m_LocationLocIDs != null)
            {
                newEntry.LocationsDiscoveredCount = dailyLog.m_LocationLocIDs.Count;
                for (int i = 0; i < dailyLog.m_LocationLocIDs.Count; i++)
                {
                    string regionLocationText = Localization.Get(dailyLog.m_RegionLocIDs[i]) + " - " + Localization.Get(dailyLog.m_LocationLocIDs[i]);
                    newEntry.LocationsDiscovered.Add(regionLocationText);
                }
            }
            newEntry.WorldExploredPercent = dailyLog.m_WorldExplored;
            newEntry.HoursRested = dailyLog.m_HoursRested;
            newEntry.ConditionRangePercent.Add("Min", dailyLog.m_ConditionLow);
            newEntry.ConditionRangePercent.Add("Max", dailyLog.m_ConditionHigh);
            newEntry.CaloriesBurned = dailyLog.m_CaloriesBurned;
            if (dailyLog.m_Afflictions != null)
            {
                newEntry.InjuriesSustainedCount = dailyLog.m_Afflictions.Count;
                foreach (string InjurieSustained in dailyLog.m_Afflictions)
                {
                    newEntry.InjuriesSustained.Add(Localization.Get(InjurieSustained));
                }
            }
            newEntry.Notes = dailyLog.m_Notes;

            return newEntry;
        }

        public static string BuildDailyEntryText(DailyEntry dailyEntry)
        {
            string dailyEntryText = "";

            dailyEntryText += "Day: " + dailyEntry.Day + "\n";
            dailyEntryText += "LocationsDiscoveredCount: " + dailyEntry.LocationsDiscoveredCount + "\n";
            dailyEntryText += "LocationsDiscovered: " + "\n";
            foreach (string LocationDiscovered in dailyEntry.LocationsDiscovered)
            {
                dailyEntryText += " - " + LocationDiscovered + "\n";
            }
            dailyEntryText += "WorldExploredPercent: " + dailyEntry.WorldExploredPercent + "\n";
            dailyEntryText += "HoursRested: " + dailyEntry.HoursRested + "\n";
            dailyEntryText += "ConditionRangePercent: " + "\n";
            dailyEntryText += " - Min: " + dailyEntry.ConditionRangePercent["Min"] + "\n";
            dailyEntryText += " - Max: " + dailyEntry.ConditionRangePercent["Max"] + "\n";
            dailyEntryText += "CaloriesBurned: " + dailyEntry.CaloriesBurned + "\n";
            dailyEntryText += "InjuriesSustained: " + dailyEntry.InjuriesSustainedCount + "\n";
            foreach (string InjurieSustained in dailyEntry.InjuriesSustained)
            {
                dailyEntryText += " - " + InjurieSustained + "\n";
            }
            dailyEntryText += "Notes: " + dailyEntry.Notes + "\n";
            return dailyEntryText + "\n";
        }

        public class DailyEntry
        {
            public int Day = 0;
            public int LocationsDiscoveredCount = 0;
            public List<string> LocationsDiscovered = new();
            public int WorldExploredPercent = 0;
            public int HoursRested = 0;
            public Dictionary<string, int> ConditionRangePercent = new();
            public int CaloriesBurned = 0;
            public int InjuriesSustainedCount = 0;
            public List<string> InjuriesSustained = new();
            public string Notes = "";
        }

    }
}
