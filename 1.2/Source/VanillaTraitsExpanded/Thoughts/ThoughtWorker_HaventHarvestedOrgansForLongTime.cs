using RimWorld;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_HaventHarvestedOrgansForLongTime : ThoughtWorker
	{
		public override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_MadSurgeon))
            {
				if (TraitUtils.TraitsManager.madSurgeonsWithLastHarvestedTick.ContainsKey(p))
                {
					var lastTick = TraitUtils.TraitsManager.madSurgeonsWithLastHarvestedTick[p];
					if ((Find.TickManager.TicksAbs - lastTick) > 10 * GenDate.TicksPerDay)
                    {
						return ThoughtState.ActiveDefault;
                    }
				}
				else
                {
					TraitUtils.TraitsManager.madSurgeonsWithLastHarvestedTick[p] = Find.TickManager.TicksAbs;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
