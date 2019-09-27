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
        protected abstract string ObjectLabel { get; }

        public T GetObject<T>() where T: Def
        {
            return (T)GetObject();
        }

        public ReligionPropertyData Subject => subject;
        public ReligionPropertyData Witness => witness;

        public ReligionInfoEntry GetReligionInfoEntry()
        {
            if (GetObject() != null)
                return new ReligionInfoEntry("ReligionInfo_Object".Translate(), GetObject().LabelCap, GetDescription());
            return null;
        }

        private string GetDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(GetObject().description);
            stringBuilder.AppendLine();

            if (subject != null)
            {
                stringBuilder.AppendLine("ReligionInfo_AsSubject".Translate());
                stringBuilder.Append(subject.GetInfo());
            }
                
            if (witness != null)
            {
                stringBuilder.AppendLine("ReligionInfo_AsWitness".Translate());
                stringBuilder.Append(witness.GetInfo());
            }
            return stringBuilder.ToString();
        }

        public virtual void ExposeData()
        {
            Scribe_Deep.Look<ReligionPropertyData>(ref this.subject, "subject");
            Scribe_Deep.Look<ReligionPropertyData>(ref this.witness, "witness");
        }
    }
}
