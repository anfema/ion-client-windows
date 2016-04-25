using System;


namespace Anfema.Ion.DataModel
{
    public class IonDateTimeContent : IonContent
    {
        public DateTime dateTime { get; set; }


        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if objects are equal, false otherwise</returns>
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
                IonDateTimeContent content = (IonDateTimeContent)obj;

                return dateTime == content.dateTime;
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
