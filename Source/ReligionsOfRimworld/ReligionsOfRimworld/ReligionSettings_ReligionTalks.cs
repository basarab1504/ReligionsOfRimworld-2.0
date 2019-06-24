using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_ReligionTalks : ReligionSettings
    {
        private InteractionDef interaction /*= ReligionDefOf.ReligionTalk*/;
        private float baseChanceOfConversation = 0.05f;
        //private float chanceToConvert = 1f;
        private float spouseRelationChanceFactor = 1f;
        private SimpleCurveDef moodFactorCurve;
        private SimpleCurveDef opinionFactorCurve;

        public InteractionDef Interaction => interaction;
        public float BaseChanceOfConversation => baseChanceOfConversation;
        //public float ChanceToConvert => chanceToConvert;
        public float SpouseRelationChanceFactor => spouseRelationChanceFactor;
        public SimpleCurveDef MoodFactorCurve => moodFactorCurve;
        public SimpleCurveDef OpinionFactorCurve => opinionFactorCurve;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            return null;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<InteractionDef>(ref this.interaction, "interaction");
            Scribe_Values.Look<float>(ref baseChanceOfConversation, "baseChanceOfConversation");
            Scribe_Values.Look<float>(ref spouseRelationChanceFactor, "spouseRelationChanceFactor");
            Scribe_Defs.Look<SimpleCurveDef>(ref this.moodFactorCurve, "moodFactorCurve");
            Scribe_Defs.Look<SimpleCurveDef>(ref this.opinionFactorCurve, "opinionFactorCurve");
        }
    }
}
