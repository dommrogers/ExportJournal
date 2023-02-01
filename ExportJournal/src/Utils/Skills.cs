using Il2Cpp;
using MelonLoader;
using MelonLoader.TinyJSON;

namespace ExportJournal
{
    internal class Skills
    {

        private static string skillsFilename = "Skills";


        public static void Generate()
        {
            MelonLogger.Msg("Exporting Skills..");
            string wroteTo = "";

            string fileName = skillsFilename;

            // should we add the timestamp prefix ?
            if (Settings.options.exportTimestampPrefix == true)
            {
                fileName = Main.currentDateTime + fileName;
            }

            // collect the general notes
            List<SkillEntry> skillEntries = BuildSkillEntries();

            if (skillEntries == null || skillEntries.Count == 0)
            {
                MelonLogger.Msg("- nothing to export");
                return;
            }

            // write out text file
            if (Settings.options.exportToTxt == true)
            {
                string skillEntryText = "";
                foreach (SkillEntry skillEntry in skillEntries)
                {
                    skillEntryText += BuildSkillEntriesText(skillEntry);
                }

                string filePath = Path.Combine(Main.saveToFolder, fileName + ".txt"); ;
                Main.WriteToFile(filePath, skillEntryText);
                wroteTo += "TXT|";
            }

            // write out json file
            if (Settings.options.exportToJson == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".json");
                string json = JSON.Dump(skillEntries, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
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


        public static string BuildSkillEntriesText(SkillEntry skillEntry)
        {
            string skillEntryText = "";
            skillEntryText += "SkillName: " + skillEntry.SkillName + "\n";
            skillEntryText += "SkillLevel: " + skillEntry.SkillLevel + "\n";
            skillEntryText += "SkillProgressionPercent: " + skillEntry.SkillProgressionPercent + "\n";
            return skillEntryText + "\n";
        }
        public static List<SkillEntry> BuildSkillEntries()
        {
            List<SkillEntry> list = new();

            int numSkills = GameManager.GetSkillsManager().GetNumSkills();
            for (int i = 0; i < numSkills; i++)
            {
                Skill skillFromIndex = GameManager.GetSkillsManager().GetSkillFromIndex(i);
                if (skillFromIndex)
                {
                    SkillEntry newEntry = new();
                    newEntry.SkillName = skillFromIndex.m_DisplayName;
                    newEntry.SkillLevel = skillFromIndex.GetCurrentTierNumber() + 1;
                    newEntry.SkillProgressionPercent = Math.Round(Double.Parse(skillFromIndex.GetProgressToNextLevelAsNormalizedValue(0).ToString()) * 100,0);

                    list.Add(newEntry);
                }
            }
            return list;
        }

        public class SkillEntry
        {
            public string SkillName = "";
            public int SkillLevel = 0;
            public double SkillProgressionPercent = 0;
        }

    }
}
