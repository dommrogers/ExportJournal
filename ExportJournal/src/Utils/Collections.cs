
namespace ExportJournal
{
    internal class Collections
    {

        private static string collectionsFilename = "Collections";


        public static void Generate()
        {
            MelonLogger.Msg("Exporting Collections..");
            string wroteTo = "";

            string fileName = collectionsFilename;

            // should we add the timestamp prefix ?
            if (Settings.options.exportTimestampPrefix == true)
            {
                fileName = Main.currentDateTime + fileName;
            }

            // collect the general notes
            CollectionsList collectionsList = BuildCollectionsList();

            if (collectionsList == null)
            {
                MelonLogger.Msg("- nothing to export");
                return;
            }

            // write out text file
            if (Settings.options.exportToTxt == true)
            {
                string collectionsListText = "";
                collectionsListText += "NotesCount: " + collectionsList.NotesCount + "\n";
                collectionsListText += "PolaroidsCount: " + collectionsList.PolaroidsCount + "\n";
                collectionsListText += "MementosCount: " + collectionsList.MementosCount + "\n";
                collectionsListText += "CairnsCount: " + collectionsList.CairnsCount + "\n";
                collectionsListText += "BufferMemoriesCount: " + collectionsList.BufferMemoriesCount + "\n";
                collectionsListText += "SurveyedLocationsCount: " + collectionsList.SurveyedLocationsCount + "\n";
                foreach (string location in collectionsList.SurveyedLocations)
                {
                    collectionsListText += " - " + location + "\n";
                }


                string filePath = Path.Combine(Main.saveToFolder, fileName + ".txt"); ;
                Main.WriteToFile(filePath, collectionsListText);
                wroteTo += "TXT|";
            }

            // write out json file
            if (Settings.options.exportToJson == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".json");
                string json = JSON.Dump(collectionsList, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
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


        public static CollectionsList BuildCollectionsList()
        {
            CollectionsList collectionList = new();
            Panel_Log panel_log = InterfaceManager.GetPanel<Panel_Log>();
            panel_log.RefreshCollectibleCounters();
            panel_log.BuildCartographyRegionList();

            collectionList.NotesCount = panel_log.m_CurrNumNoteCollectibles;
            collectionList.PolaroidsCount = panel_log.m_CurrNumPostcardCollectibles;
            collectionList.MementosCount = panel_log.m_CurrNumMementoCollectibles;
            collectionList.CairnsCount = panel_log.m_CurrNumCairnCollectibles;
            collectionList.BufferMemoriesCount = panel_log.m_CurrNumAuroraScreenCollectibles;
            collectionList.SurveyedLocationsCount = panel_log.m_SurveyedLocations;

            List<string> locations = new();
            if (panel_log.m_SurveyRegionList != null && panel_log.m_SurveyRegionList.Count > 0)
            {
                foreach (SurveyRegionInfo regionInfo in panel_log.m_SurveyRegionList)
                {
                    if (regionInfo.m_IsUnlocked && regionInfo.m_Achievements != null && regionInfo.m_Achievements.Count > 0) {
                        foreach (SurveyAchievementInfo achievementInfo in regionInfo.m_Achievements)
                        {
                            if (achievementInfo.m_Completed) {
                                locations.Add(regionInfo.m_RegionName + " - " + achievementInfo.m_AchievementName);
                            }
                        }
                        
                    }
                }
            }
            collectionList.SurveyedLocations = locations;

            return collectionList;
        }

        public class CollectionsList
        {
            public int NotesCount = 0;
            public int PolaroidsCount = 0;
            public int MementosCount = 0;
            public int CairnsCount = 0;
            public int BufferMemoriesCount = 0;
            public int SurveyedLocationsCount = 0;
            public List<string> SurveyedLocations = new();
        }

    }
}
