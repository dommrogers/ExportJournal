using Il2Cpp;
using MelonLoader;
using MelonLoader.TinyJSON;

namespace ExportJournal
{
    internal class GeneralNotes
    {

        private static string generalNotesFilename = "GeneralNotes";


        public static void Generate()
        {
            MelonLogger.Msg("Exporting General Notes..");
            string wroteTo = "";

            string fileName = generalNotesFilename;

            // should we add the timestamp prefix ?
            if (Settings.options.exportTimestampPrefix == true)
            {
                fileName = Main.currentDateTime + fileName;
            }

            // collect the general notes
            string generalNotes = GameManager.GetLogComponent().m_GeneralNotes;

            if (generalNotes == null || generalNotes == "")
            {
                MelonLogger.Msg("- nothing to export");
                return;
            }

            // write out text file
            if (Settings.options.exportToTxt == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".txt"); ;
                Main.WriteToFile(filePath, generalNotes);
                wroteTo += "TXT|";
            }

            // write out json file
            if (Settings.options.exportToJson == true)
            {
                object jsonObject = new { GeneralNotes = generalNotes };
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".json");
                string json = JSON.Dump(jsonObject, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
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

    }
}
