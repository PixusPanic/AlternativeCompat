using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Liquids;

namespace AlternativeCompat.Utils
{
    //[JITWhenModsEnabled(AlternativeCompat.liquidLib)]
    public class QuicksilverCondition : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod(AlternativeCompat.depths);

        [JITWhenModsEnabled(AlternativeCompat.liquidLib, AlternativeCompat.depths)]
        public static int Quicksilver => LiquidLoader.LiquidType<Quicksilver>();

        [JITWhenModsEnabled(AlternativeCompat.liquidLib)]
        public static void NearQuicksilver(Recipe recipe) =>
            recipe.AddCondition(Language.GetOrRegister("Mods.TheDepths.Recipes.NearQuicksilver"),
                LiquidLoader.NearLiquid(Quicksilver).Predicate);

        [JITWhenModsEnabled(AlternativeCompat.liquidLib)]
        public static bool NearQuicksilver() => LiquidLoader.NearLiquid(Quicksilver).IsMet();
    }
}
