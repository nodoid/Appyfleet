using System.IO;
using System.Threading.Tasks;

namespace mvvmframework.Interfaces
{
    public interface IFileSystem
    {
        bool FileExists(string filename);
        Task DownloadFile(string url, string filename);
    }
}
