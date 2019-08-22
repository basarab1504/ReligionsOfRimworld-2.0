using RimWorld;
using System.Collections.Generic;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_ReligionTalks : ReligionSettings
    {
        private InteractionDef interaction;
        private float baseChanceOfConversation = 0.05f;
        private SimpleCurveDef moodFactorCurve;
        private SimpleCurveDef opinionFactorCurve;

        public InteractionDef Interaction => interaction;
        public float BaseChanceOfConversation => baseChanceOfConversation;
        public SimpleCurveDef MoodFactorCurve => moodFactorCurve;
        public SimpleCurveDef OpinionFactorCurve => opinionFactorCurve;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            if (interaction != null)
                yield return new ReligionInfoEntry("ReligionInfo_BaseChanceOfConversation".Translate(), baseChanceOfConversation.ToString());
            if (moodFactorCurve != null)
                yield return new ReligionInfoEntry("ReligionInfo_MoodFactorCurve".Translate(), "", moodFactorCurve.description);
            if (opinionFactorCurve != null)
                yield return new ReligionInfoEntry("ReligionInfo_OpinionFactorCurve".Translate(), "", opinionFactorCurve.description);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<InteractionDef>(ref this.interaction, "interaction");
            Scribe_Values.Look<float>(ref baseChanceOfConversation, "baseChanceOfConversation");
            Scribe_Defs.Look<SimpleCurveDef>(ref this.moodFactorCurve, "moodFactorCurve");
            Scribe_Defs.Look<SimpleCurveDef>(ref this.opinionFactorCurve, "opinionFactorCurve");
        }
    }
}
