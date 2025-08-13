namespace NoteTakingApp.Configurations
{
    public class NotesConfiguration
    {
        public string NotesDirectory { get; set; } = "NoteFiles";
        public bool CreateDirectoryIfNotExists { get; set; } = true;

    }
}
