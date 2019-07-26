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
    public class Dialog_ActivityTaskConfig : Window
    {
        private ActivityTask task;

        private Vector2 thingFilterScrollPosition;

        public Dialog_ActivityTaskConfig(ActivityTask task)
        {
            this.task = task;

            this.forcePause = true;
            this.doCloseX = true;
            this.doCloseButton = true;
            this.absorbInputAroundWindow = true;
            this.closeOnClickedOutside = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            Rect rect = new Rect(0f, 0f, 400f, 50f);
            Widgets.Label(rect, this.task.Label);
            float width = (float)((int)((inRect.width - 34f) / 3f));
            Rect rect2 = new Rect(0f, 50f, width, inRect.height - 50f - this.CloseButSize.y);
            Rect rect3 = new Rect(rect2.xMax + 17f, 50f, width, inRect.height - 50f - this.CloseButSize.y);
            Rect rect4 = new Rect(rect3.xMax + 17f, 50f, 0f, inRect.height - 50f - this.CloseButSize.y);
            rect4.xMax = inRect.xMax;
            Text.Font = GameFont.Small;

            Listing_Standard mainListing = new Listing_Standard();
            mainListing.Begin(rect2);
            DrawInfo(mainListing);
            mainListing.End();

            Listing_Standard middleListing = new Listing_Standard();
            middleListing.Begin(rect3);
            PawnRestriction(middleListing);
            middleListing.Gap();
            IngredientPawnRestriction(middleListing, task.HumanlikeIngredient, true);
            middleListing.Gap();
            IngredientPawnRestriction(middleListing, task.AnimalIngredient, false);
            middleListing.End();

            Listing_Standard rightListing = new Listing_Standard();
            rightListing.Begin(rect4);
            DrawThingsFilter(rightListing);
            rightListing.End();
            //Listing_Standard listing_Standard4 = listing_Standard.BeginSection((float)Dialog_Bill_ReligionActivityConfig.WorkerSelectionSubdialogHeight);

            //listing_Standard.EndSection(listing_Standard4);


            //listing_Standard.Begin(rect4);
            //Listing_Standard listing_Standard3 = listing_Standard.BeginSection(100f);
            //ListOfThings(listing_Standard3.GetRect(90));
            //listing_Standard.EndSection(listing_Standard3);
            //listing_Standard.End();
        }

        private void DrawInfo(Listing_Standard holder)
        {
            Listing_Standard listing_Standard = holder.BeginSection(100f);
            if (this.task.Suspended)
            {
                if (listing_Standard.ButtonText("Suspended".Translate(), (string)null))
                {
                    this.task.Suspended = false;
                    SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
                }
            }
            else if (listing_Standard.ButtonText("NotSuspended".Translate(), (string)null))
            {
                this.task.Suspended = true;
                SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
            }
            StringBuilder stringBuilder = new StringBuilder();
            if (this.task.Description != null)
            {
                stringBuilder.AppendLine(this.task.Description);
                stringBuilder.AppendLine();
            }
            Verse.Text.Font = GameFont.Small;
            string str4 = stringBuilder.ToString();
            listing_Standard.Label(str4, -1f, (string)null);
            Verse.Text.Font = GameFont.Small;
            holder.EndSection(listing_Standard);
        }

        #region PawnRestriction
        private void PawnRestriction(Listing_Standard holder)
        {
            Listing_Standard listing_Standard = holder.BeginSection(100f);
            listing_Standard.Label("RoR_ActivityPawnRestriction".Translate());
            Widgets.Dropdown<ActivityTask, Pawn>(listing_Standard.GetRect(30f), this.task, (ActivityTask b) => b.PawnRestriction, (ActivityTask b) => this.GeneratePawnRestrictionOptions(), (this.task.PawnRestriction != null) ? this.task.PawnRestriction.LabelShortCap : "AnyWorker".Translate(), null, null, null, null, false);
            holder.EndSection(listing_Standard);
        }

        private IEnumerable<Widgets.DropdownMenuElement<Pawn>> GeneratePawnRestrictionOptions()
        {
            yield return new Widgets.DropdownMenuElement<Pawn>
            {
                option = new FloatMenuOption("AnyWorker".Translate(), delegate
                {
                    this.task.PawnRestriction = null;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = null
            };
            //SkillDef workSkill = this.bill.recipe.workSkill;
            IEnumerable<Pawn> pawns = PawnsFinder.AllMaps_FreeColonists;
            //pawns = from pawn in pawns
            //        orderby pawn.LabelShortCap
            //        select pawn;
            //if (workSkill != null)
            //{
            //    pawns = from pawn in pawns
            //            orderby pawn.skills.GetSkill(this.bill.recipe.workSkill).Level descending
            //            select pawn;
            //}
            WorkGiverDef workGiver = GetWorkgiver();
            if (workGiver == null)
            {
                Log.ErrorOnce("Generating pawn restrictions for a BillGiver without a Workgiver", 96455148, false);
            }
            else
            {
                pawns = from pawn in pawns
                        orderby pawn.workSettings.WorkIsActive(workGiver.workType) descending
                        select pawn;
                pawns = from pawn in pawns
                        orderby pawn.story.WorkTypeIsDisabled(workGiver.workType)
                        select pawn;
                foreach (Pawn pawn in pawns)
                {
                    if (pawn == task.HumanlikeIngredient.ConcretePawn)
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "Religion_Unavaliable".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                    }
                    if (pawn.GetReligionComponent().Religion != task.Facility.AssignedReligion)
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "Religion_WrongReligion".Translate(pawn.GetReligionComponent().Religion.Label)), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    if (pawn.story.WorkTypeIsDisabled(workGiver.workType))
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "WillNever".Translate(workGiver.verb)), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    //if (this.bill.recipe.workSkill != null && !pawn.workSettings.WorkIsActive(workGiver.workType))
                    //{
                    //    yield return new Widgets.DropdownMenuElement<Pawn>
                    //    {
                    //        option = new FloatMenuOption(string.Format("{0} ({1} {2}, {3})", new object[]
                    //        {
                    //            pawn.LabelShortCap,
                    //            pawn.skills.GetSkill(this.bill.recipe.workSkill).Level,
                    //            this.bill.recipe.workSkill.label,
                    //            "NotAssigned".Translate()
                    //        }), delegate
                    //        {
                    //            this.task.PawnRestriction = pawn;
                    //        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    //        payload = pawn
                    //    };
                    //    continue;
                    //}
                    if (!pawn.workSettings.WorkIsActive(workGiver.workType))
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "NotAssigned".Translate()), delegate
                            {
                                this.task.PawnRestriction = pawn;
                            }, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    yield return new Widgets.DropdownMenuElement<Pawn>
                    {
                        option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate
                        {
                            this.task.PawnRestriction = pawn;
                        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                        payload = pawn
                    };
                }
            }
        }
        #endregion

        private void IngredientPawnRestriction(Listing_Standard holder, IngredientPawn ingredientPawn, bool isHumanlike)
        {
            Listing_Standard listing_Standard = holder.BeginSection(100f);
            string label = null;
            if (isHumanlike)
                label = "RoR_ActivityPawnHumanlike".Translate();
            else
                label = "RoR_ActivityPawnAnimal".Translate();
            listing_Standard.Label(label.Translate());
            Widgets.Dropdown<IngredientPawn, bool>(listing_Standard.GetRect(30f), ingredientPawn, (IngredientPawn b) => b.PartOfColony, (IngredientPawn b) => this.GenerateBoolOptions(ingredientPawn), ingredientPawn.PartOfColony.ToString().Translate(), null, null, null, null, false);
            Widgets.Dropdown<IngredientPawn, Pawn>(listing_Standard.GetRect(30f), ingredientPawn, (IngredientPawn b) => b.ConcretePawn, (IngredientPawn b) => this.GenerateIngredientPawnRestrictionOptions(ingredientPawn, isHumanlike), (ingredientPawn.ConcretePawn != null) ? ingredientPawn.ConcretePawn.LabelShortCap : "AnyWorker".Translate(), null, null, null, null, false);
            holder.EndSection(listing_Standard);
        }

        private IEnumerable<Widgets.DropdownMenuElement<bool>> GenerateBoolOptions(IngredientPawn ingredientPawn)
        {
            yield return new Widgets.DropdownMenuElement<bool>
            {
                option = new FloatMenuOption("True".Translate(), delegate
                {
                    ingredientPawn.PartOfColony = true;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = true
            };
            yield return new Widgets.DropdownMenuElement<bool>
            {
                option = new FloatMenuOption("False".Translate(), delegate
                {
                    ingredientPawn.PartOfColony = false;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = false
            };
        }

        private IEnumerable<Widgets.DropdownMenuElement<Pawn>> GenerateIngredientPawnRestrictionOptions(IngredientPawn ingredientPawn, bool isHumanlike)
        {
            yield return new Widgets.DropdownMenuElement<Pawn>
            {
                option = new FloatMenuOption("AnyWorker".Translate(), delegate
                {
                    ingredientPawn.ConcretePawn = null;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = null
            };
            IEnumerable<Pawn> pawns = task.Facility.Map.mapPawns.AllPawnsSpawned;

            if (isHumanlike)
                pawns = pawns.Where(x => x.RaceProps.Humanlike);
            else
                pawns = pawns.Where(x => x.RaceProps.Animal);

            foreach (Pawn pawn in pawns)
            {
                if (task.ThingFilter.Allows(pawn.def) && (!ingredientPawn.PartOfColony || ingredientPawn.PartOfColony &&
                    ((pawn.RaceProps.Humanlike && pawn.IsPrisonerOfColony) || (pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer))))
                {
                    if (pawn == task.PawnRestriction)
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "Religion_Unavaliable".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                    else
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate
                            {
                                ingredientPawn.ConcretePawn = pawn;
                            }, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                }
            }
        }

        private WorkGiverDef GetWorkgiver()
        {
            List<WorkGiverDef> defsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
            for (int index = 0; index < defsListForReading.Count; ++index)
            {
                WorkGiverDef workGiverDef = defsListForReading[index];
                WorkGiver_DoReligionActivity worker = workGiverDef.Worker as WorkGiver_DoReligionActivity;
                if (worker != null)
                    return workGiverDef;
            }
            Log.ErrorOnce(string.Format("Can't find a WorkGiver for a BillGiver {0}", (object)task.Facility.ToString()), 57348750, false);
            return (WorkGiverDef)null;
        }

        private void DrawThingsFilter(Listing_Standard holder)
        {
            Listing_Standard listing_Standard = holder.BeginSection(320f);

            DrawThingsList(listing_Standard);

            listing_Standard.Gap();

            if (listing_Standard.ButtonText("ClearAll".Translate()))
            {
                task.ThingFilter.DisallowAll();
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera((Map)null);
            }
            if (listing_Standard.ButtonText("AllowAll".Translate()))
            {
                task.ThingFilter.AllowAll();
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera((Map)null);
            }

            holder.EndSection(listing_Standard);
        }

        private void DrawThingsList(Listing_Standard holder)
        {
            Listing_Standard list = new Listing_Standard();
            Rect outRect = holder.GetRect(200f);
            Rect viewRect = new Rect(outRect.x, outRect.y, outRect.width - 16f, task.ThingFilter.AvaliableThings.Count() * (Text.LineHeight + holder.verticalSpacing));
            list.BeginScrollView(outRect, ref thingFilterScrollPosition, ref viewRect);
            foreach (ThingDef def in task.ThingFilter.AvaliableThings)
            {
                bool isAllowed = task.ThingFilter.Allows(def);
                list.CheckboxLabeled(def.LabelCap, ref isAllowed);
                task.ThingFilter.SetAllowance(def, isAllowed);
            }
            list.EndScrollView(ref viewRect);
        }
    }
}
