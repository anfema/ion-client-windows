using System;

namespace Anfema.Amp.Util
{
    class KeyValueStorage
    {
        private static Windows.Storage.ApplicationDataContainer _settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        
        public static void Set<T>( String key, T value )
        {
            // Save value to app storage
            _settings.Values[key] = value;
        }

        public static T Get<T>( String key )
        {
            // Retrieve value from app storage
            if ( _settings.Values.Keys.Contains( key ) )
            {
                return ( T ) _settings.Values[key];
            }

            return default( T );
        }
    }
}
