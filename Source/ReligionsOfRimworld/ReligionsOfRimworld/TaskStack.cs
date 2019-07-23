using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class ActivityTaskManager : IExposable
    {
        private List<ActivityTask> tasks = new List<ActivityTask>();

        public int Count => tasks.Count;

        public void Add(ActivityTask activityTask)
        {
            tasks.Add(activityTask);
        }

        public void Delete(ActivityTask activityTask)
        {
            tasks.Remove(activityTask);
        }

        public void Clear()
        {
            tasks.Clear();
        }

        public ActivityTask DoListing(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
        {
            ActivityTask task1 = (ActivityTask)null;
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            if (this.Count < 15)
            {
                Rect rect1 = new Rect(0.0f, 0.0f, 150f, 29f);
                if (Widgets.ButtonText(rect1, "AddBill".Translate(), true, false, true))
                    Find.WindowStack.Add((Window)new FloatMenu(recipeOptionsMaker()));
                UIHighlighter.HighlightOpportunity(rect1, "AddBill");
            }
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
            Rect outRect = new Rect(0.0f, 35f, rect.width, rect.height - 35f);
            Rect viewRect = new Rect(0.0f, 0.0f, outRect.width - 16f, viewHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
            float y = 0.0f;
            for (int index = 0; index < this.Count; ++index)
            {
                ActivityTask task2 = this.tasks[index];
                Rect rect1 = task2.DoInterface(0.0f, y, viewRect.width, index);
                if (Mouse.IsOver(rect1))
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
            Scribe_Collections.Look<ActivityTask>(ref this.tasks, "tasks");
        }
    }
}
