//using RimWorld;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Verse;

//namespace ReligionsOfRimworld
//{
//    public class ReligionActivityStack : IExposable
//    {          
//        private List<ReligionActivityTask> tasks = new List<ReligionActivityTask>();
//        public Building_ReligiousBuildingFacility parent;
//        public const int MaxCount = 15;
//        private const float TopAreaHeight = 35f;
//        private const float BillInterfaceSpacing = 6f;
//        private const float ExtraViewHeight = 60f;

//        public ReligionActivityStack(Building_ReligiousBuildingFacility parent)
//        {
//            this.parent = parent;
//        }

//        public List<ReligionActivityTask> Tasks
//        {
//            get
//            {
//                return this.tasks;
//            }
//        }

//        public ReligionActivityTask this[int index]
//        {
//            get
//            {
//                return this.tasks[index];
//            }
//        }

//        public int Count
//        {
//            get
//            {
//                return this.tasks.Count;
//            }
//        }

//        public ReligionActivityTask FirstShouldDoNow
//        {
//            get
//            {
//                for (int index = 0; index < this.Count; ++index)
//                {
//                    if (this.tasks[index].ShouldDoNow())
//                        return this.tasks[index];
//                }
//                return (ReligionActivityTask)null;
//            }
//        }

//        public bool AnyShouldDoNow
//        {
//            get
//            {
//                for (int index = 0; index < this.Count; ++index)
//                {
//                    if (this.tasks[index].ShouldDoNow())
//                        return true;
//                }
//                return false;
//            }
//        }

//        public void AddTask(ReligionActivityTask task)
//        {
//            task.billStack = this;
//            this.tasks.Add(task);
//        }

//        public void Delete(ReligionActivityTask task)
//        {
//            task.deleted = true;
//            this.tasks.Remove(task);
//        }

//        public void Clear()
//        {
//            this.tasks.Clear();
//        }

//        public void Reorder(ReligionActivityTask task, int offset)
//        {
//            int index = this.tasks.IndexOf(task) + offset;
//            if (index < 0)
//                return;
//            this.tasks.Remove(task);
//            this.tasks.Insert(index, task);
//        }

//        public void RemoveIncompletableBills()
//        {
//            for (int index = this.tasks.Count - 1; index >= 0; --index)
//            {
//                if (!this.tasks[index].CompletableEver)
//                    this.tasks.Remove(this.tasks[index]);
//            }
//        }

//        public int IndexOf(ReligionActivityTask task)
//        {
//            return this.tasks.IndexOf(task);
//        }

//        public void ExposeData()
//        {
//            Scribe_Collections.Look<ReligionActivityTask>(ref this.tasks, "bills", LookMode.Deep, new object[0]);
//            if (Scribe.mode != LoadSaveMode.ResolvingCrossRefs)
//                return;
//            if (this.tasks.RemoveAll((Predicate<ReligionActivityTask>)(x => x == null)) != 0)
//                Log.Error("Some bills were null after loading.", false);
//            for (int index = 0; index < this.tasks.Count; ++index)
//                this.tasks[index].billStack = this;
//        }

//        public ReligionActivityTask DoListing(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
//        {
//            ReligionActivityTask bill1 = (ReligionActivityTask)null;
//            GUI.BeginGroup(rect);
//            Text.Font = GameFont.Small;
//            if (this.Count < 15)
//            {
//                Rect rect1 = new Rect(0.0f, 0.0f, 150f, 29f);
//                if (Widgets.ButtonText(rect1, "AddBill".Translate(), true, false, true))
//                    Find.WindowStack.Add((Window)new FloatMenu(recipeOptionsMaker()));
//                UIHighlighter.HighlightOpportunity(rect1, "AddBill");
//            }
//            Text.Anchor = TextAnchor.UpperLeft;
//            GUI.color = Color.white;
//            Rect outRect = new Rect(0.0f, 35f, rect.width, rect.height - 35f);
//            Rect viewRect = new Rect(0.0f, 0.0f, outRect.width - 16f, viewHeight);
//            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
//            float y = 0.0f;
//            for (int index = 0; index < this.Count; ++index)
//            {
//                ReligionActivityTask bill2 = this.tasks[index];
//                Rect rect1 = bill2.DoInterface(0.0f, y, viewRect.width, index);
//                if (!bill2.DeletedOrDereferenced && Mouse.IsOver(rect1))
//                    bill1 = bill2;
//                y += rect1.height + 6f;
//            }
//            if (Event.current.type == EventType.Layout)
//                viewHeight = y + 60f;
//            Widgets.EndScrollView();
//            GUI.EndGroup();
//            return bill1;
//        }
//    }
//}
