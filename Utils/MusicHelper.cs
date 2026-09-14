using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AlternativeCompat.Utils
{
    public class MusicHelper : ModSystem
    {
        [JITWhenModsEnabled(AlternativeCompat.confection)]
        public static int DecideOnTOWConfectionMusic()
        {
            int newMusic = MusicID.OtherworldlyHallow;
            if ((double)Main.LocalPlayer.position.Y >= Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2) && !WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16))
            {
                if (Main.remixWorld && (double)Main.LocalPlayer.position.Y >= Main.rockLayer * 16.0 + (double)(Main.screenHeight / 2))
                {
                    if (Main.LocalPlayer.ZoneUndergroundDesert)
                    {
                        newMusic = MusicID.OtherworldlyDesert;
                    }
                    else if (Main.cloudAlpha > 0f)
                    {
                        newMusic = MusicID.OtherworldlyRain;
                    }
                    else
                    {
                        newMusic = MusicID.OtherworldlyHallow;
                    }
                }
                else
                {
                    newMusic = MusicLoader.GetMusicSlot(ModLoader.GetMod(AlternativeCompat.confection), "Sounds/Music/OtherworldlyConfectionUnderground");
                }
            }
            else if (Main.dayTime)
            {
                if (Main.cloudAlpha > 0f && !Main.gameMenu)
                {
                    newMusic = MusicID.OtherworldlyRain;
                }
                else
                {
                    newMusic = MusicID.OtherworldlyHallow;
                }
            }
            else if (Main._shouldUseStormMusic)
            {
                if (Main.bloodMoon)
                {
                    newMusic = MusicID.OtherworldlyEerie;
                }
                else
                {
                    newMusic = MusicID.OtherworldlyRain;
                }
            }
            else if (WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16))
            {
                if (Main.bloodMoon)
                {
                    newMusic = MusicID.OtherworldlyEerie;
                }
                else
                {
                    newMusic = MusicID.OtherworldlyOcean;
                }
            }
            else if (Main.LocalPlayer.ZoneDesert)
            {
                newMusic = MusicID.OtherworldlyDesert;
            }
            else if (Main.remixWorld)
            {
                newMusic = MusicID.OtherworldlySpace;
            }
            else if (!Main.dayTime)
            {
                if (Main.bloodMoon)
                {
                    newMusic = MusicID.OtherworldlyEerie;
                }
                else if (Main.cloudAlpha > 0f && !Main.gameMenu)
                {
                    newMusic = MusicID.OtherworldlyNight;
                }
                else
                {
                    newMusic = MusicID.OtherworldlyNight;
                }
            }
            return newMusic;
        }

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        public static int DecideOnNewConfectionMusic()
        {
            int newMusic = MusicLoader.GetMusicSlot(ModLoader.GetMod(AlternativeCompat.confection), "Sounds/Music/Confection");
            bool flag10 = Main.LocalPlayer.townNPCs > 2f;
            if (Main.SceneMetrics.ShadowCandleCount > 0 || Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == ItemID.ShadowCandle)
            {
                flag10 = false;
            }
            if ((double)Main.LocalPlayer.position.Y >= Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2) && (Main.remixWorld || !WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16)))
            {
                if (Main.remixWorld && (double)Main.LocalPlayer.position.Y >= Main.rockLayer * 16.0 + (double)(Main.screenHeight / 2))
                {
                    newMusic = MusicLoader.GetMusicSlot(ModLoader.GetMod(AlternativeCompat.confection), "Sounds/Music/Confection");
                }
                else
                {
                    newMusic = MusicLoader.GetMusicSlot(ModLoader.GetMod(AlternativeCompat.confection), "Sounds/Music/ConfectionUnderground");
                }
            }
            else if (Main.dayTime)
            {
                if (Main._shouldUseStormMusic)
                {
                    newMusic = MusicID.Monsoon;
                }
                else if (Main.cloudAlpha > 0f && !Main.gameMenu)
                {
                    newMusic = MusicID.Rain;
                }
                else if (Main._shouldUseWindyDayMusic && !Main.remixWorld)
                {
                    newMusic = MusicID.WindyDay;
                }
                else
                {
                    newMusic = MusicLoader.GetMusicSlot(ModLoader.GetMod(AlternativeCompat.confection), "Sounds/Music/Confection");
                }
            }
            else if (Main._shouldUseStormMusic)
            {
                if (Main.bloodMoon)
                {
                    newMusic = MusicID.Eerie;
                }
                else
                {
                    newMusic = MusicID.Monsoon;
                }
            }
            else if (WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16))
            {
                if (Main.bloodMoon)
                {
                    newMusic = MusicID.Eerie;
                }
                else if (flag10)
                {
                    if (Main.dayTime)
                    {
                        newMusic = MusicID.TownDay;
                    }
                    else
                    {
                        newMusic = MusicID.TownNight;
                    }
                }
                else
                {
                    newMusic = (Main.dayTime ? MusicID.Ocean : MusicID.OceanNight);
                }
            }
            else if (Main.LocalPlayer.ZoneDesert)
            {
                if ((double)Main.LocalPlayer.position.Y >= Main.worldSurface * 16.0)
                {
                    int num6 = (int)(Main.LocalPlayer.Center.X / 16f);
                    int num7 = (int)(Main.LocalPlayer.Center.Y / 16f);
                    if (WorldGen.InWorld(num6, num7) && (WallID.Sets.Conversion.Sandstone[Main.tile[num6, num7].WallType] || WallID.Sets.Conversion.HardenedSand[Main.tile[num6, num7].WallType]))
                    {
                        newMusic = MusicID.UndergroundDesert;
                    }
                    else
                    {
                        newMusic = MusicID.Desert;
                    }
                }
                else
                {
                    newMusic = MusicID.Desert;
                }
            }
            else if (Main.remixWorld)
            {
                newMusic = (Main.dayTime ? MusicID.SpaceDay : MusicID.Space);
            }
            else if (!Main.dayTime)
            {
                if (Main.bloodMoon)
                {
                    newMusic = MusicID.Eerie;
                }
                else if (Main.cloudAlpha > 0f && !Main.gameMenu)
                {
                    newMusic = MusicID.Rain;
                }
                else
                {
                    newMusic = MusicID.Night;
                }
            }
            return newMusic;
        }
    }
}
