using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class MentalStateWorker_Kleptomaniac : MentalStateWorker
	{
		public override bool StateCanOccur(Pawn pawn)
		{
			return pawn.Map.mapPawns.AllPawns.Where(x => x.RaceProps.Humanlike && x.FactionOrExtraMiniOrHomeFaction != pawn.Faction && !x.HostileTo(pawn)).Any();
		}
	}
}
