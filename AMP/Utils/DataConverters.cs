using Anfema.Ion.DataModel;
using System.Collections.Generic;


namespace Anfema.Ion.Utils
{
    public static class DataConverters
    {
        /// <summary>
        /// Helps to convert a list of IonContent to a PageObservableCollection
        /// </summary>
        /// <param name="list"></param>
        /// <returns>ObservableCollection with all the content of the list given</returns>
        public static IonPageObservableCollection convertContent( List<IonContent> list )
        {
            IonPageObservableCollection oc = new IonPageObservableCollection();

            for( int i=0; i< list.Count; i++ )
            {
                // Check if the item is avialible (means that is has all properties filled with data)
                if( !list[i].isAvailable )
                {
                   continue;
                }

                switch (list[i].type)
                {
                    case "textcontent":
                        {
                            oc.textContent.Add((IonTextContent)list[i]);
                            break;
                        }
                    case "imagecontent":
                        {
                            oc.imageContent.Add((IonImageContent)list[i]);
                            break;
                        }
                    case "colorcontent":
                        {
                            oc.colorContent.Add((IonColorContent)list[i]);
                            break;
                        }
                    case "datetimecontent":
                        {
                            oc.dateTimeContent.Add((IonDateTimeContent)list[i]);
                            break;
                        }
                    case "filecontent":
                        {
                            oc.fileContent.Add((IonFileContent)list[i]);
                            break;
                        }
                    case "flagcontent":
                        {
                            oc.flagContent.Add((IonFlagContent)list[i]);
                            break;
                        }
                    case "mediacontent":
                        {
                            oc.mediaContent.Add((IonMediaContent)list[i]);
                            break;
                        }
                    case "optioncontent":
                        {
                            oc.optionContent.Add((IonOptionContent)list[i]);
                            break;
                        }
                    case "numbercontent":
                        {
                            oc.numberContent.Add((IonNumberContent)list[i]);
                            break;
                        }
                    case "connectioncontent":
                        {
                            oc.connectionContent.Add((IonConnectionContent)list[i]);
                            break;
                        }
                }
            }

            return oc;
        }
    }
}
