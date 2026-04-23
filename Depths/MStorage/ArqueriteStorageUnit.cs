using AlternativeCompat.Confection.MStorage;
using MagicStorage.Components;
using MagicStorage.CrossMod.Storage;
using MagicStorage.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Items.Placeable;

namespace AlternativeCompat.Depths.MStorage
{
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
    internal class ArqueriteStorageUnitTier : StorageUnitTier
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.depths);

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public override int UpgradeItemType => ModContent.ItemType<ArqueriteUpgrade>();

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public override int CoreItemType => ModContent.ItemType<ArqueriteCore>();

        // You can find the capacity of the base Magic Storage tiers at:  MagicStorage/CrossMod/Default/StorageUnitTiers.cs
        public override int Capacity => 120;

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public override int StorageUnitItemType => ModContent.ItemType<ArqueriteStorageUnitItem>();

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public override int StorageUnitTileType => ModContent.TileType<ArqueriteStorageUnitTile>();

        public override void SetStaticDefaults()
        {
            // Set the upgrade paths for this tier in SetStaticDefaults
            SetUpgradeableFrom(Demonite);
            SetUpgradeableFrom(Crimtane);
            SetUpgradeableTo(Hallowed);
            if (ModLoader.HasMod(AlternativeCompat.confection)) Neapolinite();
        }

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private void Neapolinite() => SetUpgradeableTo(ModContent.GetInstance<NeapoliniteStorageUnitTier>());

        public override void Frame(StorageUnitFullness fullness, bool active, out int frameX, out int frameY)
        {
            // Based on the ArqueriteStorageUnitTile.png spritesheet:
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
            // Based on the ArqueriteStorageUnitTile.png spritesheet:
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
    internal class ArqueriteStorageUnitTile : MagicStorage.Components.StorageUnit
    {
        public override string LocalizationCategory => "Depths.Tiles.MagicStorage";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.depths);

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
    internal class ArqueriteStorageUnitItem : BaseStorageUnitItem
    {
        public override string LocalizationCategory => "Depths.Items.MagicStorage";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.depths);

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public override StorageUnitTier Tier => ModContent.GetInstance<ArqueriteStorageUnitTier>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;  // This rarity will be automatically applied to the ExampleUpgrade and ExampleCore items as well
        }
    }

    [ExtendsFromMod("MagicStorage")]
    internal class ArqueriteUpgrade : BaseStorageUpgradeItem
    {
        public override string LocalizationCategory => "Depths.Items.MagicStorage";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.depths);
        
        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public override StorageUnitTier Tier => ModContent.GetInstance<ArqueriteStorageUnitTier>();

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ArqueriteBar>(), 10)
                .AddIngredient(ItemID.Topaz)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    [ExtendsFromMod("MagicStorage")]
    internal class ArqueriteCore : BaseStorageCore
    {
        public override string LocalizationCategory => "Depths.Items.MagicStorage";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.depths);

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public override StorageUnitTier Tier => ModContent.GetInstance<ArqueriteStorageUnitTier>();


        // The tooltip will default to the localization at:  Mods.MagicStorage.Items.StorageCore.CommonTooltip
        // If you want to customize the tooltip, you can override it here
        public override LocalizedText Tooltip => base.Tooltip;
    }
}
