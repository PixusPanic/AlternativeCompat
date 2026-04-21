using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AlternativeCompat.Depths.MStorage
{
    [JITWhenModsEnabled("MagicStorage")]
    public class ChasmeShadowDiamond : ModSystem
    {
        int chasme = -1;

        public override bool IsLoadingEnabled(Mod mod)
        {
            if (!ModLoader.TryGetMod(AlternativeCompat.depths, out var depths) ||
                !depths.TryFind("ChasmeHeart", out ModNPC chasmeBoss)) return false;
            chasme = chasmeBoss.Type;

            return true;
        }

        public override void OnModLoad()
        {
            if (chasme > -1 && ModLoader.TryGetMod("MagicStorage", out var storage))
                storage.Call("Set Shadow Diamond Drop Rule", chasme, storage.Call("Get Shadow Diamond Drop Rule", 1, -1));
        }
    }
}
