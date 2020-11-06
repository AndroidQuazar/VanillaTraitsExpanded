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
        public Pawn GetCandidateToSteal(Pawn thief, List<Pawn> pawns)
        {
            foreach (var p in pawns)
            {
                Log.Message(p + " - " + p.CurJobDef);
                if (p.CurJobDef != JobDefOf.Goto || p.CurJob.targetA.Cell.DistanceTo(thief.Position) < p.Position.DistanceTo(thief.Position))
                {
                    Log.Message("JobGiver_StealingItems : ThinkNode_JobGiver - GetCandidateToSteal - return p; - 3", true);
                    return p;
                }
            }
            return null;
        }
        public override Job TryGiveJob(Pawn pawn)
        {
            Log.Message("JobGiver_StealingItems : ThinkNode_JobGiver - TryGiveJob - var mentalState = pawn.MentalState as MentalState_Kleptomaniac; - 5", true);
            var mentalState = pawn.MentalState as MentalState_Kleptomaniac;
            Log.Message("JobGiver_StealingItems : ThinkNode_JobGiver - TryGiveJob - if (mentalState == null) - 6", true);
            if (mentalState == null)
            {
                Log.Message("JobGiver_StealingItems : ThinkNode_JobGiver - TryGiveJob - return null; - 7", true);
                return null;
            }
            var pawnsCandidates = pawn.Map.mapPawns.AllPawns.Where(x => x.RaceProps.Humanlike && x.Position.IsValid && x.FactionOrExtraMiniOrHomeFaction != pawn.Faction && !x.HostileTo(pawn)).ToList();
            Log.Message("JobGiver_StealingItems : ThinkNode_JobGiver - TryGiveJob - if (pawnsCandidates != null && pawnsCandidates.Count > 0) - 9", true);
            if (pawnsCandidates != null && pawnsCandidates.Count > 0)
            {
                Log.Message("JobGiver_StealingItems : ThinkNode_JobGiver - TryGiveJob - var victim = GetCandidateToSteal(pawnsCandidates.OrderBy(x => x.Position.DistanceTo(pawn.Position)).ToList()); - 10", true);
                var victim = GetCandidateToSteal(pawn, pawnsCandidates.OrderBy(x => x.Position.DistanceTo(pawn.Position)).ToList());
                Log.Message("JobGiver_StealingItems : ThinkNode_JobGiver - TryGiveJob - if (victim != null) - 11", true);
                if (victim != null)
                {
                    Log.Message(pawn + " trying to steal item from " + victim + " in " + victim.positionInt);
                    return JobMaker.MakeJob(VTEDefOf.VTE_StealItems, victim);
                }
            }
            Log.Message(pawn + " can't nothing steal");
            return null;
        }
    }
}

