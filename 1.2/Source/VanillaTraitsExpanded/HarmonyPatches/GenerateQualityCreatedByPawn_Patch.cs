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
	[HarmonyPatch(typeof(QualityUtility))]
	[HarmonyPatch("GenerateQualityCreatedByPawn")]
	[HarmonyPatch(new Type[]
		{
			typeof(Pawn),
			typeof(SkillDef)
		}, new ArgumentType[]
		{
			0,
			0
		})]
	public static class GenerateQualityCreatedByPawn_Patch
	{
		private static void Postfix(ref QualityCategory __result, Pawn pawn, SkillDef relevantSkill)
		{
			if (pawn.HasTrait(VTEDefOf.VTE_Perfectionist))
            {
				if (__result != QualityCategory.Legendary)
                {
					var newResult = (QualityCategory)((int)__result + 1);
					//Log.Message(pawn + " has VTE_Perfectionist, product quality was increased from " + __result + " to " + newResult);
					__result = newResult;
				}
				if (__result == QualityCategory.Normal || __result == QualityCategory.Awful || __result == QualityCategory.Poor)
				{
					pawn.TryGiveThought(VTEDefOf.VTE_CreatedLowQualityItem);
					//Log.Message(pawn + " has VTE_Perfectionist, gains VTE_CreatedLowQualityItem thought due to low quality item");
				}
				if (__result == QualityCategory.Legendary && pawn.InspirationDef != InspirationDefOf.Inspired_Creativity)
                {
					__result = QualityCategory.Masterwork;
				}
			}
		}
	}
}
