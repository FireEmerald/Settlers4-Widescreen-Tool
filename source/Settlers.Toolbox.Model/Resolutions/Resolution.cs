namespace Settlers.Toolbox.Model.Resolutions
{
    public class Resolution
    {
        public GameResolution Id { get; }

        public ushort Width { get; }
        public ushort Height { get; }

        public Resolution(GameResolution id, ushort width, ushort height)
        {
            Id = id;

            Width = width;
            Height = height;
        }
    }
}