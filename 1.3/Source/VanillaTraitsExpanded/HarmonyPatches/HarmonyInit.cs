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
	[StaticConstructorOnStartup]
	internal static class HarmonyInit
	{
		static HarmonyInit()
		{
			new Harmony("OskarPotocki.VanillaTraitsExpanded").PatchAll();
		}
	}


	//[HarmonyPatch(typeof(Pawn_JobTracker), "StartJob")]
	//public class StartJobPatch
	//{
	//    private static void Postfix(Pawn_JobTracker __instance, Pawn ___pawn, Job newJob, JobTag? tag)
	//    {
	//        if (___pawn.RaceProps.Humanlike)// && (!___pawn.CurJobDef?.defName.Contains("Wait") ?? false))
	//        {
	//			Log.Message(___pawn + " is starting " + newJob);
	//		}
	//	}
	//}
	//
	//
	//[HarmonyPatch(typeof(Pawn_JobTracker), "EndCurrentJob")]
	//public class EndCurrentJobPatch2
	//{
	//    private static void Prefix(Pawn_JobTracker __instance, Pawn ___pawn, JobCondition condition, ref bool startNewJob, bool canReturnToPool = true)
	//    {
	//		if (___pawn.RaceProps.Humanlike)// && (!___pawn.CurJobDef?.defName.Contains("Wait") ?? false))
	//
	//		{
	//			Log.Message(___pawn + " is ending " + ___pawn.CurJob);
	//		}
	//	}
	//}
	//
	//[HarmonyPatch(typeof(ThinkNode_JobGiver), "TryIssueJobPackage")]
	//public class TryIssueJobPackage
	//{
	//	private static void Postfix(ThinkNode_JobGiver __instance, ThinkResult __result, Pawn pawn, JobIssueParams jobParams)
	//	{
	//		if (pawn.RaceProps.Humanlike && __result.Job != null)
	//		{
	//			Log.Message(pawn + " gets " + __result.Job + " from " + __instance);
	//		}
	//	}
	//}
}
