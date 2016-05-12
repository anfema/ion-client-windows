namespace Anfema.Ion.Utils
{
    public class IonLogMessage
    {
        private string _message;
        private IonLogMessageTypes _type;


        /// <summary>
        /// Constructor with initialization
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="messageType">Type of this message</param>
        public IonLogMessage( string message, IonLogMessageTypes messageType )
        {
            _message = message;
            _type = messageType;
        }


        /// <summary>
        /// Accessor for the message
        /// </summary>
        public string message
        {
            get
            {
                return _message;
            }
        }


        /// <summary>
        /// Accessor for the type of the log message
        /// </summary>
        public IonLogMessageTypes type
        {
            get
            {
                return _type;
            }
        }
    }
}