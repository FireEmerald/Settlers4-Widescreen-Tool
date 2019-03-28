namespace Settlers.Toolbox.Infrastructure.IO.Interfaces
{
    public interface IIniFileAdapter
    {
        string ReadValueFromFile(string section, string key, string file, string defaultValue = "");
        bool WriteValueToFile(string section, string key, string value, string file);
    }
}