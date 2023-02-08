
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

            NoteEntry notes = new();
            notes.GeneralNotes = generalNotes;

            // write out text file
            if (Settings.options.exportToTxt == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".txt"); ;
                Main.WriteToFile(filePath, notes.GeneralNotes);
                wroteTo += "TXT|";
            }

            // write out json file
            if (Settings.options.exportToJson == true)
            {
                string filePath = Path.Combine(Main.saveToFolder, fileName + ".json");
                string json = JSON.Dump(notes, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
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

        public class NoteEntry
        {
            public string GeneralNotes = "";
        }

    }
}
