namespace Settlers.Toolbox.Model.Languages
{
    public class LanguagePackLink
    {
        public int Id { get; }
        public string Url { get; }
        public string Sha1 { get; }

        public LanguagePackLink(int id, string url, string sha1)
        {
            Id = id;
            Url = url;
            Sha1 = sha1;
        }
    }
}