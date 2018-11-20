namespace Settlers.Toolbox.Model.Resolutions
{
    /// <summary>
    /// The supported resolutions.
    /// </summary>
    /// <remarks>
    /// See https://store.steampowered.com/hwsurvey/Steam-Hardware-Software-Survey-Welcome-to-Steam for global usage.
    /// </remarks>
    public enum GameResolution
    {
        Unknown    = -1,
        Default    = 0,
        R1024_600  = 1,
        R1280_720  = 2,
        R1280_800  = 3,
        R1366_768  = 4,
        R1440_900  = 5,
        R1680_1050 = 6,
        R1920_1080 = 7,
        R1920_1200 = 8,
        R2560_1440 = 9,
        R3840_2160 = 10,
        Custom     = 11
    }
}