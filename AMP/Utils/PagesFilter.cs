using Anfema.Amp.DataModel;
using System;


namespace Anfema.Amp.Utils
{
    public class PageFilter
    {
        // All pages
        public static Predicate<PagePreview> all = x => x != null;


        // All pages with no parent -> root pages
        public static Predicate<PagePreview> rootElements = x => x.parent == null;


        // Pages with specific identifier
        public static Predicate<PagePreview> identifierEquals( string identifier )
        {
            return x => x.identifier != null && x.identifier.Equals(identifier);
        }


        // Pages with identifier containing a substring
        public static Predicate<PagePreview> identifierContains( string identifier )
        {
            return x => x.identifier != null && x.identifier.Contains(identifier);
        }


        // Pages with a specific layout
        public static Predicate<PagePreview> layoutEquals( string layout )
        {
            return x => x.layout != null && x.layout.Equals(layout);
        }


        // Pages, which are child of a specific page
        public static Predicate<PagePreview> childOf( string parentIdentifier )
        {
            return x => x.parent != null && x.parent.Equals(parentIdentifier);
        }
    }
}
