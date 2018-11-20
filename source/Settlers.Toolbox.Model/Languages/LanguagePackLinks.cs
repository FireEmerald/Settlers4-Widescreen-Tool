using System.Collections.Generic;

namespace Settlers.Toolbox.Model.Languages
{
    public class LanguagePackLinks
    {
        public IReadOnlyList<LanguagePackLink> Languages { get; }

        public LanguagePackLinks(IReadOnlyList<LanguagePackLink> languages)
        {
            Languages = languages;
        }
    }
}