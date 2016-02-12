using Anfema.Amp.DataModel;
using Anfema.Amp.Pages;
using Anfema.Amp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Caching
{
    public class CollectionCacheIndex : CacheIndex
    {
        public DateTime lastUpdated { get; set; }
        public string lastModified { get; set; }


        public CollectionCacheIndex( string filename, DateTime lastUpdated, string lastModified ) : base(filename)
        {
            this.lastUpdated = lastUpdated;
            this.lastModified = lastModified;
        }

        // TODO: implement requestURL constructor


        public bool isOutdated( AmpConfig config )
        {
            return lastUpdated < (DateTime.Now.AddMinutes(-config.minutesUntilCollectionRefresh));
        }


        [JsonIgnore]
        public DateTime? lastModifiedDate
        {
            get
            {
                if(lastModified == null)
                {
                    Debug.WriteLine("Last modified: string is null");
                    return null;
                }

                try
                {
                    DateTime lastModifiedDate = DateTime.Parse(lastModified);
                    Debug.WriteLine("Last modified: Succesfully parsed " + lastModified);
                    return lastModifiedDate;
                }
                catch( Exception e)
                {
                    Debug.WriteLine("Last modified: Parse error for " + lastModified);
                    return null;
                }
            }
        }


        public static async Task<CollectionCacheIndex> retrieve( string requestURL, string collectionIdentifier )
        {
            return await CacheIndexStore.retrieve<CollectionCacheIndex>(requestURL, collectionIdentifier);
        }


        public static void save( AmpConfig config, string lastModified )
        {
            string collectionURL = PagesURLs.getCollectionURL(config);
            CollectionCacheIndex cacheIndex = new CollectionCacheIndex(collectionURL, DateTimeUtils.now(), lastModified);
            CacheIndexStore.save<CollectionCacheIndex>(collectionURL, cacheIndex, config);
        }

    }
}
