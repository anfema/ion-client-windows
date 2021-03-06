﻿using Anfema.Ion.DataModel;
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

            for( int i = 0; i < list.Count; i++ )
            {
                // Check if the item is avialible (means that is has all properties filled with data)
                if( !list[i].isAvailable )
                {
                    continue;
                }

                switch( list[i].type )
                {
                    case IonConstants.TextContentIdentifier:
                        {
                            oc.textContent.Add( (IonTextContent)list[i] );
                            break;
                        }
                    case IonConstants.ImageContentIdentifier:
                        {
                            oc.imageContent.Add( (IonImageContent)list[i] );
                            break;
                        }
                    case IonConstants.ColorContentIdentifier:
                        {
                            oc.colorContent.Add( (IonColorContent)list[i] );
                            break;
                        }
                    case IonConstants.DateTimeContentIdentifier:
                        {
                            oc.dateTimeContent.Add( (IonDateTimeContent)list[i] );
                            break;
                        }
                    case IonConstants.FileContentIdentifier:
                        {
                            oc.fileContent.Add( (IonFileContent)list[i] );
                            break;
                        }
                    case IonConstants.FlagContentIdentifier:
                        {
                            oc.flagContent.Add( (IonFlagContent)list[i] );
                            break;
                        }
                    case IonConstants.MediaContentIdentifier:
                        {
                            oc.mediaContent.Add( (IonMediaContent)list[i] );
                            break;
                        }
                    case IonConstants.OptionContentIdentifier:
                        {
                            oc.optionContent.Add( (IonOptionContent)list[i] );
                            break;
                        }
                    case IonConstants.NumberContentIdentifier:
                        {
                            oc.numberContent.Add( (IonNumberContent)list[i] );
                            break;
                        }
                    case IonConstants.ConnectionContentIdentifier:
                        {
                            oc.connectionContent.Add( (IonConnectionContent)list[i] );
                            break;
                        }
                    case IonConstants.ChartContentIdentifier:
                        {
                            oc.chartContent.Add( (IonChartContent)list[i] );
                            break;
                        }
                }
            }

            return oc;
        }


        /// <summary>
        /// Uesd to get a list of strings with the identifiers of the given pages
        /// </summary>
        /// <param name="pages"></param>
        /// <returns>List of strings</returns>
        public static List<string> getPageIdentifier( List<IonPage> pages )
        {
            List<string> pageIdentifier = new List<string>();

            for( int i=0; i< pages.Count; i++ )
            {
                pageIdentifier.Add( pages[i].identifier );
            }

            return pageIdentifier;
        }


        /// <summary>
        /// Used to get a list of pageIdentifiers from a given list of pagePreviews
        /// </summary>
        /// <param name="pagePreviews"></param>
        /// <returns>List of identifier</returns>
        public static List<string> getPageIdentifier( List<IonPagePreview> pagePreviews )
        {
            List<string> pageIdentifier = new List<string>();

            for( int i=0; i<pagePreviews.Count; i++ )
            {
                pageIdentifier.Add( pagePreviews[i].identifier );
            }

            return pageIdentifier;
        }
    }
}
