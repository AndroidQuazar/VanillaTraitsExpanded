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
	[HarmonyPatch(typeof(ToilEffects))]
	[HarmonyPatch("WithProgressBar")]
	public static class WithProgressBar_Patch
	{
		private static void Postfix(this Toil __result)
		{
			__result.AddPreTickAction(delegate
			{
				if (__result.actor.HasTrait(VTEDefOf.VTE_Clumsy) && Rand.Chance(0.01f))
                {
					Log.Message(__result.actor + " has a clumsy trait and is getting a bruise this time");
					var bruise = HediffMaker.MakeHediff(HediffDefOf.Bruise, __result.actor);
					__result.actor.health.AddHediff(bruise);
					Messages.Message("VTE.GotBruise".Translate(__result.actor.Named("PAWN")), __result.actor, MessageTypeDefOf.NeutralEvent, historical: false);
				}
				if (__result.actor.HasTrait(VTEDefOf.VTE_Perfectionist) && __result.actor.CurJobDef == JobDefOf.FinishFrame && Rand.Chance(0.01f))
                {
					Log.Message(__result.actor + " has Perfectionist trait and randomly decises interrupt current construction job");
					Messages.Message("VTE.DecisesInterruptCurrentCostructionJob".Translate(__result.actor.Named("PAWN"), __result.actor.CurJob.GetTarget(TargetIndex.A).Thing.Label), __result.actor, MessageTypeDefOf.NeutralEvent, historical: false);
					TraitUtils.TraitsManager.perfectionistsWithJobsToStop.Add(__result.actor);
				}
			});
		}
	}
}
