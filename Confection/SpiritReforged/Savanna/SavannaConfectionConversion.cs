using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.AcaciaTree;
using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.Grass;
using SpiritReforged.Common.TileCommon.Conversion;
using SpiritReforged.Content.Savanna.Tiles;
using SpiritReforged.Content.Savanna.Tiles.AcaciaTree;
using System.Collections.Generic;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace AlternativeCompat.Confection.SpiritReforged.Savanna
{
    [ExtendsFromMod(AlternativeCompat.spirit)]
    public class SavannaConfectionConversion : ModSystem, ISetConversion
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        public int creamsand => ModContent.TileType<Creamsand>();

        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        static int[] foliage => [ModContent.TileType<SavannaFoliage>(), ModContent.TileType<SavannaFoliageCorrupt>(),
                ModContent.TileType<SavannaFoliageCrimson>(), ModContent.TileType<SavannaFoliageHallow>() ];
        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        static int[] elephant => [ModContent.TileType<ElephantGrass>(), ModContent.TileType<ElephantGrassCorrupt>(),
                ModContent.TileType<ElephantGrassCrimson>(), ModContent.TileType<ElephantGrassHallow>()];
        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        static int[] shrubs => [ModContent.TileType<SavannaShrubs>(), ModContent.TileType<SavannaShrubsCorrupt>(),
                ModContent.TileType<SavannaShrubsCrimson>(), ModContent.TileType<SavannaShrubsHallow>()];
        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        static int[] vines => [ModContent.TileType<SavannaVine>(), ModContent.TileType<SavannaVineCorrupt>(),
                ModContent.TileType<SavannaVineCrimson>(), ModContent.TileType<SavannaVineHallow>()];
        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        static int[] acacia => [ModContent.TileType<AcaciaTree>(), ModContent.TileType<AcaciaTreeCorrupt>(),
                ModContent.TileType<AcaciaTreeCrimson>(), ModContent.TileType<AcaciaTreeHallow>()];

        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        (int[] tile, int confection)[] SavannaTiles =>
            [
            (foliage, ModContent.TileType<SavannaFoliageConfection>()),
            (elephant, ModContent.TileType<ElephantGrassConfection>()),
            (shrubs, ModContent.TileType<SavannaShrubsConfection>()),
            (vines, ModContent.TileType<SavannaVineConfection>()),
            (acacia, ModContent.TileType<AcaciaTreeConfection>())
            ];

        public ConversionHandler.Set ConversionSet => new()
        {
            { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaFoliageConfection>() },
            { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<ElephantGrassConfection>() },
            { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaShrubsConfection>() },
            { creamsand, ModContent.TileType<SavannaShrubsConfection>() },
            { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<AcaciaTreeConfection>() },
        };

        /*public override void OnModLoad()
        {
            if (ModLoader.TryGetMod(AlternativeCompat.spirit, out var spirit))
                SpiritReforgedCompat(spirit);
        }

        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        private void SpiritReforgedCompat(Mod spirit)
        {
            foreach (var (tiles, confection) in SavannaTiles)
            {
                if (confection <= 0) continue;
                foreach (int tile in tiles)
                {
                    Debugging(confection, tile);
                    if (tile <= 0) continue;
                    spirit.Call("RegisterConversionSet", "Plants", new Dictionary<int, int>() { { tile, confection } });
                }
            }
        }*/

        public override void PostSetupContent()
        {
            if (!ModLoader.HasMod(AlternativeCompat.spirit)) return;
            SpiritReforgedConversion();
        }

        [JITWhenModsEnabled(AlternativeCompat.spirit, AlternativeCompat.confection)]
        private void SpiritReforgedConversion()
        {
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrass>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassMowed>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfectionMowed>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassCorrupt>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassCrimson>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassHallow>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            /*TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassHallowMowed>(), BiomeConversionID.Corruption, TileID.CorruptGrass);
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassHallowMowed>(), BiomeConversionID.Crimson, TileID.CrimsonGrass);
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassHallowMowed>(), BiomeConversionID.Purity, TileID.GolfGrass);*/

            foreach (var (tiles, confection) in SavannaTiles)
            {
                if (confection <= 0 || confection == ModContent.TileType<AcaciaTreeConfection>()) continue;
                foreach (int tile in tiles)
                {
                    Debugging(confection, tile);
                    // If the tile is invalid, continue to the next one and don't register a conversion
                    if (tile <= 0) continue;
                    TileLoader.RegisterConversion(tile, GetConfectionBiomeConversion.ConvID, confection);
                }
            }
        }

        private static void Debugging(int biome, int tile)
        {
            // If debug messages are turned off and the user isn't running a debug, go back to the original method
            if (!ModContent.GetInstance<AltCompatClient>().DebugMessages && !System.Diagnostics.Debugger.IsAttached) return;
            var biomeName = $"{biome} (INVALID)";
            if (ModContent.GetModTile(biome) != null) biomeName = $"{biome} ({ModContent.GetModTile(biome).Name})";
            var tileName = $"{tile} (INVALID)";
            if (ModContent.GetModTile(tile) != null) tileName = $"{tile} ({ModContent.GetModTile(tile).FullName})";
            ModContent.GetInstance<AlternativeCompat>().Logger.Info($"AltBiome tile: {biomeName}; tile: {tileName}");
        }
    }
}
