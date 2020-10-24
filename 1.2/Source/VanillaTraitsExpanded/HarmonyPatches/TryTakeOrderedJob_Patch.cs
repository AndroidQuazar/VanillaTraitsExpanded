using HarmonyLib;
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
			if (___pawn.story?.traits?.HasTrait(TraitsDefOf.VTE_AbsentMinded) ?? false)
            {
				Log.Message(___pawn + " starts " + job);
				TraitUtils.TraitsManager.forcedJobs[___pawn] = job;
            }
		}
	}
}
