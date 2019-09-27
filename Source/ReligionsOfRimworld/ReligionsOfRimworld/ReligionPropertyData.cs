using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace ReligionsOfRimworld
{
    public class ReligionPropertyData : IExposable
    {
        private PropertyPawnCategory pawnCategory = PropertyPawnCategory.Everyone;
        private ThoughtDef thought;
        private ThoughtDef opinionThought;
        private PietyDef piety;

        public PropertyPawnCategory PawnCategory => pawnCategory;
        public ThoughtDef Thought => thought;
        public ThoughtDef OpinionThought => opinionThought;
        public PietyDef Piety => piety;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("ReligionInfo_PawnCategory".Translate((NamedArgument)PawnCategory.ToString()));
            stringBuilder.AppendLine();

            if (thought != null)
                stringBuilder.Append(GetDefInfo("ReligionInfo_IndividualThought".Translate(), thought));
            if (opinionThought != null)
                stringBuilder.Append(GetDefInfo("ReligionInfo_SocialThought".Translate(), opinionThought));
            if (piety != null)
                stringBuilder.Append(GetDefInfo("ReligionInfo_IndividualPiety".Translate(), piety));

            return stringBuilder.ToString();
        }

        private string GetDefInfo(string defType, Def def)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(defType + ":");
            stringBuilder.AppendLine(def.LabelCap);
            stringBuilder.AppendLine(def.description);
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

        public void ExposeData()
        {
            Scribe_Values.Look<PropertyPawnCategory>(ref this.pawnCategory, "pawnCategory");
            Scribe_Defs.Look<ThoughtDef>(ref this.thought, "thought");
            Scribe_Defs.Look<ThoughtDef>(ref this.opinionThought, "opinionThought");
            Scribe_Defs.Look<PietyDef>(ref this.piety, "piety");
        }
    }
}
