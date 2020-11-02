using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class JobGiver_StealingItems : ThinkNode_JobGiver
	{
		public Pawn GetCandidateToSteal(List<Pawn> pawns)
        {
			foreach (var p in pawns)
            {
				if (p.CurJobDef != JobDefOf.Goto)
                {
					return p;
                }
            }
			return null;
        }
		public override Job TryGiveJob(Pawn pawn)
		{
			var mentalState = pawn.MentalState as MentalState_Kleptomaniac;
			if (mentalState == null)
			{
				return null;
			}
			var pawnsCandidates = pawn.Map.mapPawns.AllPawns.Where(x => x.RaceProps.Humanlike && x.FactionOrExtraMiniOrHomeFaction != pawn.Faction && !x.HostileTo(pawn)).ToList();
			if (pawnsCandidates != null && pawnsCandidates.Count > 0)
            {
				var victim = GetCandidateToSteal(pawnsCandidates.OrderBy(x => x.Position.DistanceTo(pawn.Position)).ToList());
				if (victim != null)
                {
					return JobMaker.MakeJob(VTEDefOf.VTE_StealItems, victim);
                }
            }
			return null;
		}
	}
}
