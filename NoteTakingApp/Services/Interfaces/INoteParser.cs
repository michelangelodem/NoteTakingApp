using NoteTakingApp.Models;

namespace NoteTakingApp.Services.Interfaces
{
    public interface INoteParser
    {
        public object Parse(string content);
    }
}
