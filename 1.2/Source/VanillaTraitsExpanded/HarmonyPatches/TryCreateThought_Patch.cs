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
	[HarmonyPatch(typeof(SituationalThoughtHandler))]
	[HarmonyPatch("TryCreateThought")]
	public static class TryCreateThought_Patch
	{
		public static bool Prefix(SituationalThoughtHandler __instance, ThoughtDef def)
		{
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_HeatInclined) && def == ThoughtDefOf.SleptInHeat)
            {
				return false;
            }
			else if (__instance.pawn.HasTrait(VTEDefOf.VTE_ColdInclined) && def == ThoughtDefOf.SleptInCold)
			{
				return false;
			}
			else if (__instance.pawn.HasTrait(VTEDefOf.VTE_HeavySleeper) && def == ThoughtDefOf.SleepDisturbed)
            {
				return false;
            }
			return true;
		}
	}


	[HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemory", new Type[]
	{
		typeof(Thought_Memory),
		typeof(Pawn)
	})]
	internal static class TryGainMemory_Patch
	{
		private static List<ThoughtDef> animalThoughtDefs = new List<ThoughtDef>
		{
			ThoughtDefOf.BondedAnimalBanished,
			DefDatabase<ThoughtDef>.GetNamed("BondedAnimalDied"),
			DefDatabase<ThoughtDef>.GetNamed("BondedAnimalLost"),
			DefDatabase<ThoughtDef>.GetNamed("BondedAnimalMaster"),
			DefDatabase<ThoughtDef>.GetNamed("KilledColonyAnimal"),
			DefDatabase<ThoughtDef>.GetNamed("KilledMyBondedAnimal"),
			DefDatabase<ThoughtDef>.GetNamed("NotBondedAnimalMaster"),
			DefDatabase<ThoughtDef>.GetNamed("SoldMyBondedAnimal"),
			ThoughtDefOf.SoldMyBondedAnimalMood,
			ThoughtDefOf.Nuzzled
		};
		private static void Prefix(MemoryThoughtHandler __instance, ref Thought_Memory newThought, Pawn otherPawn)
		{
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_AnimalLover) && animalThoughtDefs.Contains(newThought.def))
            {
				newThought.moodPowerFactor *= 2f;
			}
		}
	}
}
