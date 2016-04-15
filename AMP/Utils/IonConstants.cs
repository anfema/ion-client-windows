﻿namespace Anfema.Ion.Utils
{
    /// <summary>
    /// Class used to define all constants for ION apps
    /// </summary>
    public static class IonConstants
    {
        // Ion content identifier
        public const string TextContentIdentifier = "textcontent";
        public const string ImageContentIdentifier = "imagecontent";
        public const string ColorContentIdentifier = "colorcontent";
        public const string DateTimeContentIdentifier = "datetimecontent";
        public const string FileContentIdentifier = "filecontent";
        public const string FlagContentIdentifier = "flagcontent";
        public const string MediaContentIdentifier = "mediacontent";
        public const string OptionContentIdentifier = "optioncontent";
        public const string NumberContentIdentifier = "numbercontent";
        public const string ConnectionContentIdentifier = "connectioncontent";


        // Folder identifier
        public static string FtsDbFolderIdentifier { get { return "ftsDbs"; } }
        public static string MediaFolderIdentifier { get { return "media"; } }
        public static string ArchiveFolderIdentifier { get { return "archive"; } }
        public static string FileFolderIdentifier { get { return "file"; } }
        public static string TempFolderIdentifier { get { return "temp"; } }
        public static string CacheIndicesFolderIdentifier { get { return "cache_indices"; } }


        // Other constants
        public const string Slash = "\\";
        public static string QueryBegin { get { return "?"; } }
        public static string QueryVariation { get { return "variation="; } }
        public static string[] MediaUrlIndicators { get { return new string[2] { "/media/", "/protected_media/" }; } }
        public static string JsonFileExtension { get { return ".json"; } }
    }
}
