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
	[HarmonyPatch(typeof(Pawn_JobTracker))]
	[HarmonyPatch("TryTakeOrderedJob")]
	public static class TryTakeOrderedJob_Patch
	{
		private static void Prefix(Pawn ___pawn, Job job)
		{
            if (job.def != JobDefOf.Flee && ___pawn.HasTrait(VTEDefOf.VTE_AbsentMinded))
            {
				Log.Message(___pawn + " starts " + job);
				TraitUtils.TraitsManager.forcedJobs[___pawn] = job;
			}
		}
	}

	[HarmonyPatch(typeof(Pawn_JobTracker))]
	[HarmonyPatch("StartJob")]
	public static class StartJob_Patch
	{
		private static bool Prefix(Pawn ___pawn, Job newJob, JobCondition lastJobEndCondition)
		{
			if (newJob.def == JobDefOf.Vomit && ___pawn.HasTrait(VTEDefOf.VTE_IronStomach))
			{
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(FoodUtility))]
	[HarmonyPatch("AddFoodPoisoningHediff")]
	public static class AddFoodPoisoningHediff_Patch
	{
		private static bool Prefix(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
		{
			if (pawn.HasTrait(VTEDefOf.VTE_IronStomach))
			{
				return false;
			}
			return true;
		}
	}
}
