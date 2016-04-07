using Anfema.Ion.DataModel;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Ion.MediaFiles
{
    public interface IIonFiles : IIonConfigUpdateable
    {
        Task<StorageFile> Request( String url, String checksum, Boolean ignoreCaching = false );
    }
}
