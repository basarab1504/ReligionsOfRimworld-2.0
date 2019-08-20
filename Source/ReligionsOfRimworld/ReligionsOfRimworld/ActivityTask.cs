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
        [Unsaved]
        private IngredientValueGetter ingredientValueGetterInt;
        private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

        public IEnumerable<KeyValuePair<ThingDef, int>> ThingDefsCount => thingDefsCount;
        public ActivityJobQueueDef ActivityQueue => activityJobQueue;
        public ReligionPropertyData OrganizerProperty => organizerProperty;
        public ReligionPropertyData CongregationProperty => congregationProperty;

        public IngredientValueGetter IngredientValueGetter
        {
            get
            {
                if (this.ingredientValueGetterInt == null)
                    this.ingredientValueGetterInt = (IngredientValueGetter)Activator.CreateInstance(this.ingredientValueGetterClass);
                return this.ingredientValueGetterInt;
            }
        }


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

    public class ActivityTaskSchedule : IExposable
    {
        private Building_ReligiousBuildingFacility facility;
        private List<ScheduledDay> scheduledDays;

        public ActivityTaskSchedule(Building_ReligiousBuildingFacility facility)
        {
            this.facility = facility;
            scheduledDays = new List<ScheduledDay>(15);
        }

        public Building_ReligiousBuildingFacility Facility => facility;
        public IEnumerable<ScheduledDay> ScheduledDays => scheduledDays;

        public IEnumerable<ActivityTask> AllTasks()
        {
            foreach (ScheduledDay day in scheduledDays)
                foreach (ActivityTask task in day.Tasks)
                    yield return task;
        }

        public bool AnyShouldDoNow => scheduledDays.Any(x => x.AnyShouldDoNow);

        public void Create(int dayNumber)
        {
            if (!scheduledDays.Contains(scheduledDays.FirstOrDefault(x => x.DayNumber == dayNumber)))
                scheduledDays.Add(new ScheduledDay(dayNumber));
        }

        public void Delete(int dayNumber)
        {
            scheduledDays.Remove(scheduledDays.FirstOrDefault(x => x.DayNumber == dayNumber));
        }

        public void Reorder()
        {
            scheduledDays.RemoveAll(x => x.Tasks.Count() == 0);

            foreach (ScheduledDay day in scheduledDays)
            {
                if (day.Tasks.Count() == 0)
                    scheduledDays.Remove(day);
                day.Reorder();
            }
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<ScheduledDay>(ref this.scheduledDays, "scheduledDays", LookMode.Deep, null, null);
        }
    }

    public class ActivityTask : IExposable, ILoadReferenceable
    {
        private int loadID = -1;
        private ActivityTaskSchedule manager;
        private SimpleFilter filter;
        private float ingredientSearchRadius = 999f;
        private int lastIngredientSearchFailTicks = -99999;
        private bool suspended;
        private Pawn pawnRestriction;
        private ActivityTaskDef property;
        private int startHour;
        private IngredientPawn humanlike;
        private IngredientPawn animal;

        public ActivityTask(ActivityTaskSchedule manager, ActivityTaskDef def)
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            {
                this.manager = manager;
                startHour = 12;
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

        public int StartHour {
            get => startHour;
            set
            {
                startHour = value;
                manager.Reorder();
            }
        }
        public Building_ReligiousBuildingFacility ParentFacility => manager.Facility;
        public bool Suspended { get => suspended; set => suspended = value; }
        public SimpleFilter ThingFilter => filter;
        public Pawn PawnRestriction { get => pawnRestriction; set => pawnRestriction = value; }
        public float IngredientSearchRadius { get => ingredientSearchRadius; set => ingredientSearchRadius = value; }
        public int LastIngredientSearchFailTicks { get => lastIngredientSearchFailTicks; set => lastIngredientSearchFailTicks = value; }
        public IngredientPawn HumanlikeIngredient => humanlike;
        public IngredientPawn AnimalIngredient => animal;
        public ActivityTaskDef Property => property;
        public string Label => property.LabelCap;
        public string Description => property.description;

        public void ValidateSettings()
        {
            if (pawnRestriction != null && pawnRestriction.Dead)
                pawnRestriction = null;
        }

        public bool ShouldDoNow()
        {
            if (!suspended)
                return true;
            return false;
        }

        public void Notify_IterationCompleted(Pawn pawn)
        {
            suspended = true;
        }

        public bool PawnAllowedToStartAnew(Pawn p)
        {
            if (this.pawnRestriction != null)
                return this.pawnRestriction == p;
            return p.GetReligionComponent().Religion == ParentFacility.AssignedReligion;
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
            Widgets.Label(new Rect(20, 0.0f, (float)((double)rect1.width - 48.0 - 20.0), rect1.height + 5f), this.startHour.ToString());
            Widgets.Label(new Rect(50f, 0.0f, (float)((double)rect1.width - 48.0 - 20.0), rect1.height + 5f), this.property.LabelCap);
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
            GenDraw.DrawRadiusRing(ParentFacility.Position, ingredientSearchRadius);
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
