using ModSettings;
using System.Reflection;

namespace ExportJournal
{
    internal static class Settings
    {
        public static EJSettings options;

        public static void OnLoad()
        {
            options = new EJSettings();
            options.AddToModSettings("Export Journal");
            options.RefreshFields();
        }


    }

    internal class EJSettings : JsonModSettings
    {
        [Section("What to export")]

        [Name("Daily Log")]
        [Description("Export the daily log notes and stats")]
        public bool exportDailyLog = true;

        [Name("Skills")]
        [Description("Export the skills")]
        public bool exportSkills = true;

        [Name("General Notes")]
        [Description("Export the general notes")]
        public bool exportGeneralNotes = true;

        [Section("Export options")]

        [Name("Auto Export on death")]
        [Description("Do you want to auto export the Journal if/when your character dies")]
        public bool exportOnDeath = true;

        [Name("Export death data")]
        [Description("Export some stats about the death")]
        public bool exportDeathStats = true;

        [Name("Add timestamp to file")]
        [Description("Add a timestamp prefix to the file (e.g. 20230101_0000_)\n\n** NOTE: Setting this to false will overwrite existing journal files for the current save.")]
        public bool exportTimestampPrefix = true;

        [Section("Export formats")]

        [Name("Export as Plain Text")]
        [Description("Export the above selection in TXT format")]
        public bool exportToTxt = true;

        [Name("Export as JSON")]
        [Description("Export the above selection in JSON format")]
        public bool exportToJson = true;

        [Section("YAML format coming soon...")]

        [Name("Export as YAML")]
        [Description("Export the above selection in YAML format")]
        public bool exportToYaml = true;

        internal void RefreshFields()
        {
            SetFieldVisible(nameof(exportDeathStats), Settings.options.exportOnDeath != false);
        }
        protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
        {
            if (
                field.Name == nameof(exportOnDeath)
                )
            {
                RefreshFields();
            }
        }
    }
}
