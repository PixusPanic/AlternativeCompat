using AlternativeCompat.Utils;
using MagicStorage.Items;
using ModLiquidLib.Utils.LiquidContent;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using TheDepths.Items.Weapons;

namespace AlternativeCompat.Depths.MStorage
{
    [JITWhenModsEnabled("MagicStorage", AlternativeCompat.depths)]
    public class AddQuicksilverToMS : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) =>
            ModLoader.HasMod(AlternativeCompat.depths) && ModLoader.HasMod("MagicStorage");

        #region Detour Biome Globe
        public override void Load()
        {
            On_Recipe.FindRecipes += Hook_FindRecipes;
        }

        public override void Unload()
        {
            On_Recipe.FindRecipes -= Hook_FindRecipes;
        }

        // I'm kinda clueless how to optimize this, so I'm mostly resorting to referencing the original MS code until it's rewritten
        [JITWhenModsEnabled("ModLiquidLib", AlternativeCompat.depths)]
        private void Hook_FindRecipes(On_Recipe.orig_FindRecipes orig, bool canDelayCheck)
        {
            // For whatever reason, this hook can end up running during worldgen and the main menu
            if (Main.gameMenu || WorldGen.gen)
            {
                orig(canDelayCheck);
                return;
            }

            Player player = Main.LocalPlayer;

            bool oldQuicksilver = player.GetModPlayer<ModLiquidPlayer>().AdjLiquid[QuicksilverCondition.Quicksilver];

            //Override these flags
            if (player.GetModPlayer<BiomePlayer>().biomeGlobe) {
                player.GetModPlayer<ModLiquidPlayer>().AdjLiquid[QuicksilverCondition.Quicksilver] = true;
            }

            orig(canDelayCheck);

            player.GetModPlayer<ModLiquidPlayer>().AdjLiquid[QuicksilverCondition.Quicksilver] = oldQuicksilver;
        }
        #endregion

        #region IL edit the crafting interface
        private static ILHook AddQuicksilverToMSCrafting;

        public override void OnModLoad()
        {
            var MS = ModLoader.GetMod("MagicStorage");
            if (MS == null) return;

            var MSAssembly = MS.GetType().Assembly;
            if (MSAssembly == null) return;

            var MSUtility = MSAssembly.GetType("MagicStorage.Utility");
            if (MSUtility != null)
            {
                var AddCraftingZones = MSUtility.GetMethod("SetVanillaAdjTiles", BindingFlags.Public | BindingFlags.Static);
                if (AddCraftingZones == null) return;
                
                AddQuicksilverToMSCrafting = new ILHook(AddCraftingZones, PatchQuicksilverToMSCrafting);
                AddQuicksilverToMSCrafting.Apply();
            }
        }

        public override void OnModUnload()
        {
            AddQuicksilverToMSCrafting?.Dispose();
        }

        // Like before, mostly resorting to how Magic Storage does this for now
        [JITWhenModsEnabled("ModLiquidLib", AlternativeCompat.depths)]
        private void PatchQuicksilverToMSCrafting(ILContext il)
        {
            var c = new ILCursor(il);

            // Go to the end of it, then inject this code
            c.Index = c.Instrs.Count - 1;

            c.Emit(OpCodes.Ldarg_0); // Item
            c.Emit(OpCodes.Ldloc_0); // Player
            c.EmitDelegate((Item item, Player player) => {
                if (item.type == ModContent.ItemType<QuicksilverBucket>() ||
                    item.type == ModContent.ItemType<BottomlessQuicksilverBucket>() ||
                    item.type == ModContent.ItemType<BiomeGlobe>())
                    player.GetModPlayer<ModLiquidPlayer>().AdjLiquid[QuicksilverCondition.Quicksilver] = true;
            });
        }
        #endregion
    }
}
