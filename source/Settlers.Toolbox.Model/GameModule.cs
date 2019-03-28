using System;

namespace Settlers.Toolbox.Model
{
    [Flags]
    public enum GameModule
    {
        None             = 0,
        Language         = 1,
        Resolution       = 2,
        CompatibilityFix = 4,
        TexturePack      = 8
    }
}