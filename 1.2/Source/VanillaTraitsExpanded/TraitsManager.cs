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
        public void PreInit()
        {
            if (forcedJobs == null) forcedJobs = new Dictionary<Pawn, Job>(); 
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

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            if (Find.TickManager.TicksGame % 60 == 0)
            {
                TryInterruptForcedJobs();
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
