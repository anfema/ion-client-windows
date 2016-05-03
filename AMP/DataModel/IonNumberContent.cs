using Anfema.Ion.Utils;

namespace Anfema.Ion.DataModel
{
    public class IonNumberContent : IonContent
    {
        public float value { get; set; }


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
                IonNumberContent content = (IonNumberContent)obj;

                return value == content.value;
            }

            catch
            {
                return false;
            }

        }


        /// <summary>
        /// Returns the hascode computed by its elements
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return EqualsUtils.calcHashCode( value, outlet, type );
        }
    }
}
