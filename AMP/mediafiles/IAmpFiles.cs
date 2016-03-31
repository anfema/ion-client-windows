using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Amp.MediaFiles
{
    public interface IAmpFiles
    {
        Task<StorageFile> Request(String url, String checksum, Boolean ignoreCaching = false);
    }
}
