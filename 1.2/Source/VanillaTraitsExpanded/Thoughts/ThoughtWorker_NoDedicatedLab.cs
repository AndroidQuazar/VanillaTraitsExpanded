using RimWorld;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_NoDedicatedLab : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_Prodigy))
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
				var labRoom = map.regionGrid.allRooms.Where(x => x.Role == RoomRoleDefOf.Laboratory).FirstOrDefault();
				if (labRoom == null)
                {
					return ThoughtState.ActiveDefault;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
