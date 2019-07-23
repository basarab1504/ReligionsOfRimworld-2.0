using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ReligionsOfRimworld
{
    public class ActivityTask : IExposable, ILoadReferenceable
    {
        private int loadID = -1;
        private ActivityTaskManager manager;
        private IntVec3 center;
        private Map map;
        private Pawn pawn;
        private float ingredientSearchRadius = 999f;
        private bool suspended;
        private Religion religion;
        private ReligionActivityProperty property;

        public ActivityTask(Building_ReligiousBuildingFacility facility, ReligionActivityProperty property)
        {
            this.loadID = Find.UniqueIDsManager.GetNextBillID();
            this.map = facility.Map;
            this.religion = facility.AssignedReligion;
            this.manager = facility.TaskManager;
            this.property = property;
        }

        public ReligionActivityProperty Property => property;
        public string Label => property.Label;
        public Pawn Pawn { get => pawn; set => pawn = value; }
        public Map Map => map;
        public Religion Religion => religion;
        public IntVec3 Center => center;
        public float IngredientSearchRadius { get => ingredientSearchRadius; set => ingredientSearchRadius = value; }

        public bool ShouldDoNow()
        {
            if (!suspended)
                return true;
            return false;
        }

        public string GetUniqueLoadID()
        {
            return "Activity_" + this.loadID;
        }

        public void TryDrawIngredientSearchRadiusOnMap()
        {
            if ((double)ingredientSearchRadius >= (double)GenRadial.MaxRadialPatternRadius)
                return;
            GenDraw.DrawRadiusRing(center, ingredientSearchRadius);
        }

        public Rect DoInterface(float x, float y, float width, int index)
        {
            Rect rect1 = new Rect(x, y, width, 53f);
            float height = 0.0f;
            rect1.height += height;
            Color baseColor = Color.white;
            if (!this.ShouldDoNow())
                baseColor = new Color(1f, 0.7f, 0.7f, 0.7f);
            GUI.color = baseColor;
            Text.Font = GameFont.Small;
            if (index % 2 == 0)
                Widgets.DrawAltRect(rect1);
            GUI.BeginGroup(rect1);
            Widgets.Label(new Rect(28f, 0.0f, (float)((double)rect1.width - 48.0 - 20.0), rect1.height + 5f), this.property.Label);
            this.DoConfigInterface(rect1.AtZero(), baseColor);
            Rect rect4 = new Rect(rect1.width - 24f, 0.0f, 24f, 24f);
            if (Widgets.ButtonImage(rect4, GraphicsCache.DeleteX, baseColor, baseColor * GenUI.SubtleMouseoverColor))
            {
                manager.Delete(this);
                SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
            }
            TooltipHandler.TipRegion(rect4, (TipSignal)"DeleteBillTip".Translate());
            Rect rect5 = new Rect(rect4);
            rect5.x -= rect5.width + 4f;
            if (Widgets.ButtonImage(rect5, GraphicsCache.Suspend, baseColor))
            {
                this.suspended = !this.suspended;
                SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
            }
            TooltipHandler.TipRegion(rect5, (TipSignal)"SuspendBillTip".Translate());
            //if (!this.StatusString.NullOrEmpty())
            //{
            //    Text.Font = GameFont.Tiny;
            //    Rect rect3 = new Rect(24f, rect1.height - height, rect1.width - 24f, height);
            //    Widgets.Label(rect3, this.StatusString);
            //    this.DoStatusLineInterface(rect3);
            //}
            GUI.EndGroup();
            if (this.suspended)
            {
                Text.Font = GameFont.Medium;
                Text.Anchor = TextAnchor.MiddleCenter;
                Rect rect3 = new Rect((float)((double)rect1.x + (double)rect1.width / 2.0 - 70.0), (float)((double)rect1.y + (double)rect1.height / 2.0 - 20.0), 140f, 40f);
                GUI.DrawTexture(rect3, (Texture)TexUI.GrayTextBG);
                Widgets.Label(rect3, "SuspendedCaps".Translate());
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
            }
            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            return rect1;
        }

        private void DoConfigInterface(Rect baseRect, Color baseColor)
        {
            Rect rect = new Rect(28f, 32f, 100f, 30f);
            GUI.color = new Color(1f, 1f, 1f, 0.65f);
            //Widgets.Label(rect, this.RepeatInfoText);
            GUI.color = baseColor;
            WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
            if (widgetRow.ButtonText("Details".Translate() + "...", (string)null, true, false))
                Find.WindowStack.Add((Window)new Dialog_ActivityTaskConfig(this));
        }

        public void ExposeData()
        {

        }
    }
}
