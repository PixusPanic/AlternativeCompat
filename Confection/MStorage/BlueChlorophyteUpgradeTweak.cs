using MagicStorage.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Items;

namespace AlternativeCompat.Confection.MStorage
{
    public class BlueChlorophyteUpgradeTweak : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("MagicStorage") && ModLoader.HasMod(AlternativeCompat.confection);

        [JITWhenModsEnabled("MagicStorage")]
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
