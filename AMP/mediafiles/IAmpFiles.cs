using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.MediaFiles
{
    public interface IAmpFiles
    {
        Task<MemoryStream> Request( String url, String checksum, Boolean ignoreCaching = false );
    }
}
