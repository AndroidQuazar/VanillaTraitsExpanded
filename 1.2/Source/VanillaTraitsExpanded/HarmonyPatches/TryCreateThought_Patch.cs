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

	[HarmonyPatch(typeof(Bill), "PawnAllowedToStartAnew", new Type[]
	{
		typeof(Pawn)
	})]
	public static class PawnAllowedToStartAnew_Patch
	{
		public static bool Prefix(Pawn p, RecipeDef ___recipe)
		{
			if (p.HasTrait(VTEDefOf.VTE_AnimalLover) && ___recipe == DefDatabase<RecipeDef>.GetNamed("ButcherCorpseFlesh"))
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

		public static Dictionary<ThoughtDef, ThoughtDef> inverseAnimalThoughDefs = new Dictionary<ThoughtDef, ThoughtDef>
		{
			{ThoughtDefOf.BondedAnimalBanished, VTEDefOf.VTE_BondedAnimalBanishedHater },
			{DefDatabase<ThoughtDef>.GetNamed("BondedAnimalDied"), VTEDefOf.VTE_BondedAnimalDiedHater},
			{DefDatabase<ThoughtDef>.GetNamed("BondedAnimalLost"), VTEDefOf.VTE_BondedAnimalLostHater },
			{DefDatabase<ThoughtDef>.GetNamed("BondedAnimalMaster"), VTEDefOf.VTE_BondedAnimalMasterHater },
			{DefDatabase<ThoughtDef>.GetNamed("KilledColonyAnimal"), VTEDefOf.VTE_KilledColonyAnimalHater },
			{DefDatabase<ThoughtDef>.GetNamed("KilledMyBondedAnimal"), VTEDefOf.VTE_KilledMyBondedAnimalHater },
			{DefDatabase<ThoughtDef>.GetNamed("NotBondedAnimalMaster"), VTEDefOf.VTE_NotBondedAnimalMasterHater },
			{DefDatabase<ThoughtDef>.GetNamed("SoldMyBondedAnimal"), VTEDefOf.VTE_SoldMyBondedAnimalHater },
			{ThoughtDefOf.SoldMyBondedAnimalMood, VTEDefOf.VTE_SoldMyBondedAnimalMoodHater },
			{ThoughtDefOf.Nuzzled, VTEDefOf.VTE_NuzzledHater}
		};
		private static void Prefix(MemoryThoughtHandler __instance, ref Thought_Memory newThought, Pawn otherPawn)
		{
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_AnimalLover) && animalThoughtDefs.Contains(newThought.def))
            {
				newThought.moodPowerFactor *= 2f;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_Vengeful) && newThought.def == ThoughtDefOf.KilledMyRival)
            {
				newThought.moodPowerFactor *= 2f;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_AnimalHater) && animalThoughtDefs.Contains(newThought.def))
            {
				newThought = (Thought_Memory)ThoughtMaker.MakeThought(inverseAnimalThoughDefs[newThought.def]);
			}
		}
	}
}
