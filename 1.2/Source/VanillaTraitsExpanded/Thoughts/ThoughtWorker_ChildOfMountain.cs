using RimWorld;
using System.Collections.Generic;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_ChildOfMountain : ThoughtWorker
	{
		public bool HasEnoughMountain(Map map)
        {
			int num = 0;
			foreach (var cell in map.AllCells)
            {
				if (map.roofGrid.RoofAt(cell) == RoofDefOf.RoofRockThick)
                {
					num++;
                }
				if (num >= 100)
                {
					return true;
                }
            }
			return false;
        } 
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.HasTrait(VTEDefOf.VTE_ChildOfSea))
            {
				if (p.Map != null && HasEnoughMountain(p.Map))
                {
					return ThoughtState.ActiveDefault;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
