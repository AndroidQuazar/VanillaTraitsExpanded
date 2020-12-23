using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
    [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
    internal static class MakeRecipeProducts_Patch
    {
        private static void Postfix(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient, IBillGiver billGiver)
        {
            if (worker.HasTrait(VTEDefOf.VTE_MadSurgeon))
            {
                if ((recipeDef == DefDatabase<RecipeDef>.GetNamed("ButcherCorpseFlesh") && ingredients != null && ingredients.Where(x => x is Corpse corpse).Any()) || recipeDef == RecipeDefOf.RemoveBodyPart)
                {
                    TraitUtils.TraitsManager.madSurgeonsWithLastHarvestedTick[worker] = GenTicks.TicksAbs;
                    worker.TryGiveThought(VTEDefOf.VTE_HarvestedOrgans);
                }
            }
        }
    }
}
