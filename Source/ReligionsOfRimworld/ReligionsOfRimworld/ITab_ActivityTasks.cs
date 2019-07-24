using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class ITab_ActivityTasks : ITab
    {
        private static readonly Vector2 WinSize = new Vector2(420f, 480f);
        [TweakValue("Interface", 0.0f, 128f)]
        private static float PasteX = 48f;
        [TweakValue("Interface", 0.0f, 128f)]
        private static float PasteY = 3f;
        [TweakValue("Interface", 0.0f, 32f)]
        private static float PasteSize = 24f;
        private float viewHeight = 1000f;
        private Vector2 scrollPosition = new Vector2();
        private ActivityTask mouseoverTask;

        public ITab_ActivityTasks()
        {
            this.size = ITab_ActivityTasks.WinSize;
            this.labelKey = "Religion_Tasks".Translate();
        }

        protected Building_ReligiousBuildingFacility SelFacility
        {
            get
            {
                return (Building_ReligiousBuildingFacility)this.SelThing;
            }
        }

        protected override void FillTab()
        {
            Rect rect1 = new Rect(ITab_ActivityTasks.WinSize.x - ITab_ActivityTasks.PasteX, ITab_ActivityTasks.PasteY, ITab_ActivityTasks.PasteSize, ITab_ActivityTasks.PasteSize);

            if (!SelFacility.IsComplete)
            {
                Widgets.Label(new Rect(0.0f, 0.0f, ITab_ActivityTasks.WinSize.x, ITab_ActivityTasks.WinSize.y).ContractedBy(10f), "Religion_BuildingIsNotAvaliable".Translate());
                return;
            }
            this.mouseoverTask = this.SelFacility.TaskManager.DoListing(new Rect(0.0f, 0.0f, ITab_ActivityTasks.WinSize.x, ITab_ActivityTasks.WinSize.y).ContractedBy(10f), GetFloatMenuOptions(), ref this.scrollPosition, ref this.viewHeight);
        }

        private List<FloatMenuOption> GetFloatMenuOptions()
        {
            List<FloatMenuOption> list = new List<FloatMenuOption>();
            foreach (ActivityTaskDef property in SelFacility.AssignedReligion.FindByTag<ReligionSettings_ReligionActivity>(SettingsTagDefOf.ActivityTag).Properties)
            {
                list.Add(new FloatMenuOption(property.label, (Action)(() =>
                {
                    if (!this.SelFacility.Map.mapPawns.FreeColonists.Any<Pawn>(x => x.GetReligionComponent().Religion == SelFacility.AssignedReligion))
                        CreateNoPawnsOfReligionDialog(SelFacility.AssignedReligion);
                    this.SelFacility.TaskManager.Create(property);
                }), MenuOptionPriority.Default, (Action)null, (Thing)null, 29f, (Func<Rect, bool>)(rect => Widgets.InfoCardButton(rect.x + 5f, rect.y + (float)(((double)rect.height - 24.0) / 2.0), property)), (WorldObject)null));
            }
            if (!list.Any<FloatMenuOption>())
                list.Add(new FloatMenuOption("NoneBrackets".Translate(), (Action)null, MenuOptionPriority.Default, (Action)null, (Thing)null, 0.0f, (Func<Rect, bool>)null, (WorldObject)null));
            return list;
        }

        private void CreateNoPawnsOfReligionDialog(Religion religion)
        {
            Find.WindowStack.Add((Window)new Dialog_MessageBox("ReligionActivity_NoPawnsOfReligion".Translate((NamedArgument)religion.Label), (string)null, (Action)null, (string)null, (Action)null, (string)null, false, (Action)null, (Action)null));
        }

        public override void TabUpdate()
        {
            if (this.mouseoverTask == null)
                return;
            this.mouseoverTask = null;
        }
    }
}
