using System;
using System.Globalization;
using System.Windows.Data;

using Settlers.Toolbox.Model.Languages;

namespace Settlers.Toolbox.Views.Converter
{
    [ValueConversion(typeof(GameLanguage), typeof(string))]
    public class LanguageToStringConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is GameLanguage)) throw new InvalidOperationException($"Passed type is invalid, {nameof(LanguageToStringConverter)} requires a {nameof(GameLanguage)}.");

            var gameLanguage = (GameLanguage) value;
            switch (gameLanguage)
            {
                case GameLanguage.English:
                    return "English";
                case GameLanguage.German:
                    return "Deutsch";
                case GameLanguage.French:
                    return "Français";
                case GameLanguage.Spanish:
                    return "Español";
                case GameLanguage.Italian:
                    return "Italiano";
                case GameLanguage.Polish:
                    return "Polski";
                case GameLanguage.Korean:
                    return "한국어 [韓國語]";
                case GameLanguage.EnglishAsianWesternFont:
                    return "English (Asian Western Font)";
                case GameLanguage.Swedish:
                    return "Svenska";
                case GameLanguage.Danish:
                    return "Dansk";
                case GameLanguage.Norwegian:
                    return "Norsk";
                case GameLanguage.Hungarian:
                    return "Magyar";
                case GameLanguage.Thai:
                    return "ภาษาไทย";
                case GameLanguage.Czech:
                    return "čeština";

                default:
                    throw new InvalidOperationException($"Unable to convert language, {gameLanguage} is not implemented.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}