using Anfema.Amp.DataModel;
using Anfema.Amp.Pages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Anfema.Amp
{
    public class Amp
    {
        // Dictionary for all the possible instances bound to a specific config
        private static Dictionary<AmpConfig, Amp> instances = new Dictionary<AmpConfig, Amp>();
        
        /// Organizes all the data handling for pages and collections
        AmpPagesWithCaching _pages;


        /// <summary>
        /// Is used to get a instance of Amp corresponding to the given configuration
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Amp getInstance( AmpConfig config )
        {
            Amp storedClient;

            if( instances.TryGetValue(config, out storedClient ) )
            {
                return storedClient;
            }

            Amp amp = new Amp( config );
            instances.Add(config, amp);
            return amp;
        }


        /// <summary>
        /// Constructor with a config file for initialization
        /// </summary>
        /// <param name="config"></param>
        public Amp( AmpConfig config )
        {
            _pages = new AmpPagesWithCaching(config);
        }


        /// <summary>
        /// Returns the whole data of a desired page already parsed as observable collections
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        /// <returns>Observable collection with the content of the page</returns>
        public async Task<AmpPageObservableCollection> getPageContentAsync(string name, Action callback)
        {
            AmpPage page = await _pages.getPageAsync(name);
            AmpPageObservableCollection content = page.contents[0].children[0]; // TODO: check for trouble this hardcoded indices could result in

            if (callback != null)
            {
                callback();
            }

            return content;
        }


        /// <summary>
        /// Returns a list of all page identifiers
        /// </summary>
        /// <returns>List of identifier</returns>
        public async Task<List<string>> getAllPageIdentifierAsync()
        {
            return await _pages.getAllPagesIdentifierAsync();
        }
    }
}
