using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class PietyWorker_ApparelStuff : PietyWorker
    {
        public override PietyState CurrentState(Pawn p)
        {
            CompReligion comp = p.GetReligionComponent();
            string reason = (string)null;
            int num = 0;
            List<Apparel> wornApparel = p.apparel.WornApparel;

            if (comp == null)
                return PietyState.Inactive;

            ReligionSettings_Social settings = comp.Religion.ApparelSettings;

            if (settings == null)
                return PietyState.Inactive;

            for (int index = 0; index < wornApparel.Count; ++index)
            {
                ReligionProperty property = settings.GetPropertyByObject(p, wornApparel[index].def);

                if (property == null)
                    property = settings.GetPropertyByObject(p, wornApparel[index].Stuff);

                if (property != null && property.Subject.Piety == this.def)
                {
                    if (reason == null)
                        reason = wornApparel[index].def.label;
                    ++num;
                }
            }
            if (num == 0)
                return PietyState.Inactive;
            return PietyState.ActiveAtStage((num - 1) + (comp.PietyTracker.PietyNeed.CurCategoryInt * 5), reason);
        }
    }
}