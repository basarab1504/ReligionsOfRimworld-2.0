using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class InteractionWorker_ReligionTalks : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            ReligionSettings_ReligionTalks settings = initiator.GetReligionComponent().Religion.ReligionTalksSettings;
            if (settings != null && settings.Interaction == this.interaction)
                return settings.BaseChanceOfConversation;
            return 0;
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
        {
            base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef);
            CompReligion compReligion = initiator.GetReligionComponent();
            float successChance = ChanceToConvert(initiator, recipient);
            if(new Random().NextDouble() <= successChance)
            {
                letterText = recipient.ToString() + " " + "Religion_NowBelieveIn".Translate() + " " + compReligion.Religion.Label;
                letterLabel = "Religion_IsNowReligious".Translate();
                letterDef = LetterDefOf.NeutralEvent;
            }
        }

        private float ChanceToConvert(Pawn initiator, Pawn recipient)
        {
            CompReligion initiatorComp = initiator.GetReligionComponent();
            CompReligion recipientComp = recipient.GetReligionComponent();
            ReligionSettings_ReligionTalks settings = initiatorComp.Religion.ReligionTalksSettings;
            float opinionFactor = settings.OpinionFactorCurve.Curve != null ? settings.OpinionFactorCurve.Curve.Evaluate((float)initiator.relations.OpinionOf(recipient)) : 1f;
            float moodFactor = settings.MoodFactorCurve.Curve != null ? settings.MoodFactorCurve.Curve.Evaluate((float)recipient.needs.mood.CurLevel) : 1f;
            float spouseRelationChanceFactor = settings.SpouseRelationChanceFactor;
            //float compabilityFactor = recipientComp.ReligionCompability.CompabilityFor(initiatorComp.Religion);
            return 1f * opinionFactor * moodFactor * spouseRelationChanceFactor /** compabilityFactor*/;
        }
    }
}
