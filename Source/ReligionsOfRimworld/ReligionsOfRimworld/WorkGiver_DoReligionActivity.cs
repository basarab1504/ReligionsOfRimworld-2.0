using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class WorkGiver_DoReligionActivity : WorkGiver_DoBill
    {
        public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            if (thing is Building_ReligiousBuildingFacility facility)
            {
                if (facility.AssignedReligion == null)
                    return (Job)null;

                if (this.def.fixedBillGiverDefs == null)
                    this.def.fixedBillGiverDefs = new List<ThingDef>();

                if (!this.def.fixedBillGiverDefs.Contains(thing.def))
                    this.def.fixedBillGiverDefs.Add(thing.def);

                if (base.JobOnThing(pawn, thing, forced) != null)
                {
                    return null;
                }
            }
            return (Job)null;
        }
    }
}
