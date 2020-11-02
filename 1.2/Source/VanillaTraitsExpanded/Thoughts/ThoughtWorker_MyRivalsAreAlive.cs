using RimWorld;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_MyRivalsAreAlive : ThoughtWorker
	{
		public bool RivalsAreAlive(Pawn p)
        {
			foreach (var pawn in p.relations.PotentiallyRelatedPawns)
            {
				if (pawn.RaceProps.IsFlesh)
				{
					int num = pawn.relations.OpinionOf(p);
					if (num <= -20 && !pawn.Dead)
					{
						return true;
					}
				}
			}
			return false;
        }
		public override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Vengeful) && RivalsAreAlive(p))
            {
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}
	}
}
