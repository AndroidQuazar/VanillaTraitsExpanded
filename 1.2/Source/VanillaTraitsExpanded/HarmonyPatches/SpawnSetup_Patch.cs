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
	[HarmonyPatch(typeof(Pawn))]
	[HarmonyPatch("SpawnSetup")]

	public static class SpawnSetup_Patch
	{
		private static void Postfix(Pawn __instance)
		{
			if (__instance.HasTrait(VTEDefOf.VTE_Coward))
            {
				TraitUtils.TraitsManager.cowards.Add(__instance);
            }
		}
	}
}
