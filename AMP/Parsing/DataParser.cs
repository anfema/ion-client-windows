using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Anfema.Amp.Parsing
{
    public static class DataParser
    {/*
        /// <summary>
        /// Parses the content from the generic data type to a more specific type
        /// </summary>
        /// <param name="content"></param>
        /// <returns>A Object that contains all data in observable collections</returns>
        private static AmpPageObservableCollection parseContent( List<ContentNodeRaw> content )
        {
            AmpPageObservableCollection allContent = new AmpPageObservableCollection();

            for( int i=0; i<content.Count; i++ )
            {
                // Check if the current content node "is_available"
                if( !content[i].is_available )
                {
                    AmpContent contentNode = new AmpContent();
                    contentNode.init(content[i]);
                }

                switch( content[i].type )
                {
                    case "textcontent":
                        {
                            AmpTextContent text = new AmpTextContent();
                            text.init(content[i]);
                            allContent.textContent.Add(text);
                            break;
                        }
                    case "imagecontent":
                        {
                            AmpImageContent image = new AmpImageContent();
                            image.init(content[i]);
                            allContent.imageContent.Add(image);
                            break;
                        }
                    case "colorcontent":
                        {
                            AmpColorContent color = new AmpColorContent();
                            color.init(content[i]);
                            allContent.colorContent.Add(color);
                            break;
                        }
                    case "flagcontent":
                        {
                            AmpFlagContent flag = new AmpFlagContent();
                            flag.init(content[i]);
                            allContent.flagContent.Add(flag);
                            break;
                        }
                    case "filecontent":
                        {
                            AmpFileContent file = new AmpFileContent();
                            file.init(content[i]);
                            allContent.fileContent.Add(file);
                            break;
                        }
                    case "mediacontent":
                        {
                            AmpMediaContent media = new AmpMediaContent();
                            media.init(content[i]);
                            allContent.mediaContent.Add(media);
                            break;
                        }
                    case "datetimecontent":
                        {
                            AmpDateTimeContent dateTime = new AmpDateTimeContent();
                            dateTime.init(content[i]);
                            allContent.dateTimeContent.Add(dateTime);
                            break;
                        }
                    case "optioncontent":
                        {
                            AmpOptionContent option = new AmpOptionContent();
                            option.init(content[i]);
                            allContent.optionContent.Add(option);
                            break;
                        }
                    case "kvcontent":
                        {
                            AmpKeyValueContent keyValue = new AmpKeyValueContent();
                            keyValue.init(content[i]);
                            allContent.keyValueContent.Add(keyValue);
                            break;
                        }
                    case "connectioncontent":
                        {
                            AmpConnectionContent connectionContent = new AmpConnectionContent();
                            connectionContent.init(content[i]);
                            allContent.connectionContent.Add(connectionContent);
                            break;
                        }
                }
            }

            return allContent;
        }
        */


        /// <summary>
        /// Parses a given raw json to a AmpPage
        /// </summary>
        /// <param name="pageRaw"></param>
        /// <returns>AmpPage without the generic data types</returns>
        public static async Task<AmpPage> parsePage(HttpResponseMessage response)
        {
            // Extract the json string from the content of the response message
            string responseString = await response.Content.ReadAsStringAsync();

            // Parse the page to a raw page container
            AmpPage pageParsed = new AmpPage();

            try
            {
                AMPPageRoot pageParsedNew = JsonConvert.DeserializeObject<AMPPageRoot>(responseString);
                pageParsed = pageParsedNew.page[0];
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error deserializing page json: " + e.Message);
            }


            PageRaw pageRaw = JsonConvert.DeserializeObject<PageRootRaw>( responseString ).page[0];


            
            // Copy entries that don't have to be modified

            /*
            // Parse all content
            for (int j = 0; j < pageRaw.contents.Count; j++)
            {
                AmpPageContent content = new AmpPageContent();

                // copy untouched parameters
                content.outlet = pageRaw.contents[j].outlet;
                content.type = pageRaw.contents[j].type;
                content.variation = pageRaw.contents[j].variation;

                // Parse the whole content
                AmpPageObservableCollection pageContent = parseContent(pageRaw.contents[j].children);
                content.children.Add(pageContent);

                pageParsed.contents.Add(content);
            }       */    
            
            return pageParsed;
        }


        /// <summary>
        /// Parses the content of a HttpResponseMessage as a collection and returns the first collection in the array
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<AmpCollection> parseCollection( HttpResponseMessage response )
        {
            return JsonConvert.DeserializeObject<CollectionRoot>( await response.Content.ReadAsStringAsync() ).collection[0];
        }
    }



    public class AmpContentConverter : Newtonsoft.Json.Converters.CustomCreationConverter<AmpContent>
    {
        public override AmpContent Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        protected AmpContent Create(Type objectType, JObject jObject)
        {
            string objectTypeString = (string) jObject.Property("type");

            switch(objectTypeString)
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
