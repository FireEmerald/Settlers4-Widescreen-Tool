using System;
using System.Windows.Markup;

namespace Settlers.Toolbox.Views.Converter
{
    public class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}