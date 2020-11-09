using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
	public class JobDriver_StealItems : JobDriver
	{
		private Pawn Victim => (Pawn)job.GetTarget(TargetIndex.A).Thing;
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}
		public override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil gotoVictim = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			yield return gotoVictim;
			yield return Toils_General.Wait(30).WithProgressBarToilDelay(TargetIndex.A).FailOnDespawnedOrNull(TargetIndex.A);
			yield return DoStealing();
		}

		private Toil DoStealing()
		{
			return new Toil
			{
				initAction = delegate
				{
					var mostValuableItem = Victim.inventory?.innerContainer?.InnerListForReading?.OrderByDescending(x => x.MarketValue).FirstOrDefault();
					if (mostValuableItem != null)
                    {
						if (Rand.Chance(0.5f))
                        {
							if (Victim.inventory.innerContainer.TryTransferToContainer(mostValuableItem, this.pawn.inventory.innerContainer))
                            {
								Messages.Message("VTE.PawnStoleItem".Translate(mostValuableItem.Label, pawn.Named("PAWN"), Victim.Named("VICTIM")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
								if (Rand.Chance(0.5f))
								{
									Victim.Faction.TryAffectGoodwillWith(pawn.Faction, -5, reason: "VTE.KleptomaniacStealsItemFrom".Translate(pawn.Named("PAWN"), Victim.Named("VICTIM")));
								}
							}
						}
						else
                        {
							Messages.Message("VTE.PawnFailedToStealItem".Translate(mostValuableItem.Label, pawn.Named("PAWN"), Victim.Named("VICTIM")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
							Victim.Faction.TryAffectGoodwillWith(pawn.Faction, -5, reason: "VTE.KleptomaniacFailedToStealItemFrom".Translate(pawn.Named("PAWN"), Victim.Named("VICTIM")));
						}
					}
				},
				atomicWithPrevious = true
			};
		}
	}
}
