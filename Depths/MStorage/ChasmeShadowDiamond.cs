using Terraria.ModLoader;
using TheDepths.NPCs.Chasme;

namespace AlternativeCompat.Depths.MStorage
{
    [JITWhenModsEnabled("MagicStorage")]
    public class ChasmeShadowDiamond : ModSystem
    {
        [JITWhenModsEnabled(AlternativeCompat.depths)]
        private static int Chasme => ModContent.NPCType<ChasmeHeart>();

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.depths);

        public override void OnModLoad()
        {
            if (Chasme > -1 && ModLoader.TryGetMod("MagicStorage", out var mStorage))
                mStorage.Call("Set Shadow Diamond Drop Rule", Chasme, mStorage.Call("Get Shadow Diamond Drop Rule", 1, -1));
        }
    }
}
