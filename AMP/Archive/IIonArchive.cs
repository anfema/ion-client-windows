using Anfema.Ion.DataModel;
using Anfema.Ion.MediaFiles;
using Anfema.Ion.Pages;
using System;
using System.Threading.Tasks;

namespace Anfema.Ion.Archive
{
    public interface IIonArchive : IIonConfigUpdateable
    {
        Task loadArchive( IIonFiles ionFiles, IIonPages ionPages, string url, Action callback = null );
    }
}
