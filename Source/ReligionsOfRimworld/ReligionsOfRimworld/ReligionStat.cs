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
        protected PietyDef individualPiety;
        protected PietyDef socialPiety;
        protected ThoughtDef individualThought;
        protected ThoughtDef socialThought;
        protected bool onlyForPlayerColony;

        public PietyDef IndividualPiety => individualPiety;
        public PietyDef SocialPiety => socialPiety;
        public ThoughtDef IndividualThought => individualThought;
        public ThoughtDef SocialThought => socialThought;
        public bool OnlyForPlayerColony => onlyForPlayerColony;

        public abstract Def GetObject();

        public IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            yield return new ReligionInfoEntry("");
            if (GetObject() != null)
                yield return new ReligionInfoEntry("ReligionInfo_Object".Translate(), GetObject().LabelCap, GetObject().description);
            if (individualPiety != null)
                yield return new ReligionInfoEntry("ReligionInfo_IndividualPiety".Translate(), "", PietyDefExplanation(individualPiety));
            if (socialPiety != null)
                yield return new ReligionInfoEntry("ReligionInfo_SocialPiety".Translate(), "", PietyDefExplanation(socialPiety));
            if (individualThought != null)
                yield return new ReligionInfoEntry("ReligionInfo_IndividualThought".Translate(), "", ThoughtDefExplanation(individualThought));
            if (socialThought != null)
                yield return new ReligionInfoEntry("ReligionInfo_SocialThought".Translate(), "", ThoughtDefExplanation(socialThought));
        }

        private string PietyDefExplanation(PietyDef def)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (def.IsSituational)
                stringBuilder.AppendLine("ReligionInfo_PietySituational".Translate());
            if(def.DurationDays != 0)
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

        public virtual void ExposeData()
        {
            Scribe_Defs.Look<PietyDef>(ref this.individualPiety, "individualPiety");
            Scribe_Defs.Look<PietyDef>(ref this.socialPiety, "socialPiety");
            Scribe_Defs.Look<ThoughtDef>(ref this.individualThought, "individualThought");
            Scribe_Defs.Look<ThoughtDef>(ref this.socialThought, "socialThought");
            Scribe_Values.Look<bool>(ref this.onlyForPlayerColony, "onlyForPlayerColony");
        }
    }
}
