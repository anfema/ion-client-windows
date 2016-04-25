﻿using Newtonsoft.Json;
using Windows.UI;

namespace Anfema.Ion.DataModel
{
    public class IonColorContent : IonContent
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
        public int a { get; set; }


        // Return a color object from the stored values
        [JsonIgnore]
        public Color color
        {
            get { return new Color { R = (byte)r, G = (byte)g, B = (byte)b, A = (byte)a }; }
        }


        /// <summary>
        /// Checks the given IonColor for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if both IonColors are equal</returns>
        public override bool Equals( object obj )
        {
            // Basic IonContent equality check
            if( !base.Equals( obj ) )
            {
                return false;
            }
            
            try
            {
                // Try to cast to color and check for equality
                Color otherColor = (Color)obj;

                return color.Equals( otherColor ); ;
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
