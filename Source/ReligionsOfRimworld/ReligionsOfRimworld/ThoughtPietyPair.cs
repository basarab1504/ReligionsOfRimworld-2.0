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

        public IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            if (thought != null)
                yield return new ReligionInfoEntry("ReligionInfo_IndividualThought".Translate(), "", ThoughtDefExplanation(thought));
            if (opinionThought != null)
                yield return new ReligionInfoEntry("ReligionInfo_SocialThought".Translate(), "", ThoughtDefExplanation(opinionThought));
            if (piety != null)
                yield return new ReligionInfoEntry("ReligionInfo_IndividualPiety".Translate(), "", PietyDefExplanation(piety));
        }

        private string PietyDefExplanation(PietyDef def)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (def.IsSituational)
                stringBuilder.AppendLine("ReligionInfo_PietySituational".Translate());
            if (def.DurationDays != 0)
                stringBuilder.AppendLine("ReligionInfo_DurationDays".Translate() + ": " + def.DurationDays);
            if (def.Stages.Count() != 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("ReligionInfo_Stages".Translate() + ":");
            }
            int i = 1;
            foreach (PietyStage stage in def.Stages)
            {
                stringBuilder.AppendLine("ReligionInfo_Stage".Translate() + ": " + i.ToString());
                stringBuilder.AppendLine(stage.Label);
                //stringBuilder.AppendLine(stage.Description); 
                stringBuilder.AppendLine("ReligionInfo_PietyOffset".Translate() + " " + stage.PietyOffset.ToString());
                stringBuilder.AppendLine("ReligionInfo_MultiplierValue".Translate() + " " + stage.MultiplierValue.ToString());
                stringBuilder.AppendLine("ReligionInfo_PietyRate".Translate() + " " + stage.PietyRate.ToString());
                stringBuilder.AppendLine();
                i++;
            }
            return stringBuilder.ToString();
        }

        private string ThoughtDefExplanation(ThoughtDef def)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (def.IsSituational)
                stringBuilder.AppendLine("ReligionInfo_PietySituational".Translate());
            if (def.durationDays != 0)
                stringBuilder.AppendLine("ReligionInfo_DurationDays".Translate() + ": " + def.durationDays);
            if (def.stages.Count != 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("ReligionInfo_Stages".Translate() + ":");
            }
            int i = 1;
            foreach (ThoughtStage stage in def.stages)
            {
                stringBuilder.AppendLine("ReligionInfo_Stage".Translate() + ": " + i.ToString());
                stringBuilder.AppendLine(stage.label);
                //stringBuilder.AppendLine(stage.description);
                stringBuilder.AppendLine("ReligionInfo_BaseMoodEffect".Translate() + " " + stage.baseMoodEffect.ToString());
                stringBuilder.AppendLine("ReligionInfo_BaseOpinionOffset".Translate() + " " + stage.baseOpinionOffset.ToString());
                stringBuilder.AppendLine();
                i++;
            }
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
