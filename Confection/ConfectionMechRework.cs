using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using static Terraria.GameContent.ItemDropRules.Chains;
using static Terraria.GameContent.ItemDropRules.Conditions;
using static TheConfectionRebirth.NPCs.ConfectionGlobalNPC;

namespace AlternativeCompat.Confection
{
    //[JITWhenModsEnabled("PrimeRework")]
    public class ConfectionMRGlobalItem : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod) =>
            ModLoader.HasMod("PrimeRework") && ModLoader.HasMod(AlternativeCompat.confection);

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static int GetOre(int oreType)
        {
            return oreType switch
            {
                1 => ModContent.ItemType<HallowedOre>(),
                2 => ModContent.ItemType<NeapoliniteOre>(),
                _ => -1,
            };
        }

        public override bool InstancePerEntity => true;

        int hallowedOre = ModLoader.HasMod(AlternativeCompat.confection) ? GetOre(1) : -1;
        int neapoliniteOre = ModLoader.HasMod(AlternativeCompat.confection) ? GetOre(2) : -1;

        private static int FindBossBag(string bossBag)
        {
            if (ModContent.TryFind("PrimeRework", bossBag, out ModItem bossType))
                return bossType.Type;
            return -1;
        }

        int terminator = FindBossBag("TerminatorBossBag");
        int caretaker = FindBossBag("CaretakerBossBag");
        int mechclops = FindBossBag("MechclopsBossBag");
        int siegeEngine = FindBossBag("SiegeEngineBossBag");

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (ModLoader.HasMod(AlternativeCompat.confection) &&
                (IsEqualTo(item, terminator) || IsEqualTo(item, caretaker) ||
                IsEqualTo(item, mechclops) || IsEqualTo(item, siegeEngine)))
                OreLoot(itemLoot);
        }

        private static bool IsEqualTo(Item item, int boss) => item.type == boss && boss > -1;

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private void OreLoot(ItemLoot itemLoot)
        {
            itemLoot.Remove(FindHallowedBars(itemLoot));

            DrunkWorldIsNotActive NotDrunk = new();

            LeadingConditionRule ConfectionCondition = new(new ConfectionDropRule());
            ConfectionCondition.OnSuccess(ItemDropRule.ByCondition(NotDrunk, neapoliniteOre, 1, 20 * 5, 35 * 5));
            itemLoot.Add(ConfectionCondition);

            LeadingConditionRule HallowCondition = new(new HallowDropRule());
            HallowCondition.OnSuccess(ItemDropRule.ByCondition(NotDrunk, hallowedOre, 1, 20 * 5, 35 * 5));
            itemLoot.Add(HallowCondition);

            LeadingConditionRule DrunkCondition = new(new DrunkWorldIsActive());
            DrunkCondition.OnSuccess(ItemDropRule.Common(hallowedOre, 1, 10 * 5, 18 * 5));
            DrunkCondition.OnSuccess(ItemDropRule.Common(neapoliniteOre, 1, 10 * 5, 18 * 5));
            itemLoot.Add(DrunkCondition);
        }


        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static CommonDrop FindHallowedBars(ItemLoot loot)
        {
            foreach (IItemDropRule item in loot.Get(false))
            {
                CommonDrop c = (CommonDrop)(object)(item is CommonDrop ? item : null);
                if (c != null && c.itemId == ItemID.HallowedBar)
                    return (object)c as CommonDrop;
            }
            return null;
        }
    }

    //[JITWhenModsEnabled("PrimeRework")]
    public class ConfectionMRGlobalNPC : GlobalNPC
    {
        public override bool IsLoadingEnabled(Mod mod) =>
            ModLoader.HasMod("PrimeRework") && ModLoader.HasMod(AlternativeCompat.confection);

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private static int GetOre(int oreType)
        {
            return oreType switch
            {
                1 => ModContent.ItemType<HallowedOre>(),
                2 => ModContent.ItemType<NeapoliniteOre>(),
                _ => -1,
            };
        }

        public override bool InstancePerEntity => true;

        int hallowedOre = ModLoader.HasMod(AlternativeCompat.confection) ? GetOre(1) : -1;
        int neapoliniteOre = ModLoader.HasMod(AlternativeCompat.confection) ? GetOre(2) : -1;

        private static int FindBoss(string bossName)
        {
            if (ModContent.TryFind("PrimeRework", bossName, out ModNPC bossType))
                return bossType.Type;
            return -1;
        }

        int terminator = FindBoss("TheTerminator");
        int caretaker = FindBoss("Caretaker");
        int mechclops = FindBoss("Mechclops");
        int siegeEngine = FindBoss("SiegeEngine");
        int mechdusa = FindBoss("MechdusaTail");

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (ModLoader.HasMod(AlternativeCompat.confection) && (IsEqualTo(npc, terminator) ||
                IsEqualTo(npc, caretaker) || IsEqualTo(npc, mechclops) ||
                IsEqualTo(npc, siegeEngine) || IsEqualTo(npc, mechdusa)))
                OreLoot(npcLoot);
        }

        private static bool IsEqualTo(NPC npc, int boss) => npc.type == boss && boss > -1;

        [JITWhenModsEnabled(AlternativeCompat.confection)]
        private void OreLoot(NPCLoot npcLoot)
        {
            npcLoot.RemoveWhere(rule =>
            {
                if (rule is LeadingConditionRule leadingRule)
                {
                    return leadingRule.ChainedRules.Any(chain =>
                        chain is TryIfSucceeded successChain &&
                        successChain.RuleToChain is CommonDrop drop &&
                        drop.itemId == ItemID.HallowedBar
                    );
                }
                ;
                return false;
            });

            NotDrunkandExpert ExpertDrunkmode = new();
            IItemDropRuleCondition Expertmode = new NotExpert();

            LeadingConditionRule ConfectionCondition = new(new ConfectionDropRule());
            ConfectionCondition.OnSuccess(ItemDropRule.ByCondition(ExpertDrunkmode, neapoliniteOre, 1, 15 * 5, 30 * 5));
            npcLoot.Add(ConfectionCondition);

            LeadingConditionRule HallowCondition = new(new HallowDropRule());
            HallowCondition.OnSuccess(ItemDropRule.ByCondition(ExpertDrunkmode, hallowedOre, 1, 15 * 5, 30 * 5));
            npcLoot.Add(HallowCondition);

            LeadingConditionRule DrunkCondition = new(new DrunkWorldIsActive());
            DrunkCondition.OnSuccess(ItemDropRule.ByCondition(Expertmode, hallowedOre, 1, 8 * 5, 15 * 5));
            ConfectionCondition.OnSuccess(ItemDropRule.ByCondition(Expertmode, neapoliniteOre, 1, 8 * 5, 15 * 5));
            npcLoot.Add(DrunkCondition);
        }
    }
}
