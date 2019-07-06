using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ReligionsOfRimworld
{
    public class ITab_ReligionActivity : ITab
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
        private ReligionActivityTask mouseoverTask;

        public ITab_ReligionActivity()
        {
            this.size = ITab_ReligionActivity.WinSize;
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
            Rect rect1 = new Rect(ITab_ReligionActivity.WinSize.x - ITab_ReligionActivity.PasteX, ITab_ReligionActivity.PasteY, ITab_ReligionActivity.PasteSize, ITab_ReligionActivity.PasteSize);

            if (SelFacility.AssignedReligion == null)
            {
                Widgets.Label(new Rect(0.0f, 0.0f, ITab_ReligionActivity.WinSize.x, ITab_ReligionActivity.WinSize.y).ContractedBy(10f), "Religion_NoAssignedReligion".Translate());
                return;
            }

            //if (BillUtility.Clipboard == null)
            //{
            //    GUI.color = Color.gray;
            //    Widgets.DrawTextureFitted(rect1, (Texture)GraphicsCache.Paste, 1f);
            //    GUI.color = Color.white;
            //    TooltipHandler.TipRegion(rect1, (TipSignal)"PasteBillTip".Translate());
            //}
            //else if (!this.SelFacility.def.AllRecipes.Contains(BillUtility.Clipboard.recipe) || !BillUtility.Clipboard.recipe.AvailableNow)
            //{
            //    GUI.color = Color.gray;
            //    Widgets.DrawTextureFitted(rect1, (Texture)GraphicsCache.Paste, 1f);
            //    GUI.color = Color.white;
            //    TooltipHandler.TipRegion(rect1, (TipSignal)"ClipboardBillNotAvailableHere".Translate());
            //}
            //else if (this.SelFacility.TaskStack.Count >= 15)
            //{
            //    GUI.color = Color.gray;
            //    Widgets.DrawTextureFitted(rect1, (Texture)GraphicsCache.Paste, 1f);
            //    GUI.color = Color.white;
            //    TooltipHandler.TipRegion(rect1, (TipSignal)("PasteBillTip".Translate() + " (" + "PasteBillTip_LimitReached".Translate() + ")"));
            //}
            //else
            //{
            //    if (Widgets.ButtonImageFitted(rect1, GraphicsCache.Paste, Color.white))
            //    {
            //        Bill bill = BillUtility.Clipboard.Clone();
            //        bill.InitializeAfterClone();
            //        this.SelFacility.TaskStack.AddBill(new Bill_ReligionActivity(MiscDefOf.ReligionActivity, settings));
            //        SoundDefOf.Tick_Low.PlayOneShotOnCamera((Map)null);
            //    }
            //    TooltipHandler.TipRegion(rect1, (TipSignal)"PasteBillTip".Translate());
            //}
            this.mouseoverTask = this.SelFacility.TaskStack.DoListing(new Rect(0.0f, 0.0f, ITab_ReligionActivity.WinSize.x, ITab_ReligionActivity.WinSize.y).ContractedBy(10f), (Func<List<FloatMenuOption>>)(() =>
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach(ReligionSettings_ReligionActivity settings in GetAllRelatedSettings(SelFacility.AssignedReligion))
                {
                    list.Add(new FloatMenuOption(settings.Label, (Action)(() =>
                    {
                        if (!this.SelFacility.Map.mapPawns.FreeColonists.Any<Pawn>((Func<Pawn, bool>)(col => ReligionActivityUtility.PawnSatisfiesSkillRequirements(col, settings.SkillRequirements))))
                            CreateNoPawnsWithSkillDialog(settings);
                        if (!this.SelFacility.Map.mapPawns.FreeColonists.Any<Pawn>(x => x.GetReligionComponent().Religion == SelFacility.AssignedReligion))
                            CreateNoPawnsOfReligionDialog(SelFacility.AssignedReligion);
                        this.SelFacility.TaskStack.Add(new ReligionActivityTask(SelFacility.TaskStack, settings, SelFacility.Position));
                        //if (recipe.conceptLearned != null)
                        //    PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
                        //if (!TutorSystem.TutorialMode)
                        //    return;
                        TutorSystem.Notify_Event((EventPack)("AddBill-" + settings.Label));
                    }), MenuOptionPriority.Default, (Action)null, (Thing)null, 29f, (Func<Rect, bool>)(rect => Widgets.InfoCardButton(rect.x + 5f, rect.y + (float)(((double)rect.height - 24.0) / 2.0), (Def)MiscDefOf.ReligionActivity)), (WorldObject)null));
                }
                if (!list.Any<FloatMenuOption>())
                    list.Add(new FloatMenuOption("NoneBrackets".Translate(), (Action)null, MenuOptionPriority.Default, (Action)null, (Thing)null, 0.0f, (Func<Rect, bool>)null, (WorldObject)null));
                return list;
            }), ref this.scrollPosition, ref this.viewHeight);
        }

        public override void TabUpdate()
        {
            if (this.mouseoverTask == null)
                return;           
            this.mouseoverTask = (ReligionActivityTask)null;
        }

        private IEnumerable<ReligionSettings_ReligionActivity> GetAllRelatedSettings(Religion religion)
        {
            foreach (ReligionSettings settings in religion.AllSettings)
                if (settings is ReligionSettings_ReligionActivity)
                    yield return (ReligionSettings_ReligionActivity)settings;
        }

        private void CreateNoAssignedReligionDialog()
        {
            Find.WindowStack.Add((Window)new Dialog_MessageBox("Religion_NoAssignedReligion".Translate(), (string)null, (Action)null, (string)null, (Action)null, (string)null, false, (Action)null, (Action)null));
        }

        private void CreateNoPawnsWithSkillDialog(ReligionSettings_ReligionActivity settings)
        {
            Find.WindowStack.Add((Window)new Dialog_MessageBox("ActivityRequiresSkills".Translate((NamedArgument)settings.Label) + "\n\n" + ReligionActivityUtility.MinSkillString(settings.SkillRequirements), (string)null, (Action)null, (string)null, (Action)null, (string)null, false, (Action)null, (Action)null));
        }

        private void CreateNoPawnsOfReligionDialog(Religion religion)
        {
            Find.WindowStack.Add((Window)new Dialog_MessageBox("ReligionActivity_NoPawnsOfReligion".Translate((NamedArgument)religion.Label), (string)null, (Action)null, (string)null, (Action)null, (string)null, false, (Action)null, (Action)null));
        }
    }
}
