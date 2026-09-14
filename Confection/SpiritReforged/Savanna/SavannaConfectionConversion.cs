using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.AcaciaTree;
using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.Grass;
using SpiritReforged.Common.TileCommon.Conversion;
using SpiritReforged.Content.Savanna.Tiles;
using SpiritReforged.Content.Savanna.Tiles.AcaciaTree;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Tiles;

namespace AlternativeCompat.Confection.SpiritReforged.Savanna
{
    [ExtendsFromMod(AlternativeCompat.spirit)]
    public class SavannaConfectionConversion : ModSystem, ISetConversion
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        public static int creamsand => ModContent.TileType<Creamsand>();

        static int[] grass => [ModContent.TileType<SavannaGrass>(), ModContent.TileType<SavannaGrassCorrupt>(),
            ModContent.TileType<SavannaGrassCrimson>(), ModContent.TileType<SavannaGrassHallow>()];
        static int[] grassMowed => [ModContent.TileType<SavannaGrassMowed>(), ModContent.TileType<SavannaGrassHallowMowed>()]; 
        static int[] foliage => [ModContent.TileType<SavannaFoliage>(), ModContent.TileType<SavannaFoliageCorrupt>(),
                ModContent.TileType<SavannaFoliageCrimson>(), ModContent.TileType<SavannaFoliageHallow>() ];
        //[JITWhenModsEnabled(AlternativeCompat.spirit)]
        static int[] elephant => [ModContent.TileType<ElephantGrass>(), ModContent.TileType<ElephantGrassCorrupt>(),
                ModContent.TileType<ElephantGrassCrimson>(), ModContent.TileType<ElephantGrassHallow>()];
        //[JITWhenModsEnabled(AlternativeCompat.spirit)]
        static int[] shrubs => [ModContent.TileType<SavannaShrubs>(), ModContent.TileType<SavannaShrubsCorrupt>(),
                ModContent.TileType<SavannaShrubsCrimson>(), ModContent.TileType<SavannaShrubsHallow>()];
        //[JITWhenModsEnabled(AlternativeCompat.spirit)]
        static int[] vines => [ModContent.TileType<SavannaVine>(), ModContent.TileType<SavannaVineCorrupt>(),
                ModContent.TileType<SavannaVineCrimson>(), ModContent.TileType<SavannaVineHallow>()];
        //[JITWhenModsEnabled(AlternativeCompat.spirit)]
        static int[] acacia => [ModContent.TileType<AcaciaTree>(), ModContent.TileType<AcaciaTreeCorrupt>(),
                ModContent.TileType<AcaciaTreeCrimson>(), ModContent.TileType<AcaciaTreeHallow>()];

        //[JITWhenModsEnabled(AlternativeCompat.spirit)]
        (int[] tile, int confection, string name)[] SavannaTiles =>
            [
            //(grass, ModContent.TileType<SavannaGrassConfection>(), nameof(SavannaGrass)),
            //(grassMowed, ModContent.TileType<SavannaGrassConfectionMowed>(), nameof(SavannaGrass)),
            (foliage, ModContent.TileType<SavannaFoliageConfection>(),  nameof(SavannaFoliage)),
            (elephant, ModContent.TileType<ElephantGrassConfection>(),  nameof(ElephantGrass)),
            (shrubs, ModContent.TileType<SavannaShrubsConfection>(), nameof(SavannaShrubs)),
            //(vines, ModContent.TileType<SavannaVineConfection>(),  nameof(SavannaVine)),
            //(acacia, ModContent.TileType<AcaciaTreeConfection>(),  nameof(AcaciaTree))
            ];

        public ConversionHandler.Set ConversionSet => new()
        {
            { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaFoliageConfection>() },
            { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<ElephantGrassConfection>() },
            { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaShrubsConfection>() },
            { creamsand, ModContent.TileType<SavannaShrubsConfection>() },
            { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<AcaciaTreeConfection>() },

            { ModContent.TileType<SavannaGrassConfectionMowed>(), ModContent.TileType<SavannaFoliageConfection>() },
            { ModContent.TileType<SavannaGrassConfectionMowed>(), ModContent.TileType<ElephantGrassConfection>() },
            { ModContent.TileType<SavannaGrassConfectionMowed>(), ModContent.TileType<SavannaShrubsConfection>() },
            { ModContent.TileType<SavannaGrassConfectionMowed>(), ModContent.TileType<AcaciaTreeConfection>() }
        };

        public override void OnModLoad()
        {
            if (ModLoader.TryGetMod(AlternativeCompat.spirit, out var spirit))
                SpiritReforgedCall(spirit);
        }

        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        private void SpiritReforgedCall(Mod spirit)
        {
            spirit.Call("RegisterConversionSet", nameof(AcaciaTree), new Dictionary<int, int>()
            { { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<AcaciaTreeConfection>() } });
            spirit.Call("RegisterConversionSet", nameof(AcaciaTree), new Dictionary<int, int>()
            { { ModContent.TileType<SavannaGrassConfectionMowed>(), ModContent.TileType<AcaciaTreeConfection>() } });

            // I tried to automate this but I don't think my code worked - I'm too lazy to fix it right now
            /*foreach (var (tiles, confection, name) in SavannaTiles)
            {
                if (confection <= 0) continue;
                foreach (int tile in tiles)
                {
                    Debugging(confection, tile, name);
                    if (tile <= 0) continue;
                    spirit.Call("RegisterConversionSet", name, new Dictionary<int, int>() { { tile, confection } });
                }
            }*/

            spirit.Call("RegisterConversionSet", "Plants", new Dictionary<int, int>()
            { { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaFoliageConfection>() } });
            /*spirit.Call("RegisterConversionSet", "Plants", new Dictionary<int, int>()
            { { ModContent.TileType<SavannaGrassConfectionMowed>(), ModContent.TileType<SavannaFoliageConfection>() } });*/

            spirit.Call("RegisterConversionSet", nameof(ElephantGrass), new Dictionary<int, int>()
            { { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<ElephantGrassConfection>() } });
            spirit.Call("RegisterConversionSet", nameof(ElephantGrass), new Dictionary<int, int>()
            { { ModContent.TileType<SavannaGrassConfectionMowed>(), ModContent.TileType<ElephantGrassConfection>() } });

            spirit.Call("RegisterConversionSet", nameof(SavannaShrubs), new Dictionary<int, int>()
            { { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaShrubsConfection>() } });
            spirit.Call("RegisterConversionSet", nameof(SavannaShrubs), new Dictionary<int, int>()
            { { ModContent.TileType<SavannaGrassConfectionMowed>(), ModContent.TileType<SavannaShrubsConfection>() } });
            spirit.Call("RegisterConversionSet", nameof(SavannaShrubs), new Dictionary<int, int>()
            { { creamsand, ModContent.TileType<SavannaShrubsConfection>() } });
        }

        public override void PostSetupContent()
        {
            if (!ModLoader.HasMod(AlternativeCompat.spirit)) return;
            SpiritReforgedCompat();
        }

        [JITWhenModsEnabled(AlternativeCompat.spirit, AlternativeCompat.confection)]
        private void SpiritReforgedCompat()
        {
            foreach (var tile in grass)
                TileLoader.RegisterConversion(tile, GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            foreach (var tile in grassMowed)
                TileLoader.RegisterConversion(tile, GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfectionMowed>());
            foreach (var tile in foliage)
                TileLoader.RegisterConversion(tile, GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaFoliageConfection>());
            foreach (var tile in elephant)
                TileLoader.RegisterConversion(tile, GetConfectionBiomeConversion.ConvID, ModContent.TileType<ElephantGrassConfection>());
            foreach (var tile in shrubs)
                TileLoader.RegisterConversion(tile, GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaShrubsConfection>());

            /*foreach (var (tiles, confection, _) in SavannaTiles)
            {
                if (confection <= 0 || confection == ModContent.TileType<AcaciaTreeConfection>()) continue;
                foreach (int tile in tiles)
                {
                    Debugging(confection, tile);
                    // If the tile is invalid, continue to the next one and don't register a conversion
                    if (tile <= 0) continue;
                    TileLoader.RegisterConversion(tile, GetConfectionBiomeConversion.ConvID, confection);
                }
            }*/
            #region Termite mounds
            foreach (var termite in termite)
            {
                TileObjectData data = TileObjectData.GetTileData(termite, 0);
                if (data != null) AppendTile(data);
            }
            
            #endregion
        }

        static int[] termite => [ModContent.TileType<TermiteMoundLarge>(), ModContent.TileType<TermiteMoundMedium>(),
                ModContent.TileType<TermiteMoundSmall>()];

        private static void AppendTile(TileObjectData data)
        {
            // Add to the existing anchor list
            int[] oldTiles = data.AnchorValidTiles ?? [];

            data.AnchorValidTiles = [.. oldTiles,
                ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaGrassConfectionMowed>()];
        }

        private static void Debugging(int biome, int tile, string set = null)
        {
            // If debug messages are turned off and the user isn't running a debug, go back to the original method
            if (!ModContent.GetInstance<AltCompatClient>().DebugMessages && !System.Diagnostics.Debugger.IsAttached) return;
            var biomeName = $"{biome} (INVALID)";
            if (ModContent.GetModTile(biome) != null) biomeName = $"{biome} ({ModContent.GetModTile(biome).Name})";
            var tileName = $"{tile} (INVALID)";
            if (ModContent.GetModTile(tile) != null) tileName = $"{tile} ({ModContent.GetModTile(tile).FullName})";
            if (set != null)
                ModContent.GetInstance<AlternativeCompat>().Logger.Info($"AltBiome tile: {biomeName}; tile: {tileName}: set: {set}");
            else ModContent.GetInstance<AlternativeCompat>().Logger.Info($"AltBiome tile: {biomeName}; tile: {tileName}");
        }
    }
}
