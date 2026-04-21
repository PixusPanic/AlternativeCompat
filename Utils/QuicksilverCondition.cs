using ModLiquidLib.ModLoader;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Liquids;

namespace AlternativeCompat.Utils
{
    [ExtendsFromMod("ModLiquidLib", "TheDepths")]
    public class QuicksilverCondition : ModSystem
    {
        public static void NearQuicksilver(Recipe recipe) =>
            recipe.AddCondition(Language.GetOrRegister("Mods.TheDepths.Recipes.NearQuicksilver"),
                LiquidLoader.NearLiquid(LiquidLoader.LiquidType<Quicksilver>()).Predicate);

        public static bool NearQuicksilver() => LiquidLoader.NearLiquid(LiquidLoader.LiquidType<Quicksilver>()).IsMet();
    }
}
