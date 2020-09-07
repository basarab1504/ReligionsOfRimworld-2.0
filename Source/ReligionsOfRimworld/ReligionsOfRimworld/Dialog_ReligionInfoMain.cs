using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    class Dialog_ReligionInfoMain : Window
    {
        Religion religion;
        float currentY = 0;
        float padding = 50;

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(1150f, 760f);
            }
        }

        protected override float Margin
        {
            get
            {
                return 0.0f;
            }
        }

        public Dialog_ReligionInfoMain(Religion religion)
        {
            this.religion = religion;
            this.forcePause = true;
            //this.doCloseButton = true;
            this.doCloseX = true;
            this.absorbInputAroundWindow = true;
            this.closeOnClickedOutside = true;
            this.soundAppear = SoundDefOf.InfoCard_Open;
            this.soundClose = SoundDefOf.InfoCard_Close;
        }

        private void Template(Rect inner, string header, Action<Rect> action)
        {
            Rect labelRect = new Rect(inner);
            labelRect.height = Verse.Text.CalcHeight(header, inner.width);
            Widgets.Label(labelRect, header);
            action(inner.ContractedBy(20));
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect inner = new Rect(inRect).ContractedBy(18f);

            Rect tophalf = inner.TopHalf();
            Vector2 topHalfSize = tophalf.size / new Vector2(1, 3);

            Rect description = new Rect(tophalf.position, topHalfSize);
            Rect doctrines = new Rect(tophalf.position + new Vector2(0, topHalfSize.y), topHalfSize);
            Rect buildings = new Rect(tophalf.position + new Vector2(0, topHalfSize.y) * 2, topHalfSize);

            Rect criteriaRect = inner.BottomHalf().LeftHalf();
            Rect opinionRect = inner.BottomHalf().RightHalf();

            Template(description, religion.Label, x => Widgets.Label(x, religion.Description));

            //Template(doctrines, "Doctrines", x =>
            //{
            //    var settings = SettingsTagDefOf.AllSettings.Where(n => !(n is ReligionSettings_Social));
            //    int rows = settings.Count() / 5;

            //    Vector2 size = x.size / new Vector2(5, rows);

            //    float curX = x.position.x;
            //    float curY = x.position.y;

            //    float xMix = x.position.x;
            //    float xMax = x.xMax - size.x;

            //    foreach (var s in settings)
            //    {
            //        Widgets.Label(new Rect(new Vector2(curX,curY), size), s.Label);
            //        if (curX < xMax)
            //            curX += size.x;
            //        else
            //        {
            //            curX = xMix;
            //            curY += size.y;
            //        }
            //    }
            //});

            Template(buildings, "Building", x =>
            {
                ReligionSettings_AllowedBuildings settings = religion.GetSettings<ReligionSettings_AllowedBuildings>(SettingsTagDefOf.AllowedBuildingsTag);
                if (settings == null)
                {
                    Widgets.Label(x, "No buildings");
                }
                else
                {
                    x.height = 50;
                    x.width = settings.AllowedBuildings.Count() * 100;
                    float y = x.y;
                    x.center = inner.center;
                    x.y = y;
                    float curX = x.x;
                    foreach (var building in settings.AllowedBuildings)
                    {
                        Widgets.DrawTextureFitted(new Rect(curX, x.y, 50, 50), building.uiIcon, 1);
                        Widgets.Label(new Rect(curX, x.y + 60, 50, 50), building.LabelCap);
                        curX += 100;
                    }
                    currentY += 70;
                }
            });

            Template(criteriaRect, "Criteria", x =>
            {
                ReligionSettings_JoiningCriteria jSettings = religion.GetSettings<ReligionSettings_JoiningCriteria>(SettingsTagDefOf.JoiningCriteriaTag);
                if (jSettings == null)
                {
                    Widgets.Label(x, "No criteria");
                }
                else
                {
                    float localy = x.y;
                    foreach (var criteria in jSettings.Criteria.OrderByDescending(o => o.Importance))
                    {
                        Widgets.DrawTextureFitted(new Rect(x.x, localy, 20, 20), criteria.ShouldHave ? GraphicsCache.CheckboxOnTex : GraphicsCache.CheckboxOffTex, 1);
                        Rect i = new Rect(x.x + 25, localy, 80, 20);
                        GUI.color = Color.gray;
                        Widgets.Label(i, criteria.Importance.ToString());
                        GUI.color = Color.white;
                        Widgets.Label(new Rect(i.x + 100, localy, 1000, 20), criteria.Reason);
                        localy += 25;
                    }
                }
            });

            Template(opinionRect, "Opinion", x =>
            {
                ReligionSettings_Social oSettings = religion.GetSettings<ReligionSettings_Social>(SettingsTagDefOf.OpinionTag);
                if (oSettings == null)
                {
                    Widgets.Label(x, "No opinion");
                }
                else
                {
                    float localy = x.y;
                    if(oSettings.DefaultPropety != null)
                    {
                        var op = oSettings.DefaultPropety.Witness;
                        string offset = op == null ? "0" : $"{op.OpinionThought.stages[0].baseOpinionOffset} to {op.OpinionThought.stages.Last().baseOpinionOffset}";
                        GUI.color = op == null ? Color.white : op.OpinionThought.stages[0].baseOpinionOffset > 0 ? Color.green : Color.red;
                        Widgets.Label(new Rect(x.x, localy, 60, 20), offset);
                        GUI.color = Color.white;
                        Rect i = new Rect(x.x + 65, localy, 80, 20);
                        GUI.color = Color.gray;
                        Widgets.Label(i, oSettings.DefaultPropety.PawnCategory.ToString());
                        GUI.color = Color.white;
                        Widgets.Label(new Rect(x.x + 160, localy, 1000, 20), "Everyone");
                        localy += 25;
                    }
                    foreach (var opinion in oSettings.Properties)
                    {
                        var op = opinion.Witness;
                        string offset = op == null ? "0" : $"{op.OpinionThought.stages[0].baseOpinionOffset} to {op.OpinionThought.stages.Last().baseOpinionOffset}";
                        GUI.color = op == null ? Color.white : op.OpinionThought.stages[0].baseOpinionOffset > 0 ? Color.green : Color.red;
                        Widgets.Label(new Rect(x.x, localy, 60, 20), offset);
                        GUI.color = Color.white;
                        Rect i = new Rect(x.x + 65, localy, 80, 20);
                        GUI.color = Color.gray;
                        Widgets.Label(i, opinion.PawnCategory.ToString());
                        GUI.color = Color.white;
                        Widgets.Label(new Rect(x.x + 160, localy, 1000, 20), opinion.GetObject().LabelCap);
                        localy += 25;
                    }
                }
            });
        }

        private float GetY()
        {
            currentY += padding;
            return currentY;
        }
    }
}
