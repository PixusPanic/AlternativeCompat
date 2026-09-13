using Microsoft.Xna.Framework;
using SpiritReforged.Common.ItemCommon;
using SpiritReforged.Common.Misc;
using SpiritReforged.Common.TileCommon;
using SpiritReforged.Common.UI.PotCatalogue;
using SpiritReforged.Content.Underground.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AlternativeCompat.Utils.SpiritReforged
{
    [ExtendsFromMod(AlternativeCompat.spirit)]
    public abstract class PotTileBase : BiomePots, ILootable
    {
        public virtual string TileRecord => "Mods.AlternativeCompat.Tiles.SpiritReforged.Records";

        public override Dictionary<string, int[]> TileStyles => new() { { string.Empty, [0, 1, 2] } };

        public override TileRecord AddRecord(int type, NamedStyles.StyleGroup group)
        {
            var record = new TileRecord(group.name, type, group.styles);
            return record.AddRating(2).AddDescription(Language.GetText(TileRecord + ".Biome"));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (WorldGen.generatingWorld || IsRubble) return;

            var source = new EntitySource_TileBreak(i, j);
            var center = new Vector2(i, j).ToWorldCoordinates(16, 16);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                #region loot
                var p = Main.player[Player.FindClosest(center, 0, 0)];
                TileLootSystem.Resolve(i, j, Type, frameX, frameY);

                ItemMethods.SplitCoins((int)(SpiritReforgedHelpers.CalculateCoinValue() * 2.5), delegate (int type, int stack)
                {
                    Item.NewItem(source, center, new Item(type, stack), noGrabDelay: true);
                }); //Always drop coins

                if (p.statLife < p.statLifeMax2)
                {
                    int stack = Main.rand.Next(3, 6);

                    for (int h = 0; h < stack; h++)
                        Item.NewItem(source, center, ItemID.Heart);
                }

                if (Main.rand.NextBool(100))
                    Projectile.NewProjectile(source, center, Vector2.UnitY * -4f, ProjectileID.CoinPortal, 0, 0);
                #endregion
            }
        }

        public override void AddMapData() => AddMapEntry(new Color(112, 60, 70), Language.GetText("MapObject.Pot"));

        public override void NearbyEffects(int i, int j, bool closer)
        {
            const int distance = 200;

            if (!closer || IsRubble)
                return;

            var world = new Vector2(i, j) * 16;
            float strength = Main.LocalPlayer.DistanceSQ(world) / (distance * distance);

            if (strength < 1 && Main.rand.NextFloat(35f) < 1f - strength)
            {
                var d = Dust.NewDustDirect(world, 16, 16, DustID.TreasureSparkle, 0, 0, Scale: Main.rand.NextFloat());
                d.noGravity = true;
                d.velocity = new Vector2(0, -Main.rand.NextFloat(2f));
                d.fadeIn = 1f;
            }
        }
    }
}
