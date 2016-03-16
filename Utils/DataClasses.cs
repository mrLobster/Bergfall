using System;

namespace Bergfall.Utils
{
    public class TorrentQuery
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public DateTime UploadDate { get; set; }

        public int Seeds { get; set; }

        public int Leechers { get; set; }

        public DateTime Date { get; set; }

        public string Size { get; set; }
    }

    public enum FileType
    {
        Info,
        Music,
        TV,
        Movie,
        Application,
        Game,
        Porn,
        Video,
        Package,
        DiscImage,
        Other
    }

    public enum StringVerifier
    {
        Email,
        Country,
        URL
    }
}