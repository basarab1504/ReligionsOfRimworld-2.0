using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ReligionsOfRimworld
{
    public class ThoughtWorker_NeedPiety : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            CompReligion comp = p.GetReligionComponent();
            if (comp == null)
                return ThoughtState.Inactive;

            ReligionSettings_PietyNeed settings = comp.Religion.GetSettings<ReligionSettings_PietyNeed>(SettingsTagDefOf.NeedTag);
            if (settings == null)
                return ThoughtState.Inactive;

            if (settings.NeedThought != null && settings.NeedThought == this.def)
                return ThoughtState.ActiveAtStage(comp.PietyTracker.PietyNeed.CurCategoryInt);

            return ThoughtState.Inactive;
        }
    }
}
