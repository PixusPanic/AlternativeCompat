using MagicStorage.Components;
using MagicStorage.CrossMod.Storage;
using MagicStorage.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace AlternativeCompat.Confection.MStorage
{
    public class LoadNeapoliniteTier : ModSystem
    {
        private static bool MSAndConfection() => ModLoader.HasMod(AlternativeCompat.confection) && ModLoader.HasMod("MagicStorage");

        public static int upgrade = MSAndConfection() ? GetInstance(1) : -1;
        public static int core = MSAndConfection() ? GetInstance(2) : -1;
        public static int storageItem = MSAndConfection() ? GetInstance(3) : -1;
        public static int storageTile = MSAndConfection() ? GetInstance(4) : -1;

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        public static int neapoliniteBar => ModContent.ItemType<NeapoliniteBar>();

        [JITWhenModsEnabled("MagicStorage")]
        private static int GetInstance(int file)
        {
            return file switch
            {
                1 => ModContent.ItemType<NeapoliniteUpgrade>(),
                2 => ModContent.ItemType<NeapoliniteCore>(),
                3 => ModContent.ItemType<NeapoliniteStorageUnitItem>(),
                4 => ModContent.TileType<NeapoliniteStorageUnitTile>(),
                _ => -1,
            };
        }
    }

    // This file showcases how to make a basic tier for Storage Units
    // To implement a custom Storage Unit tier, you need several components:
    //   1. The StorageUnitTier which defines the tier's properties and upgrade paths
    //   2. A StorageUnit tile that uses the tier
    //   3. A BaseStorageUnitItem item which places the tile
    //   4. A BaseStorageUpgradeItem item that applies this tier to a placed Storage Unit of the previous tier
    //   5. A BaseStorageCoreItem item that is extracted when using a Storage Core Wrench
    // Most of this is handled automatically for you, and you just have to define the relations between the components and their tier
    // In this example, the Demonite and Crimtane tiers can be upgraded to the Example tier, which can then be upgraded to the Hellstone tier

    [ExtendsFromMod("MagicStorage")]
    internal class NeapoliniteStorageUnitTier : StorageUnitTier
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override int UpgradeItemType => LoadNeapoliniteTier.upgrade;

        public override int CoreItemType => LoadNeapoliniteTier.core;

        // You can find the capacity of the base Magic Storage tiers at:  MagicStorage/CrossMod/Default/StorageUnitTiers.cs
        public override int Capacity => 160;

        public override int StorageUnitItemType => LoadNeapoliniteTier.storageItem;

        public override int StorageUnitTileType => LoadNeapoliniteTier.storageTile;

        public override void SetStaticDefaults()
        {
            // Set the upgrade paths for this tier in SetStaticDefaults
            SetUpgradeableFrom(Hellstone);
            // SetUpgradableFrom(Arquerite) is set in ArqueriteStorageUnit
            SetUpgradeableTo(BlueChlorophyte);
        }

        public override void Frame(StorageUnitFullness fullness, bool active, out int frameX, out int frameY)
        {
            // Based on the NeapoliniteStorageUnitTile.png spritesheet:
            //   Style columns are 36 pixels wide total
            //   Active styles are in the first three columns
            //   Each fullness type takes up one style column
            //   There is only one row of styles
            const int STYLE_WIDTH = 36;
            const int FULLNESS_STYLES = 3;

            frameX = active ? 0 : FULLNESS_STYLES * STYLE_WIDTH;
            frameX += (int)fullness * STYLE_WIDTH;
            frameY = 0;
        }

        public override void GetState(int frameX, int frameY, out StorageUnitFullness fullness, out bool active)
        {
            // Based on the NeapoliniteStorageUnitTile.png spritesheet:
            //   Style columns are 36 pixels wide total
            //   Active styles are in the first three columns
            //   Each fullness type takes up one style column
            const int STYLE_WIDTH = 36;
            const int FULLNESS_STYLES = 3;

            active = frameX < FULLNESS_STYLES * STYLE_WIDTH;
            fullness = (StorageUnitFullness)(frameX % (FULLNESS_STYLES * STYLE_WIDTH) / STYLE_WIDTH);
        }

        public override bool IsValidTile(int frameX, int frameY)
        {
            // Since this example only has one row of styles, there's no check needed
            // However, if you were using a spritesheet with multiple tiers, you can check the framing here
            return true;
        }
    }

    [ExtendsFromMod("MagicStorage")]
    internal class NeapoliniteStorageUnitTile : MagicStorage.Components.StorageUnit
    {
        public override string LocalizationCategory => "Confection.Tiles.MagicStorage";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override void ModifyObjectData()
        {
            // By default, StorageUnit expects spritesheets to have 6 style columns per row
            // If you want to modify this behavior, update:
            //   TileObjectData.newTile.StyleHorizontal
            //   TileObjectData.newTile.StyleMultiplier
            //   TileObjectData.newTile.StyleWrapLimit
            base.ModifyObjectData();
        }

        public override TEStorageUnit GetTileEntity()
        {
            // By default, StorageUnit places a TEStorageUnit tile entity
            // Use this method to place a custom tile entity if needed
            return base.GetTileEntity();
        }

        protected override bool GetGlowmask(int x, int y, int type, int frameX, int frameY, out Asset<Texture2D> asset, out Color drawColor)
        {
            // By default, StorageUnit.GetGlowMask() uses Texture + "_Glow" for loading the glowmask texture
            // It will also emit a Color.White light blended with the world's lighting
            // Use this method to modify the glowmask behavior if needed
            return base.GetGlowmask(x, y, type, frameX, frameY, out asset, out drawColor);
        }
    }

    [ExtendsFromMod("MagicStorage")]
    internal class NeapoliniteStorageUnitItem : BaseStorageUnitItem
    {
        public override string LocalizationCategory => "Confection.Items.MagicStorage";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override StorageUnitTier Tier => ModLoader.HasMod(AlternativeCompat.confection) ? GetTier : StorageUnitTier.Basic;

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static StorageUnitTier GetTier => ModContent.GetInstance<NeapoliniteStorageUnitTier>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Pink;  // This rarity will be automatically applied to the ExampleUpgrade and ExampleCore items as well
        }
    }

    [ExtendsFromMod("MagicStorage")]
    internal class NeapoliniteUpgrade : BaseStorageUpgradeItem
    {
        public override string LocalizationCategory => "Confection.Items.MagicStorage";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override StorageUnitTier Tier => ModLoader.HasMod(AlternativeCompat.confection) ? GetTier : StorageUnitTier.Basic;

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static StorageUnitTier GetTier => ModContent.GetInstance<NeapoliniteStorageUnitTier>();

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!ModLoader.HasMod(AlternativeCompat.depths)) return;

            TooltipLine line = tooltips.Find(x => x.Name == "Tooltip0" && x.Mod == "Terraria");
            if (line == null) return;

            line.Text = Language.GetTextValue(this.GetLocalization("TooltipDepths").Value);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(LoadNeapoliniteTier.neapoliniteBar, 10)
                .AddIngredient(ItemID.SoulofFright)
                .AddIngredient(ItemID.SoulofMight)
                .AddIngredient(ItemID.SoulofSight)
                .AddIngredient(ItemID.Sapphire)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    [ExtendsFromMod("MagicStorage")]
    internal class NeapoliniteCore : BaseStorageCore
    {
        public override string LocalizationCategory => "Confection.Items.MagicStorage";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override StorageUnitTier Tier => ModLoader.HasMod(AlternativeCompat.confection) ? GetTier : StorageUnitTier.Basic;

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static StorageUnitTier GetTier => ModContent.GetInstance<NeapoliniteStorageUnitTier>();

        // The tooltip will default to the localization at:  Mods.MagicStorage.Items.StorageCore.CommonTooltip
        // If you want to customize the tooltip, you can override it here
        public override LocalizedText Tooltip => base.Tooltip;
    }
}
