
using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.AcaciaTree;
using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.Grass;
using SpiritReforged.Content.Savanna.Tiles;
using SpiritReforged.Content.Savanna.Tiles.AcaciaTree;
using Terraria.ModLoader;

namespace AlternativeCompat.Confection.SpiritReforged.Savanna
{
    public class SavannaConfectionConversion : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override void Load()
        {
            if (ModLoader.TryGetMod(AlternativeCompat.spirit, out var spirit))
                SpiritReforgedCompat(spirit);
        }

        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        private void SpiritReforgedCompat(Mod spirit)
        {
            spirit.Call("RegisterConversionSet", nameof(SavannaGrass), ModContent.TileType<SavannaGrassConfection>());
            spirit.Call("RegisterConversionSet", nameof(ElephantGrass), ModContent.TileType<ElephantGrassConfection>());
            spirit.Call("RegisterConversionSet", nameof(SavannaFoliage), ModContent.TileType<SavannaFoliageConfection>());
            spirit.Call("RegisterConversionSet", nameof(SavannaShrubs), ModContent.TileType<SavannaShrubsConfection>());
            spirit.Call("RegisterConversionSet", nameof(AcaciaTree), ModContent.TileType<AcaciaTreeConfection>());
        }

        public override void PostSetupContent()
        {
            if (!ModLoader.HasMod(AlternativeCompat.spirit)) return;
            SpiritReforgedConversion();
        }

        [JITWhenModsEnabled(AlternativeCompat.spirit)]
        private void SpiritReforgedConversion()
        {
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrass>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassMowed>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfectionMowed>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassCorrupt>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassCrimson>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassHallow>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrassHallowMowed>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfectionMowed>());

            TileLoader.RegisterConversion(ModContent.TileType<SavannaGrass>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaGrassConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<ElephantGrass>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<ElephantGrassConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaFoliage>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaFoliageConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<SavannaShrubs>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<SavannaShrubsConfection>());
            TileLoader.RegisterConversion(ModContent.TileType<AcaciaTree>(), GetConfectionBiomeConversion.ConvID, ModContent.TileType<AcaciaTreeConfection>());
        }
    }
}
