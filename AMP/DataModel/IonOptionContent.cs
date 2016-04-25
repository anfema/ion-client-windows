using Newtonsoft.Json;

namespace Anfema.Ion.DataModel
{
    public class IonOptionContent : IonContent
    {
        public string value { get; set; }


        [JsonIgnore]
        public string selectedOption
        {
            get { return value; }
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
                IonOptionContent content = (IonOptionContent)obj;

                return value.Equals( content.value );
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