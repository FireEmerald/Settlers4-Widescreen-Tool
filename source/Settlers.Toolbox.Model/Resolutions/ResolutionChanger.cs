using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Settlers.Toolbox.Infrastructure.IO.Interfaces;
using Settlers.Toolbox.Infrastructure.Logging;
using Settlers.Toolbox.Model.Resolutions.Interfaces;

namespace Settlers.Toolbox.Model.Resolutions
{
    public class ResolutionChanger : IResolutionChanger
    {
        private const string GFX_ENGINE_RELATIVE_PATH = @"Exe\GfxEngine.dll";

        private const int GFX_ENGINE_POS_WIDTH  = 0x0001068E;
        private const int GFX_ENGINE_POS_HEIGHT = 0x00010693;

        private const string GAMESETTINGS_RELATIVE_PATH = @"Config\GameSettings.cfg";
        private const string GAMESETTINGS_SECTION = "GAMESETTINGS";
        private const string GAMESETTINGS_KEY_BORDERSCROLL = "BorderScroll";
        private const string GAMESETTINGS_KEY_FULLSCREEN   = "Fullscreen";
        private const string GAMESETTINGS_KEY_SCREENMODE   = "Screenmode";
        private const string GAMESETTINGS_KEY_WINDOWHEIGHT = "WindowHeight";
        private const string GAMESETTINGS_KEY_WINDOWWIDTH  = "WindowWidth";

        private readonly IIniFileAdapter _IniFileAdapter;
        private readonly IResolutionFactory _ResolutionFactory;

        public ResolutionChanger(IIniFileAdapter iniFileAdapter, IResolutionFactory resolutionFactory)
        {
            if (iniFileAdapter == null) throw new ArgumentNullException(nameof(iniFileAdapter));
            if (resolutionFactory == null) throw new ArgumentNullException(nameof(resolutionFactory));

            _IniFileAdapter = iniFileAdapter;
            _ResolutionFactory = resolutionFactory;
        }

        public Resolution DetectResolution(DirectoryInfo installDir)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            FileInfo gfxEngineFile = GetGfxEngineFile(installDir);

            ushort detectedWidth  = ReadData(gfxEngineFile, GFX_ENGINE_POS_WIDTH);
            ushort detectedHeight = ReadData(gfxEngineFile, GFX_ENGINE_POS_HEIGHT);

            IReadOnlyList<Resolution> allPresetResolutions = _ResolutionFactory.CreateAllPresets();

            Resolution detectedResolution = allPresetResolutions.SingleOrDefault(res => res.Width == detectedWidth && res.Height == detectedHeight);

            return detectedResolution ?? _ResolutionFactory.CreateCustom(detectedWidth, detectedHeight);
        }

        private ushort ReadData(FileInfo file, int position)
        {
            byte[] buffer = new byte[2];

            using (Stream stream = file.OpenRead())
            {
                stream.Position = position;
                stream.Read(buffer, 0, buffer.Length);
            }

            buffer = SwitchBytes(buffer);
            return BitConverter.ToUInt16(buffer, 0);
        }

        public void ChangeResolution(DirectoryInfo installDir, GameResolution gameResolution)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            Resolution resolution = _ResolutionFactory.Create(gameResolution);

            ChangeResolution(installDir, resolution);
        }

        public void ChangeResolution(DirectoryInfo installDir, ushort width, ushort height)
        {
            if (installDir == null) throw new ArgumentNullException(nameof(installDir));

            Resolution resolution = _ResolutionFactory.CreateCustom(width, height);

            ChangeResolution(installDir, resolution);
        }

        private void ChangeResolution(DirectoryInfo installDir, Resolution resolution)
        {
            PatchGfxEngineFile(installDir, resolution.Width, resolution.Height);
            UpdateGameSettingsResolution(installDir, resolution.Width, resolution.Height);

            LogAdapter.Log(LogLevel.Information, "Resolution changed ... OK");
        }

        private FileInfo GetGfxEngineFile(DirectoryInfo installDir)
        {
            var gfxEngineFile = new FileInfo(Path.Combine(installDir.FullName, GFX_ENGINE_RELATIVE_PATH));

            if (!gfxEngineFile.Exists)
            {
                throw new FileNotFoundException($"Unable to access '{GFX_ENGINE_RELATIVE_PATH}', file does not exist.");
            }

            return gfxEngineFile;
        }

        private void PatchGfxEngineFile(DirectoryInfo installDir, ushort width, ushort height)
        {
            FileInfo gfxEngineFile = GetGfxEngineFile(installDir);

            byte[] widthBytes = BitConverter.GetBytes(width);
            byte[] heightBytes = BitConverter.GetBytes(height);

            widthBytes = SwitchBytes(widthBytes);
            heightBytes = SwitchBytes(heightBytes);

            ReplaceData(gfxEngineFile, GFX_ENGINE_POS_WIDTH, widthBytes);
            ReplaceData(gfxEngineFile, GFX_ENGINE_POS_HEIGHT, heightBytes);
        }

        private byte[] SwitchBytes(byte[] bytes)
        {
            // The width/height of the resolution is stored in a block of 2 byte.
            // Before/after the width/height is written/read the high-byte must be switched with the low-byte.
            // 0x1234 -> 0x3412
            return new[] { bytes[1], bytes[0] };
        }

        private void ReplaceData(FileInfo file, int position, byte[] data)
        {
            using (Stream stream = file.OpenWrite())
            {
                stream.Position = position;
                stream.Write(data, 0, data.Length);
            }
        }

        private void UpdateGameSettingsResolution(DirectoryInfo installDir, int width, int height)
        {
            var gameSettingsFile = new FileInfo(Path.Combine(installDir.FullName, GAMESETTINGS_RELATIVE_PATH));

            if (!gameSettingsFile.Exists)
            {
                throw new FileNotFoundException($"Unable to access '{GAMESETTINGS_RELATIVE_PATH}', file does not exist.");
            }

            // TODO: FireEmerald: Validate values and move perhaps to const/enum e.g.
            _IniFileAdapter.WriteValueToFile(GAMESETTINGS_SECTION, GAMESETTINGS_KEY_BORDERSCROLL, "1", gameSettingsFile.FullName);
            _IniFileAdapter.WriteValueToFile(GAMESETTINGS_SECTION, GAMESETTINGS_KEY_FULLSCREEN,   "1", gameSettingsFile.FullName);
            _IniFileAdapter.WriteValueToFile(GAMESETTINGS_SECTION, GAMESETTINGS_KEY_SCREENMODE,   "2", gameSettingsFile.FullName);
            _IniFileAdapter.WriteValueToFile(GAMESETTINGS_SECTION, GAMESETTINGS_KEY_WINDOWHEIGHT, height.ToString(), gameSettingsFile.FullName);
            _IniFileAdapter.WriteValueToFile(GAMESETTINGS_SECTION, GAMESETTINGS_KEY_WINDOWWIDTH,  width.ToString(),  gameSettingsFile.FullName);
        }
    }
}