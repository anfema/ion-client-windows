using Anfema.Amp.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json.Converters;


namespace Anfema.Amp.Parsing
{
    public class AmpContentConverter : CustomCreationConverter<AmpContent>
    {
        public override AmpContent Create(Type objectType)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates a new Class that derives from AmpContent. Which class is being generated depends on the type-property
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="jObject"></param>
        /// <returns>Class defined by the type-property of the giben AmpContent</returns>
        protected AmpContent Create(Type objectType, JObject jObject)
        {
            string objectTypeString = (string)jObject.Property("type");

            switch (objectTypeString)
            {
                case "textcontent":
                    {
                        return new AmpTextContent();
                    }
                case "imagecontent":
                    {
                        return new AmpImageContent();
                    }
                case "colorcontent":
                    {
                        return new AmpColorContent();
                    }
                case "datetimecontent":
                    {
                        return new AmpDateTimeContent();
                    }
                case "filecontent":
                    {
                        return new AmpFileContent();
                    }
                case "flagcontent":
                    {
                        return new AmpFlagContent();
                    }
                case "mediacontent":
                    {
                        return new AmpMediaContent();
                    }
                case "optioncontent":
                    {
                        return new AmpOptionContent();
                    }
                case "numbercontent":
                    {
                        return new AmpNumberContent();
                    }
                case "connectioncontent":
                    {
                        return new AmpConnectionContent();
                    }
            }
            return null;

            //throw new ApplicationException(string.Format("The datetype " + objectTypeString + " is not defined.");
        }


        /// <summary>
        /// Reads the current Json-object
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns>Populated object with the data of the node</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            var target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
