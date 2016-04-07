using System;

namespace Anfema.Ion.Exceptions
{
    public class PagePreviewNotFoundException : NullReferenceException
    {
        public PagePreviewNotFoundException(string identifier) : base("PagePreview with identifier " + identifier + " not found") { }
    }
}
