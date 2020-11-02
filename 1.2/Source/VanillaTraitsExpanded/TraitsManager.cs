using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
    public class TraitsManager : GameComponent
    {
		public TraitsManager()
		{
		}

		public TraitsManager(Game game)
		{

		}

        public Dictionary<Pawn, Job> forcedJobs = new Dictionary<Pawn, Job>();
        public HashSet<Pawn> perfectionistsWithJobsToStop = new HashSet<Pawn>();
        public HashSet<Pawn> cowards = new HashSet<Pawn>();
        public HashSet<Pawn> bigBoned = new HashSet<Pawn>();
        public HashSet<Pawn> rebels = new HashSet<Pawn>();
        public HashSet<Pawn> snobs = new HashSet<Pawn>();
        public Dictionary<Pawn, int> madSurgeonsWithLastHarvestedTick = new Dictionary<Pawn, int>();
        public Dictionary<Pawn, int> wanderLustersWithLastMapExitedTick = new Dictionary<Pawn, int>();

        public void PreInit()
        {
            if (forcedJobs == null) forcedJobs = new Dictionary<Pawn, Job>();
            if (perfectionistsWithJobsToStop == null) perfectionistsWithJobsToStop = new HashSet<Pawn>();
            if (cowards == null) cowards = new HashSet<Pawn>();
            if (rebels == null) rebels = new HashSet<Pawn>();
            if (bigBoned == null) bigBoned = new HashSet<Pawn>();
            if (snobs == null) snobs = new HashSet<Pawn>();
            if (madSurgeonsWithLastHarvestedTick == null) madSurgeonsWithLastHarvestedTick = new Dictionary<Pawn, int>();
            if (wanderLustersWithLastMapExitedTick == null) wanderLustersWithLastMapExitedTick = new Dictionary<Pawn, int>();

        }
        public override void StartedNewGame()
        {
            PreInit();
            base.StartedNewGame();
        }

        public override void LoadedGame()
        {
            PreInit();
            base.LoadedGame();
        }
        public void TryInterruptForcedJobs()
        {
            var keysToRemove = new List<Pawn>();
            foreach (var data in forcedJobs)
            {
                if (data.Key.Map != null)
                {
                    if (data.Key.CurJob == data.Value)
                    {
                        if (Rand.Chance(0.5f))
                        {
                            Log.Message(data.Key + " - stops forced " + data.Key.CurJob + " due to absent-minded trait");
                            Messages.Message("VTE.PawnStopsForcedJob".Translate(data.Key.Named("PAWN")), data.Key, MessageTypeDefOf.NeutralEvent, historical: false);
                            data.Key.jobs.StopAll();
                        }
                    }
                    else
                    {
                        keysToRemove.Add(data.Key);
                    }
                }
            }
            foreach (var key in keysToRemove)
            {
                forcedJobs.Remove(key);
            }
        }

        public void TryForceFleeCowards()
        {
            foreach (var pawn in cowards)
            {
                if (pawn.Map != null && Rand.Chance(0.1f))
                {
                    Log.Message(" - TryForceFleeCowards - var enemies = pawn.Map.attackTargetsCache?.GetPotentialTargetsFor(pawn)?.Where(x => x.Thing.Position.DistanceTo(pawn.Position) < 15f)?.Select(y => y.Thing); - 3", true);
                    var enemies = pawn.Map.attackTargetsCache?.GetPotentialTargetsFor(pawn)?.Where(x => x.Thing.Position.DistanceTo(pawn.Position) < 15f)?.Select(y => y.Thing);
                    Log.Message(" - TryForceFleeCowards - if (enemies?.Count() > 0) - 4", true);
                    if (enemies?.Count() > 0)
                    {
                        TraitUtils.MakeFlee(pawn, enemies.OrderBy(x => x.Position.DistanceTo(pawn.Position)).First(), 15, enemies.ToList());
                        Messages.Message("VTE.PawnCowardlyFlees".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                    }
                }
            }
        }
        public void TryBreakChairsUnderBigBoneds()
        {
            foreach (var pawn in bigBoned)
            {
                if (pawn.Map != null && Rand.Chance(0.1f))
                {
                    var firstBuilding = pawn.Position.GetFirstBuilding(pawn.Map);
                    if (firstBuilding?.def?.building?.isSittable ?? false && !(firstBuilding is Building_Throne))
                    {
                        if (pawn.CurJobDef == JobDefOf.Ingest)
                        {
                            firstBuilding.TakeDamage(new DamageInfo(DamageDefOf.Crush, (60f * firstBuilding.MaxHitPoints) / 100f));
                            pawn.jobs.StopAll();
                            Messages.Message("VTE.PawnBreaksChairs".Translate(pawn.Named("PAWN"), firstBuilding.Label), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                        }
                        else if (pawn.CurJobDef == VTEDefOf.WatchTelevision && (pawn.Position.GetFirstBuilding(pawn.Map)?.def?.building?.isSittable ?? false))
                        {
                            var chairs = pawn.Position.GetFirstBuilding(pawn.Map);
                            Messages.Message("VTE.PawnBreaksChairs".Translate(pawn.Named("PAWN"), chairs.Label), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                            firstBuilding.TakeDamage(new DamageInfo(DamageDefOf.Crush, (60f * firstBuilding.MaxHitPoints) / 100f));
                            pawn.jobs.StopAll();
                        }
                    }
                    else if (pawn.jobs.curDriver is JobDriver_SitFacingBuilding && pawn.CurJob?.targetB.Thing != null && !(pawn.CurJob?.targetB.Thing is Building_Throne))
                    {
                        Messages.Message("VTE.PawnBreaksChairs".Translate(pawn.Named("PAWN"), pawn.CurJob.targetB.Thing.Label), pawn, MessageTypeDefOf.NeutralEvent, historical: false);
                        pawn.CurJob.targetB.Thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, (60f * firstBuilding.MaxHitPoints) / 100f));
                        pawn.jobs.StopAll();
                    }
                }
            }
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            if (Find.TickManager.TicksGame % 60 == 0)
            {
                TryInterruptForcedJobs();
                TryForceFleeCowards();
            }
            else if (Find.TickManager.TicksGame % 100 == 0)
            {
                TryBreakChairsUnderBigBoneds();
            }
            if (perfectionistsWithJobsToStop.Count > 0)
            {
                foreach (var pawn in perfectionistsWithJobsToStop)
                {
                    pawn.jobs.StopAll();
                    if (pawn.HasTrait(VTEDefOf.VTE_Perfectionist))
                    {
                        pawn.TryGiveThought(VTEDefOf.VTE_CouldNotFinishItem);
                    }
                }
                perfectionistsWithJobsToStop.Clear();
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                ClearListsFromNulls();
            }
            Scribe_Collections.Look(ref forcedJobs, "forcedJobs", LookMode.Reference, LookMode.Reference, ref pawnKeys, ref jobValues);
            Scribe_Collections.Look(ref madSurgeonsWithLastHarvestedTick, "madSurgeonsWithLastHarvestedTick", LookMode.Reference, LookMode.Value, ref pawnKeys2, ref tickValues);
            Scribe_Collections.Look(ref wanderLustersWithLastMapExitedTick, "wanderLustersWithLastMapExitedTick", LookMode.Reference, LookMode.Value, ref pawnKeys3, ref tickValues1);
            Scribe_Collections.Look(ref rebels, "rebels", LookMode.Reference);
            Scribe_Collections.Look(ref perfectionistsWithJobsToStop, "perfectionistsWithJobsToStop", LookMode.Reference);
            Scribe_Collections.Look(ref cowards, "cowards", LookMode.Reference);
            Scribe_Collections.Look(ref snobs, "snobs", LookMode.Reference);
            Scribe_Collections.Look(ref bigBoned, "bigBoned", LookMode.Reference);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                ClearListsFromNulls();
            }
        }

        public void ClearListsFromNulls()
        {
            forcedJobs.RemoveAll(x => x.Key == null);
            madSurgeonsWithLastHarvestedTick.RemoveAll(x => x.Key == null);
            wanderLustersWithLastMapExitedTick.RemoveAll(x => x.Key == null);
            rebels.RemoveWhere(x => x == null);
            perfectionistsWithJobsToStop.RemoveWhere(x => x == null);
            cowards.RemoveWhere(x => x == null);
            snobs.RemoveWhere(x => x == null);
            bigBoned.RemoveWhere(x => x == null);
        }

        private List<Pawn> pawnKeys = new List<Pawn>();
        private List<Job> jobValues = new List<Job>();


        private List<Pawn> pawnKeys2 = new List<Pawn>();
        private List<int> tickValues = new List<int>();

        private List<Pawn> pawnKeys3 = new List<Pawn>();
        private List<int> tickValues1 = new List<int>();
    }
}
