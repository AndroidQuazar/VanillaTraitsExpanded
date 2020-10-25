using RimWorld;
using System.Collections.Generic;
using System.Linq;
using VanillaTraitsExpanded;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class ThoughtWorker_ChildOfSea : ThoughtWorker
	{

		public static List<TerrainDef> SeaTerrain = new List<TerrainDef>()
		{
			TerrainDefOf.WaterMovingChestDeep,
			TerrainDefOf.WaterMovingShallow,
			TerrainDefOf.WaterOceanDeep,
			TerrainDefOf.WaterOceanShallow,
		};
		public bool HasEnoughSea(Map map)
        {
			int num = 0;
			foreach (var terrain in map.terrainGrid.topGrid)
            {
				if (SeaTerrain.Contains(terrain))
                {
					num++;
                }
				if (num >= 40)
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
				if (p.Map != null && HasEnoughSea(p.Map))
                {
					return ThoughtState.ActiveDefault;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
