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
        public void PreInit()
        {
            if (forcedJobs == null) forcedJobs = new Dictionary<Pawn, Job>();
            if (perfectionistsWithJobsToStop == null) perfectionistsWithJobsToStop = new HashSet<Pawn>();
            if (cowards == null) cowards = new HashSet<Pawn>();
        }
        public override void StartedNewGame()
        {
            base.StartedNewGame();
            PreInit();
        }

        public override void LoadedGame()
        {
            base.LoadedGame();
            PreInit();
        }
        public void TryInterruptForcedJobs()
        {
            var keysToRemove = new List<Pawn>();
            foreach (var data in forcedJobs)
            {
                if (data.Key.CurJob == data.Value)
                {
                    if (Rand.Chance(0.5f))
                    {
                        Log.Message(data.Key + " - stops forced " + data.Key.CurJob + " due to absent-minded trait");
                        data.Key.jobs.StopAll();
                    }
                }
                else
                {
                    keysToRemove.Add(data.Key);
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
                if (Rand.Chance(0.1f))
                {
                    var enemies = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn).Where(x => x.Thing.Position.DistanceTo(pawn.Position) < 15f).Select(y => y.Thing);
                    foreach (var e in enemies)
                    {
                        Log.Message("Enemy: " + e);
                    }
                    if (enemies?.Count() > 0)
                    {
                        TraitUtils.MakeFlee(pawn, enemies.OrderBy(x => x.Position.DistanceTo(pawn.Position)).First(), 15, enemies.ToList());
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
            Scribe_Collections.Look(ref forcedJobs, "forcedJobs", LookMode.Reference, LookMode.Reference, ref pawnKeys, ref jobValues);
        }

        private List<Pawn> pawnKeys = new List<Pawn>();
        private List<Job> jobValues = new List<Job>();
    }
}
