using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Settlers.Toolbox.Infrastructure;
using Settlers.Toolbox.Infrastructure.Json.Interfaces;
using Settlers.Toolbox.Infrastructure.Network.Interfaces;
using Settlers.Toolbox.Model.Updates.Interfaces;

namespace Settlers.Toolbox.Model.Updates
{
    public class UpdateChecker : IUpdateChecker
    {
        private const string JSON_VERSIONS_URL = @""; // TODO: FireEmerald: Add URL.

        private readonly IStringDownloader _StringDownloader;
        private readonly IJsonAdapter _JsonAdapter;

        public UpdateChecker(IStringDownloader stringDownloader, IJsonAdapter jsonAdapter)
        {
            if (stringDownloader == null) throw new ArgumentNullException(nameof(stringDownloader));
            if (jsonAdapter == null) throw new ArgumentNullException(nameof(jsonAdapter));

            _StringDownloader = stringDownloader;
            _JsonAdapter = jsonAdapter;
        }

        public Task<VersionPackage> RetrieveLatestUpdateOrNull()
        {
            return Task.Run(() =>
            {
                VersionPackages allVersionPackages = DownloadVersionPackages();

                if (allVersionPackages == null)
                    return null;

                IReadOnlyList<VersionPackage> newVersionPackages = FilterForNewerVersions(allVersionPackages);

                if (!newVersionPackages.Any())
                    return null;

                return CombineToSinglePackage(newVersionPackages);
            });
        }

        private VersionPackages DownloadVersionPackages()
        {
            var json = _StringDownloader.DownloadString(JSON_VERSIONS_URL);

            return string.IsNullOrEmpty(json) ? null : _JsonAdapter.DeserializeObject<VersionPackages>(json);
        }

        private IReadOnlyList<VersionPackage> FilterForNewerVersions(VersionPackages allVersionPackages)
        {
            if (allVersionPackages == null) throw new ArgumentNullException(nameof(allVersionPackages));

            return allVersionPackages.Versions.Where(package => package.Version > AssemblyInfo.Version).ToList();
        }

        private VersionPackage CombineToSinglePackage(IReadOnlyList<VersionPackage> versionPackages)
        {
            if (versionPackages == null || !versionPackages.Any()) throw new ArgumentNullException(nameof(versionPackages));

            VersionPackage combinedPackage = versionPackages.OrderBy(package => package.Version).Last();
            combinedPackage.DeltaChangelog = MergeChangelogs(versionPackages);

            return combinedPackage;
        }

        private string MergeChangelogs(IReadOnlyList<VersionPackage> versionPackages)
        {
            var changelogBuilder = new StringBuilder();

            foreach (VersionPackage versionPackage in versionPackages.OrderBy(package => package.Version))
            {
                changelogBuilder.Append($"{versionPackage.Version} ({TimeZone.CurrentTimeZone.ToLocalTime(versionPackage.ReleaseDate):dd.MM.yyyy})" + Environment.NewLine +
                                        versionPackage.Changelog + Environment.NewLine +
                                        Environment.NewLine);
            }

            return changelogBuilder.ToString();
        }
    }
}