using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;

namespace AlternativeCompat.Confection
{
    public class GetConfectionBiomeConversion
    {
        [JITWhenModsEnabled(AlternativeCompat.confection)]
        public static int ConvID => ModContent.GetInstance<ConfectionBiomeConversion>().Type;
    }
}
