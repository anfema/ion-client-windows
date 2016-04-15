using System;


namespace Anfema.Ion.Exceptions
{
    public class NoIonPagesRequestException : Exception
    {
        public NoIonPagesRequestException( String message ) : base( message )
        { }

        public NoIonPagesRequestException() : base()
        { }
    }
}
