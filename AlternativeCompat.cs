using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AlternativeCompat
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class AlternativeCompat : Mod
	{
        public const string avalon = "Avalon";
        public const string depths = "TheDepths";
		public const string confection = "TheConfectionRebirth";
    }

    public class AltCompatClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool MStorageWarning { get; set; }
    }
}
