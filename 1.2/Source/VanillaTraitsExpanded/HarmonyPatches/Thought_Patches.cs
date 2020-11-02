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
	[HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility), "TryGiveThoughts", new Type[]
	{
		typeof(Pawn),
		typeof(DamageInfo?),
		typeof(PawnDiedOrDownedThoughtsKind)
	})]
	public static class TryGiveThoughts_Patch
	{
		public static void Postfix(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
        {
			if (victim.RaceProps.IsMechanoid && thoughtsKind == PawnDiedOrDownedThoughtsKind.Died)
            {
				foreach (var pawn in victim.Map.mapPawns.AllPawns)
                {
					if (pawn.HasTrait(VTEDefOf.VTE_Technophobe))
                    {
						pawn.TryGiveThought(VTEDefOf.VTE_MechanoidIsKilled);
						Log.Message(pawn + " gets a VTE_MechanoidIsKilled thought due to killed mechanoid");
                    }
                }
            }
		}
	}

	[HarmonyPatch(typeof(Thought_Memory), "ShouldDiscard", MethodType.Getter)]
	public static class ShouldDiscard_Patch
	{
		public static bool Prefix(Thought_Memory __instance, ref bool __result)
		{
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_FunLoving) && (__instance.def == ThoughtDefOf.AttendedParty
				|| __instance.def.defName == "VFEV_AttendedFeast" || __instance.def.defName == "VFEV_TakingPartInFeast")) // vikings compatibility
            {
				if (__instance.age < (__instance.def.DurationTicks * 4))
                {
					__result = false;
					return false;
				}
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
	public static class TryGainMemory_Patch
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
		private static bool Prefix(MemoryThoughtHandler __instance, ref Thought_Memory newThought, Pawn otherPawn)
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
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_Squeamish) && newThought.def == ThoughtDefOf.ObservedLayingRottingCorpse && Rand.Chance(0.5f))
			{
				Job vomit = JobMaker.MakeJob(JobDefOf.Vomit);
				__instance.pawn.jobs.TryTakeOrderedJob(vomit);
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_Desensitized) && horribleThoughts.Contains(newThought.def.defName))
			{
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_RefinedPalate)) 
			{
				if (newThought.def == ThoughtDefOf.AteFineMeal || newThought.def.defName == "VCE_AteFineDessert")
				{
					newThought.moodPowerFactor = 0;
				}
				else if (newThought.def == ThoughtDefOf.AteLavishMeal || newThought.def.defName == "VCE_AteGourmetMeal" || newThought.def.defName == "VCE_AteLavishDessert")
                {
					newThought.moodPowerFactor *= 1.5f;
				}
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_ColdInclined) && newThought.CurStageIndex < 1 && newThought.def == ThoughtDef.Named("EnvironmentCold"))
			{
				return false;
            }
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_HeatInclined) && newThought.CurStageIndex < 1 && newThought.def == ThoughtDef.Named("EnvironmentHot"))
			{
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_ChildOfMountain) && newThought.def == ThoughtDef.Named("EnvironmentDark"))
			{
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_ChildOfSea) && newThought.def == ThoughtDef.Named("SoakingWet"))
			{
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_MadSurgeon) && (newThought.def == ThoughtDefOf.KnowColonistOrganHarvested || newThought.def == ThoughtDefOf.KnowGuestOrganHarvested))
			{
				return false;
			}
			if (__instance.pawn.HasTrait(VTEDefOf.VTE_Prude) && newThought.def == ThoughtDefOf.Naked)
            {
				newThought.moodPowerFactor *= 2f;
			}
			return true;
		}


		public static List<string> horribleThoughts = new List<string>
		{
			"KnowGuestExecuted",
			"KnowColonistExecuted",
			"KnowPrisonerDiedInnocent",
			"KnowColonistDied",
			"BondedAnimalDied",
			"PawnWithGoodOpinionDied",
			"PawnWithBadOpinionDied",
			"MySonDied",
			"MyDaughterDied",
			"MyHusbandDied",
			"MyWifeDied",
			"MyFianceDied",
			"MyFianceeDied",
			"MyLoverDied",
			"MyBrotherDied",
			"MySisterDied",
			"MyGrandchildDied",
			"MyFatherDied",
			"MyMotherDied",
			"MyNieceDied",
			"MyNephewDied",
			"MyHalfSiblingDied",
			"MyAuntDied",
			"MyUncleDied",
			"MyGrandparentDied",
			"MyCousinDied",
			"MyKinDied",
			"ColonistLost",
			"BondedAnimalLost",
			"PawnWithGoodOpinionLost",
			"PawnWithBadOpinionLost",
			"MySonLost",
			"MyDaughterLost",
			"MyHusbandLost",
			"MyWifeLost",
			"MyFianceLost",
			"MyFianceeLost",
			"MyLoverLost",
			"MyBrotherLost",
			"MySisterLost",
			"MyGrandchildLost",
			"MyFatherLost",
			"MyMotherLost",
			"MyNieceLost",
			"MyNephewLost",
			"MyHalfSiblingLost",
			"MyAuntLost",
			"MyUncleLost",
			"MyGrandparentLost",
			"MyCousinLost",
			"MyKinLost",
			"KnowPrisonerSold",
			"KnowGuestOrganHarvested",
			"KnowColonistOrganHarvested",
			"MyOrganHarvested",
			"ObservedLayingCorpse",
			"ObservedLayingRottingCorpse",
			"WitnessedDeathAlly",
			"WitnessedDeathNonAlly",
			"WitnessedDeathFamily",
			"WitnessedDeathBloodlust",
			"KilledHumanlikeBloodlust",
			"ColonistBanished",
			"ColonistBanishedToDie",
			"PrisonerBanishedToDie",
			"BondedAnimalBanished",
			"FailedToRescueRelative",
			"KilledMyFriend",
			"KilledMyRival",
			"KilledMyLover",
			"KilledMyFiance",
			"KilledMySpouse",
			"KilledMyFather",
			"KilledMyMother",
			"KilledMySon",
			"KilledMyDaughter",
			"KilledMyBrother",
			"KilledMySister",
			"KilledMyKin",
			"KilledMyBondedAnimal",
			"SoldPrisoner",
			"ExecutedPrisoner",
			"KilledColonist",
			"KilledColonyAnimal",
			"OtherTravelerDied"
		};
	}
}
