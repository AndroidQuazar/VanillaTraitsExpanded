using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class MentalState_TechnophobeTantrum : MentalState_TantrumRandom
	{
		public override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, outThings, GetCustomValidator());
		}
		public override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => x.def.techLevel > RimWorld.TechLevel.Medieval;
		}
	}
}
