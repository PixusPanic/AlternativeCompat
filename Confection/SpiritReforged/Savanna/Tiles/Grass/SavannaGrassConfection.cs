using Microsoft.Xna.Framework;
using SpiritReforged.Common;
using SpiritReforged.Common.TileCommon;
using SpiritReforged.Content.Savanna.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;

namespace AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.Grass
{
    [ExtendsFromMod(AlternativeCompat.spirit, AlternativeCompat.confection)]
    public class SavannaGrassConfection : SavannaGrass
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        protected override Color MapColor => new(235, 207, 150);
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            SpiritSets.Mowable[Type] = ModContent.TileType<SavannaGrassConfectionMowed>();

            ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
            ConfectionIDs.Sets.Confection[Type] = true;
            ConfectionIDs.Sets.IsNaturalConfectionTile[Type] = true;

            TileMethods.Merge(Type, ModContent.TileType<SavannaGrass>(), ModContent.TileType<SavannaGrassCorrupt>(),
                ModContent.TileType<SavannaGrassHallow>(), ModContent.TileType<SavannaGrassCrimson>());
        }

        public override void RandomUpdate(int i, int j)
        {
            base.RandomUpdate(i, j);
            WorldGen.SpreadInfectionToNearbyTile(i, j, GetConfectionBiomeConversion.ConvID);
        }

        protected override void GrowTiles(int i, int j)
        {
            var above = Framing.GetTileSafely(i, j - 1);
            if (!above.HasTile && above.LiquidAmount < 80)
            {
                int grassChance = GrassAny() ? 6 : 30;

                if (Main.rand.NextBool(grassChance))
                    Placer.PlaceTile<ElephantGrassConfection>(i, j - 1, Main.rand.Next(5, 8)).Send();
                else if (Main.rand.NextBool(10))
                    Placer.PlaceTile<SavannaFoliageConfection>(i, j - 1).Send();

                if (Main.rand.NextBool(1400) && WorldGen.PlaceTile(i, j - 1, TileID.DyePlants, true, style: 2))
                    NetMessage.SendTileSquare(-1, i, j - 1, TileChangeType.None);
            }

            if (Main.rand.NextBool(5) && WorldGen.GrowMoreVines(i, j) && Main.tile[i, j + 1].LiquidType != LiquidID.Lava)
                Placer.GrowVine(i, j + 1, ModContent.TileType<SavannaVineConfection>());

            bool GrassAny()
            {
                int type = ModContent.TileType<ElephantGrassConfection>();
                return Framing.GetTileSafely(i - 1, j - 1).TileType == type || Framing.GetTileSafely(i + 1, j - 1).TileType == type;
            }
        }
    }

    [ExtendsFromMod(AlternativeCompat.spirit, AlternativeCompat.confection)]
    public class SavannaGrassConfectionMowed : SavannaGrassMowed
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        protected override Color MapColor => new(235, 207, 150);
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
            ConfectionIDs.Sets.Confection[Type] = true;
            ConfectionIDs.Sets.IsNaturalConfectionTile[Type] = true;

            TileMethods.Merge(Type, ModContent.TileType<SavannaGrass>(), ModContent.TileType<SavannaGrassCorrupt>(),
                ModContent.TileType<SavannaGrassHallow>(), ModContent.TileType<SavannaGrassCrimson>(), ModContent.TileType<SavannaGrassConfection>());
        }

        public override void RandomUpdate(int i, int j) => WorldGen.SpreadInfectionToNearbyTile(i, j, GetConfectionBiomeConversion.ConvID);
    }

    [ExtendsFromMod(AlternativeCompat.spirit, AlternativeCompat.confection)]
    public class SavannaFoliageConfection : SavannaFoliage
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);


        public override void PreAddObjectData()
        {
            //base.PreAddObjectData();
            TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaGrassConfectionMowed>()];

            ConfectionIDs.Sets.Confection[Type] = true;
            ConfectionIDs.Sets.IsNaturalConfectionTile[Type] = true;

            DustType = ModContent.DustType<CreamDust>();
            AddMapEntry(new(235, 207, 150));
        }
    }

    [ExtendsFromMod(AlternativeCompat.spirit, AlternativeCompat.confection)]
    public class SavannaShrubsConfection : SavannaShrubs
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override void PreAddObjectData()
        {
            //base.PreAddObjectData();
            TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaGrassConfectionMowed>(),
                SavannaConfectionConversion.creamsand];

            ConfectionIDs.Sets.Confection[Type] = true;
            ConfectionIDs.Sets.IsNaturalConfectionTile[Type] = true;

            DustType = ModContent.DustType<CreamDust>();
            AddMapEntry(new(235, 207, 150));
        }
    }

    [ExtendsFromMod(AlternativeCompat.spirit, AlternativeCompat.confection)]
    [DrawOrder(DrawOrderAttribute.Layer.NonSolid, DrawOrderAttribute.Layer.OverPlayers)]
    public class ElephantGrassConfection : ElephantGrass
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override void PreAddObjectData()
        {
            //base.PreAddObjectData();
            TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaGrassConfectionMowed>()];

            ConfectionIDs.Sets.Confection[Type] = true;
            ConfectionIDs.Sets.IsNaturalConfectionTile[Type] = true;

            ConfectionIDs.Sets.Confection[Type] = true;
            ConfectionIDs.Sets.IsNaturalConfectionTile[Type] = true;

            AddMapEntry(new(235, 207, 150));
            DustType = ModContent.DustType<CreamDust>();
        }
    }

    [ExtendsFromMod(AlternativeCompat.spirit, AlternativeCompat.confection)]
    public class SavannaVineConfection : SavannaVine
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override void PreAddObjectData()
        {
            //base.PreAddObjectData();
            TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<SavannaGrassConfectionMowed>()];

            ConfectionIDs.Sets.Confection[Type] = true;
            ConfectionIDs.Sets.IsNaturalConfectionTile[Type] = true;

            AddMapEntry(new(235, 207, 150));
            DustType = ModContent.DustType<CreamDust>();
        }
    }
}
