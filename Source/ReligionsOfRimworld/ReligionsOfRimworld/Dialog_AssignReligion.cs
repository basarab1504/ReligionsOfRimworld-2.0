using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.Sound;

namespace ReligionsOfRimworld
{
    class Dialog_AssignReligion : Window
    {
        private Building_ReligiousBuildingMain assignable;
        private Vector2 scrollPosition;
        private const float EntryHeight = 35f;

        public Dialog_AssignReligion(Building_ReligiousBuildingMain assignable)
        {
            this.assignable = assignable;
            this.doCloseButton = true;
            this.doCloseX = true;
            this.closeOnClickedOutside = true;
            this.absorbInputAroundWindow = true;
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(620f, 500f);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            Rect outRect = new Rect(inRect);
            outRect.yMin += 20f;
            outRect.yMax -= 40f;
            outRect.width -= 16f;
            Rect viewRect = new Rect(0.0f, 0.0f, outRect.width - 16f, (float)(ReligionManager.GetReligionManager().AllReligions.Count() * 35.0 + 100.0));
            Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
            try
            {
                float y = 0.0f;
                bool flag = false;
                if (assignable.AssignedReligion != null)
                {
                    flag = true;
                    Rect rect = new Rect(0.0f, y, viewRect.width * 0.6f, 32f);
                    Widgets.Label(rect, assignable.AssignedReligion.Label);
                    rect.x = rect.xMax;
                    rect.width = viewRect.width * 0.4f;
                    if (Widgets.ButtonText(rect, "BuildingUnassign".Translate(), true, false, true))
                    {
                        if(assignable.AssignedBuildings.Count() != 0)
                            Messages.Message("ReligiousBuilgingAssigner_BuildingWasUnassigned".Translate(), MessageTypeDefOf.NeutralEvent);
                        this.assignable.TryUnassignReligion();
                        SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
                        return;
                    }
                    y += 35f;
                }
                if (flag)
                    y += 15f;
                foreach (Religion assigningCandidate in ReligionManager.GetReligionManager().AllReligions)
                {
                    if (assigningCandidate != assignable.AssignedReligion 
                        && assigningCandidate.AllowedBuildingsSettings != null && assigningCandidate.AllowedBuildingsSettings.AllowedBuildings.Any(x => x == assignable.def))
                    {
                        Rect rect = new Rect(0.0f, y, viewRect.width * 0.6f, 32f);
                        Widgets.Label(rect, assigningCandidate.Label);
                        rect.x = rect.xMax;
                        rect.width = viewRect.width * 0.4f;
                        if (Widgets.ButtonText(rect, "BuildingAssign".Translate(), true, false, true))
                        {
                            this.assignable.TryAssignReligion(assigningCandidate);
                            SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
                            break;
                        }
                        y += 35f;
                    }
                }
            }
            finally
            {
                Widgets.EndScrollView();
            }
        }
    }
}
