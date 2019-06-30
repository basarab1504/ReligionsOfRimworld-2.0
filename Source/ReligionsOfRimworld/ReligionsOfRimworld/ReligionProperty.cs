using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public abstract class ReligionProperty : IExposable
    {
        protected ReligionPropertyData subject;
        protected ReligionPropertyData witness;

        public abstract Def GetObject();

        public ReligionPropertyData Subject => subject;
        public ReligionPropertyData Witness => witness;

        public IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            yield return new ReligionInfoEntry("");
            if (GetObject() != null)
                yield return new ReligionInfoEntry("ReligionInfo_Object".Translate(), GetObject().LabelCap, GetObject().description);

            if (subject != null)
                foreach(ReligionInfoEntry entry in subject.GetInfoEntries())
                yield return entry;

            if (witness != null)
                foreach (ReligionInfoEntry entry in witness.GetInfoEntries())
                    yield return entry;
        }

        public virtual void ExposeData()
        {
            Scribe_Deep.Look<ReligionPropertyData>(ref this.subject, "subject");
            Scribe_Deep.Look<ReligionPropertyData>(ref this.witness, "witness");
        }
    }
}
