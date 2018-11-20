using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Settlers.Toolbox.Infrastructure.Reporting.Interfaces;
using Settlers.Toolbox.Model.Compatibility.Interfaces;

namespace Settlers.Toolbox.Model.Compatibility
{
    public class CompatibilityManager : ICompatibilityManager
    {
        private const string D3DIMM_RELATIVE_PATH = @"Exe\D3DImm.dll";
        private const string DDRAW_RELATIVE_PATH  = @"Exe\DDraw.dll";

        private readonly IReportManager _ReportManager;

        public CompatibilityManager(IReportManager reportManager)
        {
            if (reportManager == null) throw new ArgumentNullException(nameof(reportManager));

            _ReportManager = reportManager;
        }

        public bool IsFixApplied(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            IReadOnlyList<FileInfo> compatibilityDllFiles = GetCompatibilityDllFiles(installDir);

            return compatibilityDllFiles.All(file => file.Exists);
        }

        public void ApplyFix(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            var d3DImmFile = new FileInfo(Path.Combine(installDir.FullName, D3DIMM_RELATIVE_PATH));
            var dDrawFile = new FileInfo(Path.Combine(installDir.FullName, DDRAW_RELATIVE_PATH));

            if (d3DImmFile.Exists)
                d3DImmFile.Delete();

            File.WriteAllBytes(d3DImmFile.FullName, Properties.Resources.D3DImm);

            if (dDrawFile.Exists)
                dDrawFile.Delete();
            
            File.WriteAllBytes(dDrawFile.FullName, Properties.Resources.DDraw);

            _ReportManager.ReportMessage("Compatibility fix applied ... OK");
        }

        public void RemoveFix(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            IReadOnlyList<FileInfo> compatibilityDllFiles = GetCompatibilityDllFiles(installDir);

            foreach (FileInfo file in compatibilityDllFiles)
            {
                if (file.Exists)
                    file.Delete();
            }

            _ReportManager.ReportMessage("Compatibility fix removed ... OK");
        }

        private IReadOnlyList<FileInfo> GetCompatibilityDllFiles(DirectoryInfo installDir)
        {
            return new List<FileInfo>
            {
                new FileInfo(Path.Combine(installDir.FullName, D3DIMM_RELATIVE_PATH)),
                new FileInfo(Path.Combine(installDir.FullName, DDRAW_RELATIVE_PATH))
            };
        }
    }
}