
using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.AcaciaTree;
using AlternativeCompat.Confection.SpiritReforged.Savanna.Tiles.Grass;
using AlternativeCompat.Utils;
using SpiritReforged.Common.Misc;
using SpiritReforged.Content.Savanna.Biome;
using SpiritReforged.Content.Savanna.Tiles;
using SpiritReforged.Content.Savanna.Tiles.AcaciaTree;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Tiles;

namespace AlternativeCompat.Confection.SpiritReforged.Savanna.Biome
{
    /*[JITWhenModsEnabled(AlternativeCompat.spirit)]
    public class SavannaConfectionBiome : ModBiome
    {
        public override string LocalizationCategory => "AlternativeCompat.Confection.Biomes.SpiritReforged.Savanna";

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override string BestiaryIcon => "AlternativeCompat.Confection.SpiritReforged.Savanna.Biome.SavannaConfectionBiome_Icon";

        public override string BackgroundPath => "AlternativeCompat.Confection.SpiritReforged.Savanna.Biome.SavannaConfectionMapBackground";

        public override string MapBackground => BackgroundPath;
    }*/
    [JITWhenModsEnabled(AlternativeCompat.spirit)]
    public class SavannaConfectionScene : ModSceneEffect
    {
        //public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        private int SelectMusic()
        {
            if (Main.LocalPlayer.townNPCs > 2f)
                return -1;

            if (Main.swapMusic)
            {
                if (Main.drunkWorld && !Main.remixWorld)
                    return MusicHelper.DecideOnNewConfectionMusic();
                else
                    return MusicHelper.DecideOnTOWConfectionMusic();
            }
            else if (!Main.gameMenu && Main.drunkWorld && !Main.remixWorld)
                return MusicHelper.DecideOnTOWConfectionMusic();
            else
                return MusicHelper.DecideOnNewConfectionMusic();
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override int Music => SelectMusic();
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<SavannaConfectionBGStyle>();

        public override void SetStaticDefaults()
        {
            ConfectionCompat();
        }

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static void ConfectionCompat()
        {
            SceneTileCounter.SurveyByType.Add(ModContent.GetInstance<SavannaBiome>().Type, new([ModContent.TileType<SavannaGrassConfection>(),
            ModContent.TileType<SavannaGrassConfectionMowed>()], 400));
        }

        public override bool IsSceneEffectActive(Player player) => InConfection(player) &&
            SceneTileCounter.SurveyByType[ModContent.GetInstance<SavannaBiome>().Type].Success;

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static bool InConfection(Player player) => ConfectionCheckTileCount.InConfection(player);
    }

    [JITWhenModsEnabled(AlternativeCompat.spirit)]
    public class SavannaConfectionBGStyle : ModSurfaceBackgroundStyle
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.confection);

        public override int ChooseMiddleTexture() => BackgroundTextureLoader.GetBackgroundSlot(Mod, "Confection/SpiritReforged/Savanna/Biome/Savanna/Biome/SavannaConfectionBackgroundMid");
        public override int ChooseFarTexture() => BackgroundTextureLoader.GetBackgroundSlot(Mod, "Confection/SpiritReforged/Savanna/Biome/Savanna/Biome/SavannaConfectionBackgroundFar");

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            b -= 400;
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, "Confection/SpiritReforged/Savanna/Biome/Savanna/Biome/SavannaConfectionBackgroundNear");
        }

        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                        fades[i] = 1f;
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                        fades[i] = 0f;
                }
        }
    }
}
