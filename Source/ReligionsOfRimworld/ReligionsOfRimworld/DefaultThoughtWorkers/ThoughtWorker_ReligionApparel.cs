using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    class ThoughtWorker_ReligionApparel : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            CompReligion comp = p.GetReligionComponent();
            string reason = (string)null;
            int num = 0;
            List<Apparel> wornApparel = p.apparel.WornApparel;

            if (comp == null)
                return ThoughtState.Inactive;

            ReligionSettings_Social settings = comp.Religion.ApparelSettings;

            if (settings == null)
                return ThoughtState.Inactive;

            for (int index = 0; index < wornApparel.Count; ++index)
            {
                ReligionProperty property = settings.GetPropertyByObject(wornApparel[index].def);
                if (property != null && property.Subject.Thought == this.def)
                {
                    if (reason == null)
                        reason = wornApparel[index].def.label;
                    ++num;
                }
            }
            if (num == 0)
                return ThoughtState.Inactive;
            return ThoughtState.ActiveAtStage((num - 1) + (comp.PietyTracker.PietyNeed.CurCategoryInt * 5), reason);
        }
    }
}
