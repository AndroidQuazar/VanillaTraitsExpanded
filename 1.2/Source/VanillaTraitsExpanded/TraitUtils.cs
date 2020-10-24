using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaTraitsExpanded
{
    [StaticConstructorOnStartup]
    internal static class TraitUtils
    {
        public static TraitsManager TraitsManager
        {
            get
            {
                if (tManager == null)
                {
                    tManager = Current.Game.GetComponent<TraitsManager>();
                    return tManager;
                }
                return tManager;
            }
        }

        public static bool HasTrait(this Pawn pawn, TraitDef traitDef)
        {
            if (pawn.story?.traits?.HasTrait(traitDef) ?? false)
            {
                return true;
            }
            return false;
        }

        public static void TryGiveThought(this Pawn pawn, ThoughtDef thoughtDef)
        {
            pawn.needs?.mood?.thoughts.memories.TryGainMemory(thoughtDef);
        }

        public static void MakeFlee(Pawn pawn, Thing danger, int radius, List<Thing> dangers)
        {
            Job job = null;
            IntVec3 intVec;
            if (pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Flee)
            {
                intVec = pawn.CurJob.targetA.Cell;
            }
            else
            {
                intVec = CellFinderLoose.GetFleeDest(pawn, dangers, 24f);
            }

            if (intVec == pawn.Position)
            {
                intVec = GenRadial.RadialCellsAround(pawn.Position, radius, radius * 2).RandomElement();
            }
            if (intVec != pawn.Position)
            {
                job = JobMaker.MakeJob(JobDefOf.Flee, intVec, danger);
            }
            if (job != null)
            {
                Log.Message(pawn + " flee");
                pawn.jobs.TryTakeOrderedJob(job);
            }
        }

        private static TraitsManager tManager;
    }
}

