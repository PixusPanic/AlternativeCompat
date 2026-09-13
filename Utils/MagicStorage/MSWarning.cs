using AlternativeCompat.Confection.MStorage;
using AlternativeCompat.Contagion.MStorage;
using AlternativeCompat.Depths.MStorage;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AlternativeCompat.Utils.MagicStorage
{
    public class MSWarning : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.mStorage);

        #region Tile IDs and matching them
        #region Avalon
        [JITWhenModsEnabled(AlternativeCompat.avalon)]
        private static int BacciliteCore => ModContent.ItemType<BacciliteCore>();
        [JITWhenModsEnabled(AlternativeCompat.avalon)]
        private static int BacciliteStorage => ModContent.ItemType<BacciliteStorageUnitItem>();

        [JITWhenModsEnabled(AlternativeCompat.avalon)]
        private static bool Baccilite(Item entity) => entity.type == BacciliteCore || entity.type == BacciliteStorage;
        #endregion

        #region Depths
        [JITWhenModsEnabled(AlternativeCompat.depths)]
        private static int ArqueriteCore => ModContent.ItemType<ArqueriteCore>();
        [JITWhenModsEnabled(AlternativeCompat.depths)]
        private static int ArqueriteStorage => ModContent.ItemType<ArqueriteStorageUnitItem>();

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        private static bool Arquerite(Item entity) => entity.type == ArqueriteCore || entity.type == ArqueriteStorage;
        #endregion

        #region Confection
        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static int NeapoliniteCore => ModContent.ItemType<NeapoliniteCore>();
        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static int NeapoliniteStorage => ModContent.ItemType<NeapoliniteStorageUnitItem>();

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static bool Neapolinite(Item entity) => entity.type == NeapoliniteCore || entity.type == NeapoliniteStorage;
        #endregion
        #endregion

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return ModLoader.HasMod(AlternativeCompat.avalon) && Baccilite(entity) ||
                ModLoader.HasMod(AlternativeCompat.depths) && Arquerite(entity) ||
                ModLoader.HasMod(AlternativeCompat.confection) && Neapolinite(entity);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!ModContent.GetInstance<AltCompatClient>().MStorageWarning) return;
            /*TooltipLine text = tooltips.LastOrDefault();
            if (text is null) return;*/

            tooltips.Add(new TooltipLine(Mod, "MStorageWarning",
                Language.GetTextValue("Mods.AlternativeCompat.MagicStorage.Warning")));
        }
    }
}
