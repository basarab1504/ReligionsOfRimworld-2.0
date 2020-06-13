using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReligionsOfRimworld
{
    public class ThoughtWorker_ColdTemperature : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Log.Message(p.ToString() + " " + p.AmbientTemperature);
            CompReligion comp = p.GetReligionComponent();

            if (comp == null)
                return ThoughtState.Inactive;

            ReligionSettings_Social settings = comp.Religion.GetSettings<ReligionSettings_Social>(SettingsTagDefOf.TemperatureTag);

            if (settings == null)
                return ThoughtState.Inactive;

            ReligionProperty property = settings.DefaultPropety;

            if (property == null || property.Subject == null || property.Subject.Piety == null || property.Subject.Thought != this.def)
                return (ThoughtState)false;

            float num = p.AmbientTemperature;
            if ((double)num >= 0.0)
                return ThoughtState.Inactive;
            if ((double)num > -10.0)
                return ThoughtState.ActiveAtStage(0);
            if ((double)num > -20.0)
                return ThoughtState.ActiveAtStage(1);
            if ((double)num > -30.0)
                return ThoughtState.ActiveAtStage(2);
            if ((double)num > -40.0)
                return ThoughtState.ActiveAtStage(3);
            return ThoughtState.ActiveAtStage(4);
        }
    }
}
