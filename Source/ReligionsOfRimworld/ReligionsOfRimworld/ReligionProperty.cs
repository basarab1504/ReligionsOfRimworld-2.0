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
        protected PropertyPawnCategory pawnCategory = PropertyPawnCategory.Everyone;

        public abstract Def GetObject();
        public PropertyPawnCategory PawnCategory => pawnCategory;

        protected abstract string ObjectLabel { get; }
        protected abstract string Description { get; }

        public virtual bool Contains(Def def)
        {
            return GetObject() == def;
        }

        public T GetObject<T>() where T: Def
        {
            return (T)GetObject();
        }

        public ReligionPropertyData Subject => subject;
        public ReligionPropertyData Witness => witness;

        public ReligionInfoEntry GetReligionInfoEntry()
        {
            if (GetObject() != null)
                return new ReligionInfoEntry("ReligionInfo_Object".Translate(), ObjectLabel, GetDescription());

            return null;
        }

        private string GetDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("ReligionInfo_PawnCategory".Translate((NamedArgument)pawnCategory.ToString()));
            stringBuilder.AppendLine();

            if (Description != null)
            {
                stringBuilder.AppendLine(Description);
                stringBuilder.AppendLine();
            }

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
            Scribe_Values.Look<PropertyPawnCategory>(ref this.pawnCategory, "pawnCategory");
        }
    }
}
