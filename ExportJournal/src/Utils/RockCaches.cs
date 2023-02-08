
namespace ExportJournal
{
    internal class RockCaches
    {

        private static string rockCachesFilename = "RockCaches";


        public static void Generate()
        {
            MelonLogger.Msg("Exporting Rock Caches..");
            string wroteTo = "";

            string fileName = rockCachesFilename;

            // should we add the timestamp prefix ?
            if (Settings.options.exportTimestampPrefix == true)
            {
                fileName = Main.currentDateTime + fileName;
            }

            // collect the general notes
            List<RockCacheRegion> rockCacheRegionList = BuildRockCacheRegionList();

            if (rockCacheRegionList == null || rockCacheRegionList.Count == 0)
            {
                MelonLogger.Msg("- nothing to export");
                return;
            }

            // write out text file
            if (Settings.options.exportToTxt == true)
            {
                string rockCacheText = "";
                foreach (RockCacheRegion rockCacheRegion in rockCacheRegionList)
                {
                    rockCacheText += BuildRockCacheText(rockCacheRegion);
                }

                string filePath = Path.Combine(Main.saveToFolder, fileName + ".txt"); ;
                Main.WriteToFile(filePath, rockCacheText);
                wroteTo += "TXT|";
            }

            // write out json file
            if (Settings.options.exportToJson == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".json");
                string json = JSON.Dump(rockCacheRegionList, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
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

        public static string BuildRockCacheText(RockCacheRegion rockCacheRegion)
        {
            string rockCacheText = "";

            if (rockCacheRegion.RockCaches != null && rockCacheRegion.RockCaches.Count > 0)
            {
                rockCacheText += "RegionName: " + rockCacheRegion.RegionName + "\n";
                rockCacheText += "RockCacheCount: " + rockCacheRegion.RockCacheCount + "\n";
                foreach (string rockCacheName in rockCacheRegion.RockCaches)
                {
                    rockCacheText += " - " + rockCacheName + "\n";
                }
            }

            return rockCacheText;
        }


        public static RockCacheRegion BuildRockCacheRegion()
        {
            RockCacheRegion rockCacheRegion = new();

            return rockCacheRegion;
        }
        public static List<RockCacheRegion> BuildRockCacheRegionList()
        {
            List<RockCacheRegion> rockCacheRegionList = new();

            Panel_Log panel_log = InterfaceManager.GetPanel<Panel_Log>();
            panel_log.GenerateRockCacheScreenData();

//            MelonLogger.Warning("panel_log.m_RockCacheListByRegion.Count = " + panel_log.m_RockCacheListByRegion.Count);

            foreach (Il2CppSystem.Collections.Generic.KeyValuePair<string, Il2CppSystem.Collections.Generic.List<RockCacheInfo>> item in panel_log.m_RockCacheListByRegion)
            {
 //               MelonLogger.Warning("panel_log.m_RockCacheListByRegion[x].name = " + item.key);
                if (item.value != null && item.value.Count > 0)
                {
//                    MelonLogger.Warning("panel_log.m_RockCacheListByRegion[x].Count = " + item.value.Count);
                    RockCacheRegion rockCacheRegion = new();
                    rockCacheRegion.RegionName = InterfaceManager.GetNameForScene(item.key);
//                    MelonLogger.Warning("rockCacheRegion.RegionName = " + rockCacheRegion.RegionName);

                    foreach (RockCacheInfo rockCacheInfo in item.value)
                    {
//                        MelonLogger.Warning("rockCacheInfo.m_CustomName = " + rockCacheInfo.m_CustomName);
                        rockCacheRegion.RockCaches.Add(rockCacheInfo.m_CustomName);
                        rockCacheRegion.RockCacheCount++;
                    }
                    rockCacheRegionList.Add(rockCacheRegion);
                }
            }

            return rockCacheRegionList;
        }

        public class RockCacheRegion
        {
            public string RegionName = "";
            public int RockCacheCount = 0;
            public List<string> RockCaches = new();
        }

    }
}
