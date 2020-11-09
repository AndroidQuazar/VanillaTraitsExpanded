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
	[HarmonyPatch(typeof(Bill))]
	[HarmonyPatch("Notify_PawnDidWork")]
	public static class Notify_PawnDidWork_Patch
	{
		private static void Postfix(Bill __instance, Pawn p)
		{			
			if (p.HasTrait(VTEDefOf.VTE_Perfectionist) && __instance.recipe.workAmount >= 2200 && Find.TickManager.TicksGame % GenDate.TicksPerHour * 3 == 0 && Rand.Chance(0.5f))
			{
				Log.Message(p + " has Perfectionist trait and randomly decises interrupt current bill job");
				TraitUtils.TraitsManager.perfectionistsWithJobsToStop.Add(p);
			}
		}
	}
}
