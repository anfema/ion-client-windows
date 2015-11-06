using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp
{
    public class DataParser
    {
        // Parses the content from the generic data type to a more specific type
        public AmpPageContent parseContent( List<ContentNodeRaw> content )
        {
            AmpPageContent allContent = new AmpPageContent();

            for( int i=0; i<content.Count; i++ )
            {
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
                }
            }

            return allContent;
        }


        public AmpPage parsePage(AmpPageRaw pageRaw)
        {
            AmpPage pageParsed = new AmpPage();

            // Copy entries that don't have to be modified
            pageParsed.children = pageRaw.children;
            pageParsed.collection = pageRaw.collection;
            pageParsed.identifier = pageRaw.identifier;
            pageParsed.parent = pageRaw.parent;

            // Parse all translations
            for (int i = 0; i < pageRaw.translations.Count; i++)
            {
                AmpPageTranslation translation = new AmpPageTranslation();

                // copy untouched parameters
                translation.locale = pageRaw.translations[i].locale;

                // Parse all content
                for (int j = 0; j < pageRaw.translations[i].content.Count; j++)
                {
                    AmpPageTranslationContent content = new AmpPageTranslationContent();

                    // copy untouched parameters
                    content.outlet = pageRaw.translations[i].content[j].outlet;
                    content.type = pageRaw.translations[i].content[j].type;
                    content.variation = pageRaw.translations[i].content[j].variation;

                    // Parse all children
                    for (int k = 0; k < pageRaw.translations[i].content[j].children.Count; k++)
                    {
                        // Parse the whole content
                        AmpPageContent pageContent = parseContent(pageRaw.translations[i].content[j].children);
                        content.children.Add(pageContent);
                    }

                    translation.content.Add(content);
                }

                pageParsed.translations.Add(translation);
            }

            return pageParsed;
        }


        // Parses a list of raw pages
        public List<AmpPage> parsePages(List<AmpPageRaw> pagesRaw)
        {
            Debug.WriteLine("{0}.{1}: {2}", DateTime.Now.Second, DateTime.Now.Millisecond, "Parsing started");
            List<AmpPage> pagesParseed = new List<AmpPage>();

            for (int i = 0; i < pagesRaw.Count; i++)
            {
                AmpPage pageParsed = parsePage(pagesRaw[i]);

                pagesParseed.Add(pageParsed);
            }

            Debug.WriteLine("{0}.{1}: {2}", DateTime.Now.Second, DateTime.Now.Millisecond, "Parsing finished");
            return pagesParseed;
        }
    }
}
