using AlternativeCompat.Utils.SpiritReforged;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using SpiritReforged.Common.ItemCommon;
using SpiritReforged.Common.Misc;
using SpiritReforged.Common.TileCommon;
using SpiritReforged.Common.UI.PotCatalogue;
using SpiritReforged.Common.WorldGeneration;
using SpiritReforged.Content.Forest.Cloud.Items;
using SpiritReforged.Content.Underground.Items;
using SpiritReforged.Content.Underground.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AlternativeCompat.Depths.SpiritReforged
{
    [ExtendsFromMod(AlternativeCompat.spirit)]
    public class DepthsPots : PotTileBase
    {
        public override string TileRecord => "Mods.AlternativeCompat.TheDepths.Tiles.SpiritReforged.Records";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.depths);

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);
            var source = new EntitySource_TileBreak(i, j);
            var center = new Vector2(i, j).ToWorldCoordinates(16, 16);

            if (ModContent.TryFind(AlternativeCompat.depths, "DepthsPotGore1", out ModGore potGore1))
                Gore.NewGore(source, GetRandom(), Vector2.Zero, potGore1.Type);
            if (ModContent.TryFind(AlternativeCompat.depths, "DepthsPotGore2", out ModGore potGore2))
                Gore.NewGore(source, GetRandom(), Vector2.Zero, potGore2.Type);
            if (ModContent.TryFind(AlternativeCompat.depths, "DepthsPotGore3", out ModDust potGore3))
                Gore.NewGore(source, GetRandom(), Vector2.Zero, potGore3.Type);

            if (ModContent.TryFind(AlternativeCompat.depths, "QuartzDust", out ModDust quartz))
                for (int d = 0; d < 20; d++)
                    Dust.NewDustPerfect(GetRandom(), quartz.Type, Main.rand.NextVector2Unit(), Scale: Main.rand.NextFloat());

            Vector2 GetRandom(float distance = 15f) => center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(distance);
        }

        public override bool KillSound(int i, int j, bool fail)
        {
            if (fail || IsRubble) return true;

            var pos = new Vector2(i, j).ToWorldCoordinates(16, 16);

            SoundEngine.PlaySound(SoundID.Shatter, pos);
            SoundEngine.PlaySound(Break, pos);

            return false;
        }

        /*public void AddLoot(ILoot loot)
        {
            List<int> potions = [ItemID.SpelunkerPotion, ItemID.HunterPotion,
            ItemID.GravitationPotion, ItemID.LifeforcePotion, ItemID.TitanPotion, ItemID.BattlePotion,
            ItemID.MagicPowerPotion, ItemID.ManaRegenerationPotion, ItemID.BiomeSightPotion, ItemID.HeartreachPotion,
            ModContent.ItemType<DoubleJumpPotion>(), WorldGen.crimson ? ItemID.RagePotion : ItemID.WrathPotion];

            if (ModContent.TryFind(AlternativeCompat.depths, "SilverSpherePotion", out ModItem silverSphere))
                potions.Add(silverSphere.Type);
            if (ModContent.TryFind(AlternativeCompat.depths, "CrystalSkinPotion", out ModItem crystalSkin))
                potions.Add(crystalSkin.Type);

            var pCond0 = ItemDropRule.OneFromOptions(15, [.. potions]);
            var pCond1 = ItemDropRule.OneFromOptions(3, ItemID.PotionOfReturn, ItemID.LuckPotionLesser);

            pCond0.OnSuccess(pCond1);
            pCond1.OnFailedRoll(ItemDropRule.Common(ItemID.LuckPotion, 5));

            loot.Add(pCond0);

            loot.Add(ItemDropRule.ByCondition(new DropConditions.Standard(Condition.Multiplayer), ItemID.WormholePotion, 30));
            loot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<PrefixVoucher>(), 30, 25));

            if (ModContent.TryFind(AlternativeCompat.depths, "CrystalSkinPotion", out ModItem smokeBlock))
                loot.AddCommon(smokeBlock.Type, 2, 10, 15);
        }*/
    }
}
