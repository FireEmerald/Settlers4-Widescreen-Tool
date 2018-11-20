using System;
using System.Globalization;
using System.Windows.Data;

using Settlers.Toolbox.Model.Resolutions;

namespace Settlers.Toolbox.Views.Converter
{
    [ValueConversion(typeof(Resolution), typeof(string))]
    public class ResolutionToStringConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is GameResolution)) throw new InvalidOperationException($"Passed type is invalid, {nameof(ResolutionToStringConverter)} requires a {nameof(GameResolution)}.");

            var resolution = (GameResolution) value;
            switch (resolution)
            {
                case GameResolution.Default:
                    return "1024 x 768 Pixel (4:3) (Default)";
                case GameResolution.R1024_600:
                    return "1024 x 600 Pixel (16:9)";
                case GameResolution.R1280_720:
                    return "1280 x 720 Pixel (16:9)";
                case GameResolution.R1280_800:
                    return "1280 x 800 Pixel (16:10)"; // 16:10 is also called 8:5, but 16:10 is commonly used.
                case GameResolution.R1366_768:
                    return "1366 x 768 Pixel (16:9)";
                case GameResolution.R1440_900:
                    return "1440 x 900 Pixel (16:10)";
                case GameResolution.R1680_1050:
                    return "1680 x 1050 Pixel (16:10)";
                case GameResolution.R1920_1080:
                    return "1920 x 1080 Pixel (16:9)"; // FULL HD
                case GameResolution.R1920_1200:
                    return "1920 x 1200 Pixel (16:10)";
                case GameResolution.R2560_1440:
                    return "2560 x 1440 Pixel (16:9)"; // WQHD
                case GameResolution.R3840_2160:
                    return "3840 x 2160 Pixel (16:9)"; // 4K UHD
                case GameResolution.Custom:
                    return "Custom resolution...";

                default:
                    throw new InvalidOperationException($"Unable to convert resolution, {resolution} is not implemented.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}