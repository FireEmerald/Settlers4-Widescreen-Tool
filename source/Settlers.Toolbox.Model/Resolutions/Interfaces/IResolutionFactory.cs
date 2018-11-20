using System.Collections.Generic;

namespace Settlers.Toolbox.Model.Resolutions.Interfaces
{
    public interface IResolutionFactory
    {
        Resolution Create(GameResolution gameResolution);
        Resolution CreateCustom(ushort width, ushort height);

        IReadOnlyList<Resolution> CreateAllPresets();
    }
}