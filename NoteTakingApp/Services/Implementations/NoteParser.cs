using NoteTakingApp.Models;
using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations
{
    public class NoteParser : INoteParser
    {
        public object Parse(string? content)
        {
            var note = new Note();
            var nm_parser = new NoteMetadataParser();
            
            note.Metadata = (NoteMetadata)nm_parser.Parse(content);

            return note;
        }
    }
}
