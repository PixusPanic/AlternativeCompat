using Avalon.NPCs.Bosses.PreHardmode;
using Avalon.NPCs.Bosses.PreHardmode.BacteriumPrime;
using Terraria.ModLoader;

namespace AlternativeCompat.Depths.MStorage
{
    [JITWhenModsEnabled(AlternativeCompat.mStorage)]
    public class BacteriumPrimeShadowDiamond : ModSystem
    {
        [JITWhenModsEnabled(AlternativeCompat.avalon)]
        private static int BacteriumPrime => ModContent.NPCType<BacteriumPrime>();

        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.avalon);

        public override void OnModLoad()
        {
            if (BacteriumPrime > -1 && ModLoader.TryGetMod(AlternativeCompat.mStorage, out var mStorage))
                mStorage.Call("Set Shadow Diamond Drop Rule", BacteriumPrime, mStorage.Call("Get Shadow Diamond Drop Rule", 1, -1));
        }
    }
}
