namespace Settlers.Toolbox.Infrastructure.ExtensionMethods
{
    public static class UnitConversionExtensions
    {
        public static double AsBytesToMegabytes(this long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}