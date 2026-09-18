using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.Grass;
using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;
using CookieBlock = TheConfectionRebirth.Tiles.CookieBlock;
using CreamBlock = TheConfectionRebirth.Tiles.CreamBlock;
using Creamsand = TheConfectionRebirth.Tiles.Creamsand;
using Creamsandstone = TheConfectionRebirth.Tiles.Creamsandstone;
using Creamstone = TheConfectionRebirth.Tiles.Creamstone;
using HardenedCreamsand = TheConfectionRebirth.Tiles.HardenedCreamsand;

namespace AlternativeCompat.Confection
{
    [JITWhenModsEnabled(AlternativeCompat.confection)]
    public class ConfectionCheckTileCount : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public static bool InConfection(Player player) => CheckIfTileCountisNull() >= 125 &&
            (player.ZoneOverworldHeight || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneSkyHeight);

        private static int CheckIfTileCountisNull()
        {
            if (ModContent.GetInstance<ConfectionCheckTileCount>() != null)
                return ModContent.GetInstance<ConfectionCheckTileCount>().confectionBlockCount;
            return 0;
        }

        #region Tile count
        public int confectionBlockCount;

        public static int ConfectionTileMax = 600;

        public int snowpylonConfectionCount;
        public int desertpylonConfectionCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            ConfectionTiles(tileCounts);
        }

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private void ConfectionTiles(ReadOnlySpan<int> tileCounts)
        {
            snowpylonConfectionCount = tileCounts[ModContent.TileType<CreamBlock>()]
                + tileCounts[ModContent.TileType<BlueIce>()];

            desertpylonConfectionCount = tileCounts[ModContent.TileType<Creamsand>()]
                + tileCounts[ModContent.TileType<Creamsandstone>()]
                + tileCounts[ModContent.TileType<HardenedCreamsand>()];

            confectionBlockCount = tileCounts[ModContent.TileType<CookieBlock>()]
                + tileCounts[ModContent.TileType<Creamstone>()]
                + tileCounts[ModContent.TileType<CreamGrass>()]
                + tileCounts[ModContent.TileType<CreamBlock>()]
                + tileCounts[ModContent.TileType<Creamsand>()]
                + tileCounts[ModContent.TileType<HardenedCreamsand>()]
                + tileCounts[ModContent.TileType<Creamsandstone>()];
            if (ModLoader.HasMod(AlternativeCompat.spirit))
                SpiritSavanna(confectionBlockCount, tileCounts);

            Main.SceneMetrics.EvilTileCount -= confectionBlockCount;
            if (Main.SceneMetrics.EvilTileCount < 0)
                Main.SceneMetrics.EvilTileCount = 0;
            Main.SceneMetrics.BloodTileCount -= confectionBlockCount;
            if (Main.SceneMetrics.BloodTileCount < 0)
                Main.SceneMetrics.BloodTileCount = 0;

            Main.SceneMetrics.SandTileCount += tileCounts[ModContent.TileType<Creamsand>()]
                + tileCounts[ModContent.TileType<HardenedCreamsand>()]
                + tileCounts[ModContent.TileType<Creamsandstone>()];
        }

        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        private static int SpiritSavanna(int confectionBlockCount, ReadOnlySpan<int> tileCounts)
        {
            return confectionBlockCount += tileCounts[ModContent.TileType<SavannaGrassConfection>()];
        }
        #endregion
    }
}
