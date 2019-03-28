namespace Settlers.Toolbox.Interfaces
{
    public interface IWindowManager
    {
        bool ReadmeWindowIsOpen { get; }

        void OpenReadmeWindow();
    }
}