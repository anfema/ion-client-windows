using Anfema.Amp.DataModel;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Amp.MediaFiles
{
    public interface IAmpFiles : IAmpConfigUpdateable
    {
        Task<StorageFile> Request(String url, String checksum, Boolean ignoreCaching = false);
    }
}
