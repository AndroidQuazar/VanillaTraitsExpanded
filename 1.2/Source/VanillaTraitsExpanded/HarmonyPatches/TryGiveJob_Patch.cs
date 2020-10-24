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
	[HarmonyPatch(typeof(JobGiver_GetRest))]
	[HarmonyPatch("TryGiveJob")]
	public static class TryGiveJob_Patch
	{
		public static bool Prefix(Pawn pawn)
		{
			if (pawn.HasTrait(VTEDefOf.VTE_Workaholic) && Rand.Chance(0.8f))
            {
				return false;
			}
			return true;
		}
	}
}
