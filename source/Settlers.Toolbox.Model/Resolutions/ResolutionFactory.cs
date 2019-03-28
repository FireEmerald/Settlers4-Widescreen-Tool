using System;
using System.Collections.Generic;

using Settlers.Toolbox.Model.Resolutions.Interfaces;

namespace Settlers.Toolbox.Model.Resolutions
{
    public class ResolutionFactory : IResolutionFactory
    {
        public Resolution Create(GameResolution gameResolution)
        {
            switch (gameResolution)
            {
                case GameResolution.Default:
                    return new Resolution(gameResolution, 1024, 768);
                case GameResolution.R1024_600:
                    return new Resolution(gameResolution, 1024, 600);
                case GameResolution.R1280_720:
                    return new Resolution(gameResolution, 1280, 720);
                case GameResolution.R1280_800:
                    return new Resolution(gameResolution, 1280, 800);
                case GameResolution.R1366_768:
                    return new Resolution(gameResolution, 1366, 768);
                case GameResolution.R1440_900:
                    return new Resolution(gameResolution, 1440, 900);
                case GameResolution.R1680_1050:
                    return new Resolution(gameResolution, 1680, 1050);
                case GameResolution.R1920_1080:
                    return new Resolution(gameResolution, 1920, 1080);
                case GameResolution.R1920_1200:
                    return new Resolution(gameResolution, 1920, 1200);
                case GameResolution.R2560_1440:
                    return new Resolution(gameResolution, 2560, 1440);
                case GameResolution.R3840_2160:
                    return new Resolution(gameResolution, 3840, 2160);

                default:
                    throw new InvalidOperationException($"Unable to create {nameof(Resolution)}, {gameResolution} is not implemented.");
            }
        }

        public Resolution CreateCustom(ushort width, ushort height)
        {
            return new Resolution(GameResolution.Custom, width, height);
        }

        public IReadOnlyList<Resolution> CreateAllPresets()
        {
            var allPresetResolutions = new List<Resolution>();

            foreach (var gameResolution in (GameResolution[])Enum.GetValues(typeof(GameResolution)))
            {
                if (gameResolution == GameResolution.Unknown || gameResolution == GameResolution.Custom)
                    continue;

                var presetResolution = Create(gameResolution);
                allPresetResolutions.Add(presetResolution);
            }

            return allPresetResolutions;
        }
    }
}