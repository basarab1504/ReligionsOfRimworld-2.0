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
        private ThoughtDef thought;
        private ThoughtDef opinionThought;
        private PietyDef piety;

        public ThoughtDef Thought => thought;
        public ThoughtDef OpinionThought => opinionThought;
        public PietyDef Piety => piety;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (thought != null)
            {
                stringBuilder.Append("\t" + "ReligionInfo_IndividualThought".Translate() + ": ");
                stringBuilder.Append($" {thought.LabelCap} ");
                stringBuilder.Append($"({thought.stages.First().baseMoodEffect}...{thought.stages.Last().baseMoodEffect})");
                stringBuilder.AppendLine();
            }
            if (opinionThought != null)
            {
                stringBuilder.Append("\t" + "ReligionInfo_SocialThought".Translate() + ": ");
                stringBuilder.Append($" {opinionThought.LabelCap} ");
                stringBuilder.Append($"({opinionThought.stages.First().baseOpinionOffset}...{opinionThought.stages.Last().baseOpinionOffset})");
                stringBuilder.AppendLine();

            }
            if (piety != null)
            {
                stringBuilder.Append("\t" + "ReligionInfo_IndividualPiety".Translate() + ": ");
                stringBuilder.Append($" {piety.LabelCap} ");
                stringBuilder.Append($"({piety.Stages.First().PietyOffset}...{piety.Stages.Last().PietyOffset})");
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public void ExposeData()
        {
            Scribe_Defs.Look<ThoughtDef>(ref this.thought, "thought");
            Scribe_Defs.Look<ThoughtDef>(ref this.opinionThought, "opinionThought");
            Scribe_Defs.Look<PietyDef>(ref this.piety, "piety");
        }
    }
}
