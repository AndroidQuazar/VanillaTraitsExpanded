using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Verse;

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


        private static TraitsManager tManager;
    }
}

