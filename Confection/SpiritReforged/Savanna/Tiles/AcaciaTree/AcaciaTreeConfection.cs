using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.Grass;
using AlternativeCompat.Utils.SpiritReforged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritReforged.Common.TileCommon;
using SpiritReforged.Common.TileCommon.Conversion;
using SpiritReforged.Content.Savanna.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SpiritAcaciaTree = SpiritReforged.Content.Savanna.Tiles.AcaciaTree.AcaciaTree;

namespace AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.AcaciaTree
{
    [ExtendsFromMod(AlternativeCompat.spirit)]
    public class AcaciaTreeConfection : SpiritAcaciaTree
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        private readonly int TreeVariants = 3;

        public new ConversionHandler.Set ConversionSet => new()
        {
            { ModContent.TileType<SavannaGrassConfection>(), ModContent.TileType<AcaciaTreeConfection>() },
        };

        public override void PreAddObjectData()
        {
            base.PreAddObjectData();

            TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<SavannaGrassHallow>(), ModContent.TileType<SavannaGrassHallowMowed>()];
            TileID.Sets.Hallow[Type] = true;
        }

        protected override void OnGrowEffects(int i, int j, int height, int goreType) => base.OnGrowEffects(i, j, height, GoreID.TreeLeaf_Hallow);

        public override void DrawTreeFoliage(int i, int j, SpriteBatch spriteBatch)
        {
            if (!TileMethods.GetVisualInfo(i, j, out Color color, out Texture2D texture))
                return;

            var position = new Vector2(i, j) * 16 - Main.screenPosition + SpiritReforgedHelpers.GetPalmTreeOffset(i, j);
            float rotation = GetSway(i, j) * 0.08f;

            if (FindSegment(i, j) is SegmentType.LeafyTop) //Draw treetops
            {
                int frameX = i % TreeVariants;
                int frameY = Framing.GetTileSafely(i, j).TileFrameX / FrameSize % 2;

                Point size = new(330, 118);
                var source = new Rectangle((size.X + 2) * frameX, 186 + (size.Y + 2) * frameY, size.X, size.Y);
                var origin = new Vector2(source.Width / 2, source.Height);

                DrawShade(position, rotation);
                spriteBatch.Draw(texture, position + new Vector2(9, 3), source, color, rotation, origin, 1, SpriteEffects.None, 0);
            }
            else //Draw branches
            {
                int frameX = Random(i, j, 0, 2) + j % TreeVariants * 2;
                int frameY = Framing.GetTileSafely(i, j).TileFrameX / FrameSize % 3;

                bool flip = frameX % 2 == 0;
                Point size = new(32, 52);

                var source = new Rectangle((size.X + 2) * frameX, 23 + (size.Y + 2) * frameY, size.X, size.Y);
                var origin = new Vector2(flip ? source.Width : 0, 44);
                position += new Vector2(6 * (flip ? -1 : 1), 8); //Directional offset

                spriteBatch.Draw(texture, position + new Vector2(10, 0), source, color, rotation, origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
