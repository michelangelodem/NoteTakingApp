using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Interfaces
{
    public interface INoteParser
    {
        public NoteMetadata Parse(string content);
    }
}
