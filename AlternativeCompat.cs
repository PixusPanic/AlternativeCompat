using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AlternativeCompat
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class AlternativeCompat : Mod
	{
        #region Alternative Biome mods
        public const string avalon = "Avalon";
        public const string depths = "TheDepths";
		public const string confection = "TheConfectionRebirth";
        #endregion

        #region Content mods
        public const string spirit = "SpiritReforged";
        public const string mStorage = "MagicStorage";
        #endregion

        #region Misc mods
        public const string liquidLib = "ModLiquidLib";
        public const string altLib = "AltLibrary";
        #endregion
    }

    public class AltCompatConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool RequireAltMaterials { get; set; }
    }

    public class AltCompatClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool MStorageWarning { get; set; }

        [DefaultValue(false)]
        public bool DebugMessages { get; set; }
    }
}
