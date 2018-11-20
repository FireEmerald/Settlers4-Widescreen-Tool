using System;

namespace Settlers.Toolbox.Model.Updates
{
    public class VersionPackage
    {
        public Version Version { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public string DownloadLink { get; private set; }
        public string Changelog { get; private set; }

        public string DeltaChangelog { get; set; }

        public VersionPackage(Version version, DateTime releaseDate, string downloadLink, string changelog)
        {
            Version = version;
            ReleaseDate = releaseDate;
            DownloadLink = downloadLink;
            Changelog = changelog;

            DeltaChangelog = string.Empty;
        }
    }
}