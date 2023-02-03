namespace MusicFinder.MeloBit
{
    public class Product
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }

        public bool Completed { get; set; }
    }

    public class Item
    {
        public string Type { get; set; }
        public Artist Artist { get; set; }
        public string Position { get; set; }

    }
    public class Album
    {
        public string Name { get; set; }
        public List<Artist> Artists { get; set; }
        public Image Image { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Id { get; set; }
    }

    public class Artist
    {
        public string Id { get; set; }
        public int FollowersCount { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string SumSongsDownloadsCount { get; set; }
        public Image Image { get; set; }
    }

    public class Artist2
    {
        public string Id { get; set; }
        public int FollowersCount { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string SumSongsDownloadsCount { get; set; }
        public Image Image { get; set; }
    }

    public class Audio
    {
        public Medium Medium { get; set; }
        public High High { get; set; }
    }

    public class Cover
    {
        public string Url { get; set; }
    }

    public class CoverSmall
    {
        public string Url { get; set; }
    }

    public class High
    {
        public string Fingerprint { get; set; }
        public string Url { get; set; }
    }

    public class Image
    {
        public ThumbnailSmall ThumbnailSmall { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public CoverSmall CoverSmall { get; set; }
        public Cover Cover { get; set; }
        public Slider Slider { get; set; }
    }

    public class Medium
    {
        public string Fingerprint { get; set; }
        public string Url { get; set; }
    }

    public class Result
    {
        public string Type { get; set; }
        public Artist Artist { get; set; }
        public int Position { get; set; }
        public Song Song { get; set; }
        public Album Album { get; set; }
    }

    public class Root
    {
        public bool Ok { get; set; }
        public string Channel { get; set; }
        public int Total { get; set; }
        public List<Result> Results { get; set; }
    }

    public class Slider
    {
        public string Url { get; set; }
    }

    public class Song
    {
        public string Id { get; set; }
        public Album Album { get; set; }
        public List<Artist> Artists { get; set; }
        public Audio Audio { get; set; }
        public bool Copyrighted { get; set; }
        public bool Localized { get; set; }

        private string _downloadCount;
        public string DownloadCount
        {
            get
            {
                return _downloadCount;
            }
            set
            {
                if (value.Contains("."))
                {
                    if (value.Contains("k"))
                    {
                        _downloadCount = value.Remove(value.IndexOf('.')) + "000";
                    }
                    else if (value.Contains("M"))
                    {
                        _downloadCount = value.Remove(value.IndexOf('.')) + "000000";
                    }
                }
                else
                {
                    if (value.Contains("k"))
                    {
                        _downloadCount = value.Remove(value.IndexOf('k')) + "000";
                    }
                    else if (value.Contains("M"))
                    {
                        _downloadCount = value.Remove(value.IndexOf('M')) + "000000";
                    }
                    else
                    {
                        _downloadCount = value;
                    }
                }
                if (_downloadCount.Contains("<"))
                {
                    _downloadCount = _downloadCount.Replace('<', ' ');

                }
                if (_downloadCount.Contains(">"))
                {
                    _downloadCount = _downloadCount.Replace('>', ' '); ;
                }

            }
        }
        public int Duration { get; set; }
        public bool HasVideo { get; set; }
        public string Title { get; set; }
        public Image Image { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class Thumbnail
    {
        public string Url { get; set; }
    }

    public class ThumbnailSmall
    {
        public string Url { get; set; }
    }

    public enum SearchType
    {
        MostRelated = 0,
        MostDownloaded = 1,

    }
}