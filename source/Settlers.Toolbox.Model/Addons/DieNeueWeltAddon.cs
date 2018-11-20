using System;
using System.Collections.Generic;

using Settlers.Toolbox.Infrastructure;
using Settlers.Toolbox.Infrastructure.Cabinet.Interfaces;
using Settlers.Toolbox.Infrastructure.Registry.Interfaces;

namespace Settlers.Toolbox.Model.Addons
{
    public class DieNeueWeltAddon : AddonBase
    {
        private const string ADDON_REGKEY = @"SOFTWARE\BlueByte\Settlers4\Extra3";
        private const string CDVERSION_VALUE_NAME = "CDVersion";

        private readonly IRegistryAdapter _RegistryAdapter;

        public override GameAddon Id { get; }

        public override IDictionary<string, string> FileMappings { get; }
        
        public DieNeueWeltAddon(ICabContainerFactory cabContainerFactory, IRegistryAdapter registryAdapter) : base(cabContainerFactory)
        {
            if (registryAdapter == null) throw new ArgumentNullException(nameof(registryAdapter));

            _RegistryAdapter = registryAdapter;
            
            Id = GameAddon.DieNeueWelt;

            FileMappings = new Dictionary<string, string>
            {
                // Game Menu and Graphics
                { "40.gfx", @"Gfx\40.gfx" },
                { "41.gh5", @"Gfx\41.gh5" },
                { "41.gh6", @"Gfx\41.gh6" },
                { "41.gl5", @"Gfx\41.gl5" },
                { "41.gl6", @"Gfx\41.gl6" },

                // Maps
                { "MCD2_maya1.map",   @"Map\Campaign\MCD2_maya1.map" },
                { "MCD2_maya2.map",   @"Map\Campaign\MCD2_maya2.map" },
                { "MCD2_maya3.map",   @"Map\Campaign\MCD2_maya3.map" },
                { "MCD2_maya4.map",   @"Map\Campaign\MCD2_maya4.map" },
                { "MCD2_maya5.map",   @"Map\Campaign\MCD2_maya5.map" },
                { "MCD2_roman1.map",  @"Map\Campaign\MCD2_roman1.map" },
                { "MCD2_roman2.map",  @"Map\Campaign\MCD2_roman2.map" },
                { "MCD2_roman3.map",  @"Map\Campaign\MCD2_roman3.map" },
                { "MCD2_roman4.map",  @"Map\Campaign\MCD2_roman4.map" },
                { "MCD2_roman5.map",  @"Map\Campaign\MCD2_roman5.map" },
                { "MCD2_trojan1.map", @"Map\Campaign\MCD2_trojan1.map" },
                { "MCD2_trojan2.map", @"Map\Campaign\MCD2_trojan2.map" },
                { "MCD2_trojan3.map", @"Map\Campaign\MCD2_trojan3.map" },
                { "MCD2_trojan4.map", @"Map\Campaign\MCD2_trojan4.map" },
                { "MCD2_trojan5.map", @"Map\Campaign\MCD2_trojan5.map" },
                { "MCD2_viking1.map", @"Map\Campaign\MCD2_viking1.map" },
                { "MCD2_viking2.map", @"Map\Campaign\MCD2_viking2.map" },
                { "MCD2_viking3.map", @"Map\Campaign\MCD2_viking3.map" },
                { "MCD2_viking4.map", @"Map\Campaign\MCD2_viking4.map" },
                { "MCD2_viking5.map", @"Map\Campaign\MCD2_viking5.map" },
            };
        }

        // Works without special privileges
        protected override bool UnlockRegistryKeysExist()
        {
            IRegistryKeyAdapter hklm = _RegistryAdapter.LocalMachine();

            IRegistryKeyAdapter addonKey = hklm.OpenSubKey(ADDON_REGKEY, false);

            if (addonKey == null)
                return false;

            object cdVersionValue = addonKey.GetValue(CDVERSION_VALUE_NAME);

            return cdVersionValue != null;
        }

        // Requires administrator privileges
        protected override Result CreateUnlockRegistryKeys()
        {
            return new Result(); // TODO: FireEmerald: REMOVE

            IRegistryKeyAdapter hklm = _RegistryAdapter.LocalMachine();

            IRegistryKeyAdapter addonKey = hklm.CreateSubKey(ADDON_REGKEY);

            if (addonKey == null)
                return new Result($"Unable to create registry key '{hklm.Name}\\{ADDON_REGKEY}'.");

            addonKey.SetValue(CDVERSION_VALUE_NAME, 1508);
            return new Result();
        }
    }
}