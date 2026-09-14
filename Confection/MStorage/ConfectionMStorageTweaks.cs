using Avalon.NPCs.Bosses.PreHardmode;
using MagicStorage.Items;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AlternativeCompat.Confection.MStorage
{
    public class ConfectionMStorageTweaks : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.mStorage) && ModLoader.HasMod(AlternativeCompat.confection);

        [JITWhenModsEnabled(AlternativeCompat.mStorage)]
        private int HallowedUpgrade => ModContent.ItemType<UpgradeHallowed>();

        public override void PostSetupContent()
        {
            if (ModContent.GetInstance<AltCompatConfig>().RequireAltMaterials && HallowedUpgrade > -1
                && ModLoader.TryGetMod(AlternativeCompat.confection, out var confection))
                confection.Call("HallowedBarOnlyItem", HallowedUpgrade, true);
        }
    }

    public class BlueChlorophyteUpgradeTweak : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<AltCompatConfig>().RequireAltMaterials &&
            ModLoader.HasMod(AlternativeCompat.mStorage) && ModLoader.HasMod(AlternativeCompat.confection);

        [JITWhenModsEnabled(AlternativeCompat.mStorage)]
        private int BlueChlorophyteUpgrade => ModContent.ItemType<UpgradeBlueChlorophyte>();

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) =>
            entity.type == BlueChlorophyteUpgrade;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.FirstOrDefault(tip => tip.Name == "Tooltip1");
            if (line is null) return;

            line.Text = Language.GetTextValue("Mods.AlternativeCompat.Confection.MagicStorage.UpgradeBlueChlorophyte");
        }
    }
}
