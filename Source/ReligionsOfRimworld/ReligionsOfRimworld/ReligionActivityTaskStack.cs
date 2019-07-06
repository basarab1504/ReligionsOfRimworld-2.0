using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionActivityTaskStack : IExposable
    {
        private List<ReligionActivityTask> tasks = new List<ReligionActivityTask>();

        public IEnumerable<ReligionActivityTask> Tasks => tasks;
        
        public void Add(ReligionActivityTask task)
        {
            tasks.Add(task);
        }

        public void Remove(ReligionActivityTask task)
        {
            tasks.Remove(task);
        }

        public void Clear()
        {
            tasks.Clear();
        }

        public ReligionActivityTask FirstShouldDoNow
        {
            get
            {
                foreach (ReligionActivityTask task in tasks)
                    if (task.ShouldDoNow)
                        return task;
                return null;
            }
        }

        public ReligionActivityTask DoListing(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
        {
            ReligionActivityTask task1 = (ReligionActivityTask)null;
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            if (tasks.Count < 15)
            {
                Rect rect1 = new Rect(0.0f, 0.0f, 150f, 29f);
                if (Widgets.ButtonText(rect1, "Religion_AddTask".Translate(), true, false, true))
                    Find.WindowStack.Add((Window)new FloatMenu(recipeOptionsMaker()));
                UIHighlighter.HighlightOpportunity(rect1, "Religion_AddTask");
            }
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
            Rect outRect = new Rect(0.0f, 35f, rect.width, rect.height - 35f);
            Rect viewRect = new Rect(0.0f, 0.0f, outRect.width - 16f, viewHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
            float y = 0.0f;
            for (int index = 0; index < tasks.Count; ++index)
            {
                ReligionActivityTask task2 = this.tasks[index];
                Rect rect1 = task2.DoInterface(0.0f, y, viewRect.width, index);
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
            Scribe_Collections.Look<ReligionActivityTask>(ref this.tasks, "tasks", LookMode.Deep, this, null, null);
        }
    }
}
