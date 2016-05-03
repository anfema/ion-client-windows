using System;
using System.Collections.Generic;


namespace Anfema.Ion.Utils
{
    public class EqualsUtils
    {
        /// <summary>
        /// Checks two lists for equality
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if lists are equal, false otherwise</returns>
        /// http://www.dotnetperls.com/list-equals
        public static bool UnorderedEqual<T>( ICollection<T> a, ICollection<T> b )
        {
            // 1: Require that the counts are equal
            if( a.Count != b.Count )
            {
                return false;
            }

            // 2: Initialize new Dictionary of the type
            Dictionary<T, int> d = new Dictionary<T, int>();

            // 3: Add each key's frequency from collection A to the Dictionary
            foreach( T item in a )
            {
                int c;
                if( d.TryGetValue( item, out c ) )
                {
                    d[item] = c + 1;
                }
                else
                {
                    d.Add( item, 1 );
                }
            }

            // 4: Add each key's frequency from collection B to the Dictionary
            // Return early if we detect a mismatch
            foreach( T item in b )
            {
                int c;
                if( d.TryGetValue( item, out c ) )
                {
                    if( c == 0 )
                    {
                        return false;
                    }
                    else
                    {
                        d[item] = c - 1;
                    }
                }
                else
                {
                    // Not in dictionary
                    return false;
                }
            }

            // 5: Verify that all frequencies are zero
            foreach( int v in d.Values )
            {
                if( v != 0 )
                {
                    return false;
                }
            }

            // 6: We know the collections are equal
            return true;
        }


        /// <summary>
        /// Calculates a hashCode for up to three given parameters
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="obj3"></param>
        /// <returns>Integer hashCode</returns>
        public static int calcHashCode( object obj1, object obj2 = null, object obj3 = null )
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int hashingBase = (int)2166136261;
                const int hashingMultiplier = 16777619;

                int hash = hashingBase;
                hash = ( hash * hashingMultiplier ) ^ ( !Object.ReferenceEquals( null, obj1 ) ? obj1.GetHashCode() : 0 );
                hash = ( hash * hashingMultiplier ) ^ ( !Object.ReferenceEquals( null, obj2 ) ? obj2.GetHashCode() : 0 );
                hash = ( hash * hashingMultiplier ) ^ ( !Object.ReferenceEquals( null, obj3 ) ? obj3.GetHashCode() : 0 );

                return hash;
            }
        }
    }
}
