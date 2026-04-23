using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Liquids;

namespace AlternativeCompat.Utils
{
    public class QuicksilverCondition : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("ModLiquidLib") && ModLoader.HasMod(AlternativeCompat.depths);

        [JITWhenModsEnabled(AlternativeCompat.depths)]
        public static int Quicksilver => LiquidLoader.LiquidType<Quicksilver>();

        public static void NearQuicksilver(Recipe recipe) =>
            recipe.AddCondition(Language.GetOrRegister("Mods.TheDepths.Recipes.NearQuicksilver"),
                LiquidLoader.NearLiquid(Quicksilver).Predicate);

        public static bool NearQuicksilver() => LiquidLoader.NearLiquid(Quicksilver).IsMet();
    }
}
