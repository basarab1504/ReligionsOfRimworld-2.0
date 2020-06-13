using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReligionsOfRimworld
{
    public class PietyWorker_ColdTemperature : PietyWorker
    {
        public override PietyState CurrentState(Pawn p)
        {
            CompReligion comp = p.GetReligionComponent();

            if (comp == null)
                return PietyState.Inactive;

            ReligionSettings_Social settings = comp.Religion.GetSettings<ReligionSettings_Social>(SettingsTagDefOf.TemperatureTag);

            if (settings == null)
                return PietyState.Inactive;

            ReligionProperty property = settings.DefaultPropety;

            if (property == null || property.Subject == null || property.Subject.Piety == null || property.Subject.Piety != this.def)
                return PietyState.Inactive;

            float num = p.AmbientTemperature;
            if ((double)num >= 0.0)
                return PietyState.Inactive;
            if ((double)num > -10.0)
                return PietyState.ActiveAtStage(0);
            if ((double)num > -20.0)
                return PietyState.ActiveAtStage(1);
            if ((double)num > -30.0)
                return PietyState.ActiveAtStage(2);
            if ((double)num > -40.0)
                return PietyState.ActiveAtStage(3);
            return PietyState.ActiveAtStage(4);
        }
    }
}
