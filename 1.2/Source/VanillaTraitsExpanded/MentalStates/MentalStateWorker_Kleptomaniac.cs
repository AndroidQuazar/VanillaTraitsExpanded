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
			//Log.Message(pawn + (pawn.HasTrait(VTEDefOf.VTE_Kleptomaniac) && pawn.Map.mapPawns.AllPawns.Where
			//	(x => !x.Dead && x.Spawned && x.Position.IsValid && x.RaceProps.Humanlike && x.FactionOrExtraMiniOrHomeFaction != pawn.Faction && !x.HostileTo(pawn)).Any()).ToString());
			return Rand.Chance(0.5f) && pawn.HasTrait(VTEDefOf.VTE_Kleptomaniac) && pawn.Map.mapPawns.AllPawns.Where
				(x => !x.Dead && x.Spawned && x.Position.IsValid && x.RaceProps.Humanlike && x.FactionOrExtraMiniOrHomeFaction != pawn.Faction && !x.HostileTo(pawn)).Any();
		}
	}
}
