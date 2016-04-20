using Anfema.Ion.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json.Converters;


namespace Anfema.Ion.Parsing
{
    public class IonContentConverter : CustomCreationConverter<IonContent>
    {
        public override IonContent Create( Type objectType )
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates a new Class that derives from IonContent. Which class is being generated depends on the type-property
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="jObject"></param>
        /// <returns>Class defined by the type-property of the giben IonContent</returns>
        protected IonContent Create( Type objectType, JObject jObject )
        {
            string objectTypeString = (string)jObject.Property( "type" );

            switch( objectTypeString )
            {
                case "textcontent":
                    {
                        return new IonTextContent();
                    }
                case "imagecontent":
                    {
                        return new IonImageContent();
                    }
                case "colorcontent":
                    {
                        return new IonColorContent();
                    }
                case "datetimecontent":
                    {
                        return new IonDateTimeContent();
                    }
                case "filecontent":
                    {
                        return new IonFileContent();
                    }
                case "flagcontent":
                    {
                        return new IonFlagContent();
                    }
                case "mediacontent":
                    {
                        return new IonMediaContent();
                    }
                case "optioncontent":
                    {
                        return new IonOptionContent();
                    }
                case "numbercontent":
                    {
                        return new IonNumberContent();
                    }
                case "connectioncontent":
                    {
                        return new IonConnectionContent();
                    }
            }

            throw new JsonReaderException( string.Format( "The datetype " + objectTypeString + " is not defined for IonContent" ) );
        }


        /// <summary>
        /// Reads the current Json-object
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns>Populated object with the data of the node</returns>
        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            // Load JObject from stream
            JObject jObject = JObject.Load( reader );

            // Create target object based on JObject
            var target = Create( objectType, jObject );

            try
            {
                // Populate the object properties
                serializer.Populate( jObject.CreateReader(), target );

                return target;
            }
            catch( JsonSerializationException e )
            {
                // This sets a dateTime with value null to the minimal available time. This null is caused by a AMP bug!!!
                if( target.GetType() == typeof( IonDateTimeContent ) )
                {
                    return DateTime.MinValue;
                }
                throw new JsonSerializationException( "Error reading JSON-token " + target + " with value " + jObject.ToString(), e );
            }
        }
    }
}
