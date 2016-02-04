using Anfema.Amp.DataModel;
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

        private DateTime _lastUpdated;

        private string _lastModified;

        public CollectionCacheIndex( string filename, DateTime lastUpdated, string lastModified ) : base(filename)
        {
            _lastUpdated = lastUpdated;
            _lastModified = lastModified;
        }

        // TODO: implement requestURL constructor


        public DateTime lastUpdated
        {
            get
            {
                return _lastUpdated;
            }

            set
            {
                _lastUpdated = value;
            }
        }

        
        public bool isOutdated( AmpConfig config )
        {
            return _lastUpdated < (DateTime.Now.AddMinutes(-config.minutesUntilCollectionRefresh));
        }


        public DateTime? lastModifiedDate
        {
            get
            {
                if(_lastModified == null)
                {
                    Debug.WriteLine("Last modified: string is null");
                    return null;
                }

                try
                {
                    DateTime lastModifiedDate = DateTime.Parse(_lastModified);
                    Debug.WriteLine("Last modified: Succesfully parsed " + _lastModified);
                    return lastModifiedDate;
                }
                catch( Exception e)
                {
                    Debug.WriteLine("Last modified: Parse error for " + _lastModified);
                    return null;
                }
            }
        }


        public string lastModified
        {
            get
            {
                return _lastModified;
            }
            set
            {
                _lastModified = value;
            }
        }


    }
}
