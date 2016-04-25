using Newtonsoft.Json;


namespace Anfema.Ion.DataModel
{
    public class IonTextContent : IonContent
    {
        [JsonProperty( "mime_type" )]
        public string mimeType { get; set; }

        [JsonProperty( "is_multiline" )]
        public bool multiLine { get; set; }

        public string text { get; set; }


        [JsonIgnore]
        public bool isMultiLine
        {
            get { return multiLine; }
        }


        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals( object obj )
        {
            // Basic IonContent equality check
            if( !base.Equals( obj ) )
            {
                return false;
            }

            try
            {
                // Try to cast
                IonTextContent content = (IonTextContent)obj;

                return mimeType.Equals( content.mimeType )
                    && multiLine == content.multiLine
                    && text.Equals( content.text );
            }

            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Returns the exact hashCode that the base class would do
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
