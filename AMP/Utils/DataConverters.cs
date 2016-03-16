using Anfema.Amp.DataModel;
using System.Collections.Generic;


namespace Anfema.Amp.Utils
{
    public static class DataConverters
    {
        /// <summary>
        /// Helps to convert a list of AmpContent to a PageObservableCollection
        /// </summary>
        /// <param name="list"></param>
        /// <returns>ObservableCollection with all the content of the list given</returns>
        public static AmpPageObservableCollection convertContent( List<AmpContent> list )
        {
            AmpPageObservableCollection oc = new AmpPageObservableCollection();

            for( int i=0; i< list.Count; i++ )
            {
                switch (list[i].type)
                {
                    case "textcontent":
                        {
                            oc.textContent.Add((AmpTextContent)list[i]);
                            break;
                        }
                    case "imagecontent":
                        {
                            oc.imageContent.Add((AmpImageContent)list[i]);
                            break;
                        }
                    case "colorcontent":
                        {
                            oc.colorContent.Add((AmpColorContent)list[i]);
                            break;
                        }
                    case "datetimecontent":
                        {
                            oc.dateTimeContent.Add((AmpDateTimeContent)list[i]);
                            break;
                        }
                    case "filecontent":
                        {
                            oc.fileContent.Add((AmpFileContent)list[i]);
                            break;
                        }
                    case "flagcontent":
                        {
                            oc.flagContent.Add((AmpFlagContent)list[i]);
                            break;
                        }
                    case "mediacontent":
                        {
                            oc.mediaContent.Add((AmpMediaContent)list[i]);
                            break;
                        }
                    case "optioncontent":
                        {
                            oc.optionContent.Add((AmpOptionContent)list[i]);
                            break;
                        }
                    case "numbercontent":
                        {
                            oc.numberContent.Add((AmpNumberContent)list[i]);
                            break;
                        }
                    case "connectioncontent":
                        {
                            oc.connectionContent.Add((AmpConnectionContent)list[i]);
                            break;
                        }
                }
            }

            return oc;
        }
    }
}
