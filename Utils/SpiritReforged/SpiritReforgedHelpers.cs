using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AlternativeCompat.Utils.SpiritReforged
{
    [JITWhenModsEnabled("SpiritReforged")]
    public class SpiritReforgedHelpers : ModSystem
    {
        // Because these are internal in spirit Reforged, the functions need to be copied over here
        #region Pots
        public static float CalculateCoinValue()
        {
            float value = 200 + WorldGen.genRand.Next(-100, 101);
            value *= 1f + Main.rand.Next(-20, 21) * 0.01f;

            if (Main.hardMode)
                value *= 2;

            if (Main.rand.NextBool(4))
                value *= 1f + Main.rand.Next(5, 11) * 0.01f;

            if (Main.rand.NextBool(8))
                value *= 1f + Main.rand.Next(10, 21) * 0.01f;

            if (Main.rand.NextBool(12))
                value *= 1f + Main.rand.Next(20, 41) * 0.01f;

            if (Main.rand.NextBool(16))
                value *= 1f + Main.rand.Next(40, 81) * 0.01f;

            if (Main.rand.NextBool(20))
                value *= 1f + Main.rand.Next(50, 101) * 0.01f;

            if (Main.expertMode)
                value *= 2.5f;

            if (Main.expertMode && Main.rand.NextBool(2))
                value *= 1.25f;

            if (Main.expertMode && Main.rand.NextBool(3))
                value *= 1.5f;

            if (Main.expertMode && Main.rand.NextBool(4))
                value *= 1.75f;

            if (NPC.downedBoss1)
                value *= 1.1f;

            if (NPC.downedBoss2)
                value *= 1.1f;

            if (NPC.downedBoss3)
                value *= 1.1f;

            if (NPC.downedMechBoss1)
                value *= 1.1f;

            if (NPC.downedMechBoss2)
                value *= 1.1f;

            if (NPC.downedMechBoss3)
                value *= 1.1f;

            if (NPC.downedPlantBoss)
                value *= 1.1f;

            if (NPC.downedQueenBee)
                value *= 1.1f;

            if (NPC.downedGolemBoss)
                value *= 1.1f;

            if (NPC.downedPirates)
                value *= 1.1f;

            if (NPC.downedGoblins)
                value *= 1.1f;

            if (NPC.downedFrost)
                value *= 1.1f;

            return value;
        }

        /*public bool KillMultiTile(int i, int j, int frameX, int frameY, out EntitySource_TileBreak source, out Vector2 center)
        {
            source = new EntitySource_TileBreak(i, j);
            center = new Vector2(i, j).ToWorldCoordinates(16, 16);

            if (WorldGen.generatingWorld || PotTile.IsRubble) return false;

            source = new EntitySource_TileBreak(i, j);
            center = new Vector2(i, j).ToWorldCoordinates(16, 16);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                #region loot
                var p = Main.player[Player.FindClosest(center, 0, 0)];
                TileLootSystem.Resolve(i, j, this, frameX, frameY);

                ItemMethods.SplitCoins((int)(PotHelpers.CalculateCoinValue() * 2.5), delegate (int type, int stack)
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

            return true;
        };*/
        #endregion

        #region Trees
        public static Vector2 GetPalmTreeOffset(int i, int j) => new(Framing.GetTileSafely(i, j).TileFrameY - 2, 0);
        #endregion
    }
}
