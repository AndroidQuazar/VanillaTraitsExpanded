using RimWorld;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_AnimalsInColony : ThoughtWorker
	{
		public override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Menagerist))
            {
				Map map = null;
				if (p.ownership?.OwnedBed?.Map != null)
				{
					map = p.ownership.OwnedBed.Map;
				}
				else
				{
					map = p.Map;
				}
				if (map != null)
				{
					var animalCount = p.Map.mapPawns.AllPawns.Where(x => x.RaceProps.Animal && x.Faction == p.Faction).Count();
					if (animalCount <= 15)
					{
						return ThoughtState.ActiveAtStage(animalCount);
					}
					else
					{
						return ThoughtState.ActiveAtStage(15);
					}
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
