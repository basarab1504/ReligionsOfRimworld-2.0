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
    public class ActivityTaskDef : Def
    {
        private Dictionary<ThingDef, int> thingDefsCount = new Dictionary<ThingDef, int>();
        private ActivityJobQueueDef activityJobQueue;
        private ReligionPropertyData organizerProperty;
        private ReligionPropertyData congregationProperty;

        public IEnumerable<KeyValuePair<ThingDef, int>> ThingDefsCount => thingDefsCount;

        public IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            //yield return new ReligionInfoEntry("");
            //if (recipe != null)
            //    yield return new ReligionInfoEntry("ReligionInfo_Activity".Translate(), recipe.LabelCap, recipe.description);

            if (organizerProperty != null)
                foreach (ReligionInfoEntry entry in organizerProperty.GetInfoEntries())
                    yield return entry;

            if (congregationProperty != null)
                foreach (ReligionInfoEntry entry in congregationProperty.GetInfoEntries())
                    yield return entry;
        }
    }

    public class ActivityTaskManager : IExposable
    {
        private Building_ReligiousBuildingFacility facility;
        private List<ActivityTask> tasks;

        public ActivityTaskManager(Building_ReligiousBuildingFacility facility)
        {
            this.facility = facility;
            tasks = new List<ActivityTask>();
        }

        public void Create(ActivityTaskDef def)
        {
            tasks.Add(new ActivityTask(facility, def));
        }

        public void Delete(ActivityTask activityTask)
        {
            tasks.Remove(activityTask);
        }

        public ActivityTask DoListing(Rect rect, List<FloatMenuOption> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
        {
            ActivityTask task1 = (ActivityTask)null;
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            if (tasks.Count < 15)
            {
                Rect rect1 = new Rect(0.0f, 0.0f, 150f, 29f);
                if (Widgets.ButtonText(rect1, "AddTask".Translate(), true, false, true))
                    Find.WindowStack.Add((Window)new FloatMenu(recipeOptionsMaker));
                UIHighlighter.HighlightOpportunity(rect1, "AddTask");
            }
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
            Rect outRect = new Rect(0.0f, 35f, rect.width, rect.height - 35f);
            Rect viewRect = new Rect(0.0f, 0.0f, outRect.width - 16f, viewHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
            float y = 0.0f;
            for (int index = 0; index < tasks.Count; ++index)
            {
                ActivityTask task2 = this.tasks[index];
                Rect rect1 = task2.DoInterface(0.0f, y, viewRect.width, index);
                Rect deleteRect = new Rect(rect1.width - 24f, 0.0f, 24f, 24f);
                if (Widgets.ButtonImage(deleteRect, GraphicsCache.DeleteX, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                {
                    this.Delete(task2);
                    SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
                }
                TooltipHandler.TipRegion(deleteRect, (TipSignal)"DeleteBillTip".Translate());
                if (/*!task2.DeletedOrDereferenced && */Mouse.IsOver(rect1))
                    task1 = task2;
                y += rect1.height + 6f;
            }
            if (Event.current.type == EventType.Layout)
                viewHeight = y + 60f;
            Widgets.EndScrollView();
            GUI.EndGroup();
            return task1;
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<ActivityTask>(ref this.tasks, "tasks", LookMode.Deep, null, null);
        }
    }

    public class ActivityTask : IExposable, ILoadReferenceable
    {
        private int loadID = -1;
        private Building_ReligiousBuildingFacility facility;
        private SimpleFilter filter;
        private float ingredientSearchRadius = 999f;
        private int lastIngredientSearchFailTicks = -99999;
        private bool suspended;
        private Pawn pawnRestriction;
        private ActivityTaskDef property;
        private IngredientPawn humanlike;
        private IngredientPawn animal;

        public ActivityTask(Building_ReligiousBuildingFacility facility, ActivityTaskDef def)
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            {
                this.facility = facility;
                this.property = def;
                this.loadID = Find.UniqueIDsManager.GetNextBillID();
                humanlike = new IngredientPawn();
                animal = new IngredientPawn();
                List<ThingDef> defs = new List<ThingDef>();
                foreach (KeyValuePair<ThingDef, int> kvp in def.ThingDefsCount)
                    defs.Add(kvp.Key);
                filter = new SimpleFilter(defs);
            }
        }

        public Building_ReligiousBuildingFacility Facility => facility;
        public bool Suspended { get => suspended; set => suspended = value; }
        public SimpleFilter ThingFilter => filter;
        public Pawn PawnRestriction { get => pawnRestriction; set => pawnRestriction = value; }
        public float IngredientSearchRadius { get => ingredientSearchRadius; set => ingredientSearchRadius = value; }
        public IngredientPawn HumanlikeIngredient => humanlike;
        public IngredientPawn AnimalIngredient => animal;

        public string Label => property.LabelCap;
        public string Description => property.description;

        public bool ShouldDoNow()
        {
            if (!suspended)
                return true;
            return false;
        }

        public Rect DoInterface(float x, float y, float width, int index)
        {
            Rect rect1 = new Rect(x, y, width, 53f);
            Color baseColor = Color.white;
            if (!this.ShouldDoNow())
                baseColor = new Color(1f, 0.7f, 0.7f, 0.7f);
            GUI.color = baseColor;
            Text.Font = GameFont.Small;
            if (index % 2 == 0)
                Widgets.DrawAltRect(rect1);
            GUI.BeginGroup(rect1);
            Widgets.Label(new Rect(28f, 0.0f, (float)((double)rect1.width - 48.0 - 20.0), rect1.height + 5f), this.property.LabelCap);
            this.DoConfigInterface(rect1.AtZero(), baseColor);
            Rect rect5 = new Rect(rect1.width - 24f, 0.0f, 24f, 24f);
            rect5.x -= rect5.width + 4f;
            if (Widgets.ButtonImage(rect5, GraphicsCache.Suspend, baseColor))
            {
                this.suspended = !this.suspended;
                SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
            }
            TooltipHandler.TipRegion(rect5, (TipSignal)"SuspendBillTip".Translate());
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

        public void TryDrawIngredientSearchRadiusOnMap()
        {
            if ((double)ingredientSearchRadius >= (double)GenRadial.MaxRadialPatternRadius)
                return;
            GenDraw.DrawRadiusRing(facility.Position, ingredientSearchRadius);
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

        public string GetUniqueLoadID()
        {
            return "ActivityTask_" + (object)this.loadID;
        }

        public void ExposeData()
        {

        }
    }
}
