using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugsLib;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionsOfRimworldModBase : ModBase
    {
        public override string ModIdentifier
        {
            get
            {
                return "Religion";
            }
        }

        public override void DefsLoaded()
        {
            if (!this.ModIsActive)
                return;
            IEnumerable<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefs.Where<ThingDef>((Func<ThingDef, bool>)(def =>
            {
                RaceProperties race = def.race;
                if ((race != null ? (race.intelligence == Intelligence.Humanlike ? 1 : 0) : 0) == 0 || def.defName.Contains("AIPawn") || (def.defName.Contains("Android") || def.defName.Contains("Robot")))
                    return false;
                return true;
            }));
            foreach (ThingDef thingDef in thingDefs)
            {
                if (thingDef.inspectorTabsResolved == null)
                    thingDef.inspectorTabsResolved = new List<InspectTabBase>(1);
                thingDef.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_Pawn_Faith)));
                if (thingDef.comps == null)
                    thingDef.comps = new List<CompProperties>(1);
                thingDef.comps.Add((CompProperties)new CompProperties_FaithComp());
            }
        }
    }
}
