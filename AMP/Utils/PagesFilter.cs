using Anfema.Ion.DataModel;
using System;


namespace Anfema.Ion.Utils
{
    public class PageFilter
    {
        // All pages
        public static Predicate<IonPagePreview> all = x => x != null;


        // All pages with no parent -> root pages
        public static Predicate<IonPagePreview> rootElements = x => x.parent == null;


        // Pages with specific identifier
        public static Predicate<IonPagePreview> identifierEquals( string identifier )
        {
            return x => x.identifier != null && x.identifier.Equals(identifier);
        }


        // Pages with identifier containing a substring
        public static Predicate<IonPagePreview> identifierContains( string identifier )
        {
            return x => x.identifier != null && x.identifier.Contains(identifier);
        }


        // Pages with a specific layout
        public static Predicate<IonPagePreview> layoutEquals( string layout )
        {
            return x => x.layout != null && x.layout.Equals(layout);
        }


        // Pages, which are child of a specific page
        public static Predicate<IonPagePreview> childOf( string parentIdentifier )
        {
            return x => x.parent != null && x.parent.Equals(parentIdentifier);
        }
    }
}
