using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Pages
{
    public interface IAmpPages
    {
        Task<AmpCollection> getCollectionAsync();
        Task<AmpPage> getPageAsync(string pageIdentifier);
    }
}
