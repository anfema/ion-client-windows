using Anfema.Ion.DataModel;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Ion.MediaFiles
{
    public interface IIonFiles : IIonConfigUpdateable
    {
        Task<StorageFile> requestAsync( String url, String checksum, IonContent content, Boolean ignoreCaching );
        Task<StorageFile> requestArchiveFileAsync( string url );
    }
}
