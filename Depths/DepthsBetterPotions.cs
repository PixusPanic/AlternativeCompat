using BetterPotions.Common.Configs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Buffs;
using TheDepths.Dusts;
using TheDepths.Items;

namespace AlternativeCompat.Depths
{

    [JITWhenModsEnabled(AlternativeCompat.depths)]
    public class DepthsBPGlobalItem : GlobalItem
    {
        [JITWhenModsEnabled("BetterPotions")]
        private static bool ObsidianSkinBuff => ModContent.GetInstance<BetterPotionsConfig>().PotionChanges_ObsidianSkin;

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("BetterPotions") && ObsidianSkinBuff;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) =>
            entity.type == ModContent.ItemType<CrystalSkinPotion>();

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.FirstOrDefault(tip => tip.Name == "Tooltip0");
            if (line is null) return;

            line.Text = Language.GetTextValue("Mods.AlternativeCompat.Depths.BPCrystalSkinPotion");
        }
    }

    [JITWhenModsEnabled(AlternativeCompat.depths)]
    public class DepthsBPModPlayer : ModPlayer
    {
        [JITWhenModsEnabled("BetterPotions")]
        private static bool ObsidianSkinBuff => ModContent.GetInstance<BetterPotionsConfig>().PotionChanges_ObsidianSkin;

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("BetterPotions") && ObsidianSkinBuff;

        public override void PostUpdateEquips()
        {
            if (Player.HasBuff(ModContent.BuffType<CrystalSkin>()))
            {
                Player.endurance += 0.03f;

                foreach (Projectile proj in Main.projectile)
                {
                    if (Main.rand.NextBool(10) && proj.active && proj.hostile &&
                        (Vector2.Distance(Player.Center, proj.Center) < 5f || proj.Hitbox.Intersects(Player.Hitbox)))
                    {
                        proj.velocity *= -1f;

                        proj.hostile = false;
                        proj.friendly = true;

                        if (proj.damage > 0)
                        {
                            Dust.NewDust(proj.position, proj.width, proj.height, ModContent.DustType<MercurySparkleDust>());
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item150, Player.position);
                        }
                    }
                    /*else if (proj.active && proj.friendly &&(Vector2.Distance(Player.Center, proj.Center) < 5f
                        || proj.Hitbox.Intersects(Player.Hitbox)))
                    {
                        proj.velocity *= -1f;

                        Dust.NewDust(proj.position, proj.width, proj.height, ModContent.DustType<MercurySparkleDust>());
                        //Terraria.Audio.SoundEngine.PlaySound(SoundID.Item150, proj.position);
                    }*/
                }
            }
        }
    }

    [JITWhenModsEnabled(AlternativeCompat.depths)]
    public class DepthsBPGlobalBuff : GlobalBuff
    {
        [JITWhenModsEnabled("BetterPotions")]
        private static bool ObsidianSkinBuff => ModContent.GetInstance<BetterPotionsConfig>().PotionChanges_ObsidianSkin;

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("BetterPotions") && ObsidianSkinBuff;

        public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
        {
            if (type != ModContent.BuffType<CrystalSkin>()) return;

            tip = Language.GetTextValue("Mods.AlternativeCompat.Depths.BPBuffCrystalSkin");
        }
    }
}
