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
        private static float listHeight;
        private static Vector2 scrollPosition;
        private Dialog_InfoCard.InfoCardTab tab;

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(1150f, 900f);
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
            scrollPosition = new Vector2();
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

        private void Template(Rect inner, string header, int rows, int columns, IEnumerable<Action<Rect>> actions)
        {
            Rect labelRect = new Rect(inner);
            labelRect.height = Verse.Text.CalcHeight(header, inner.width);
            Widgets.Label(labelRect, header);

            Rect toAction = inner.ContractedBy(20);

            Vector2 size = new Vector2(toAction.width / columns, toAction.height / rows);
            float curX = toAction.x;
            float curY = toAction.y;
            float offset = 10f;

            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    actions.ElementAt(columns * i + j)(new Rect(new Vector2(curX, curY), size));
                    curX += size.x;
                }
                curY += size.y;
                curX = toAction.x;
            }
        }
        private void Template(Rect inner, int rows, int columns, IEnumerable<Action<Rect>> actions)
        {
            Vector2 size = new Vector2(inner.width / columns, inner.height / rows);
            float curX = inner.x;
            float curY = inner.y;
            float offset = 10f;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    actions.ElementAt(columns * i + j)(new Rect(new Vector2(curX, curY), size));
                    curX += size.x;
                }
                curY += size.y;
                curX = inner.x;
            }
        }

        private void Draw(Rect example, float xOffset, ref float y, params string[] strings)
        {
            for(int i = 0; i < strings.Count(); i++)
            {
                Rect r = new Rect(example);
                r.x = r.x + xOffset * i;
                r.width -= 10;
                Widgets.Label(r, strings.ElementAt(i));
            }
            y += 40;
        }

        private string GetRange(PietyDef def)
        {
            return $"{def.Stages.First().PietyOffset} to {def.Stages.Last().PietyOffset}";
        }

        private string GetRange(ThoughtDef def)
        {
            return $"{def.stages.First().baseMoodEffect} to {def.stages.Last().baseMoodEffect}";
        }

        private void Fill(Rect inner)
        {
            if (tab == Dialog_InfoCard.InfoCardTab.Stats)
            {
                Rect tophalf = inner.TopHalf();
                Vector2 topHalfSize = tophalf.size / new Vector2(1, 3);

                Rect description = new Rect(tophalf.position, topHalfSize);
                Rect doctrines = new Rect(tophalf.position + new Vector2(0, topHalfSize.y), topHalfSize);
                Rect buildings = new Rect(tophalf.position + new Vector2(0, topHalfSize.y) * 2, topHalfSize);

                Rect bottomHalf = inner.BottomHalf();

                Rect criteriaRect = bottomHalf.LeftHalf();
                Rect opinionRect = bottomHalf.RightHalf();

                //Template(description, religion.Label, x => Widgets.Label(x, religion.Description));
                Template(description, religion.Label, 1, 1, new Action<Rect>[1] { x => Widgets.Label(x, religion.Description) } );

                Template(doctrines, "Doctrines", 2, 3, Doctrines());
                Template(buildings, "Buildings", 1, Buildings().Count(), Buildings());
                Template(criteriaRect, "Criteria", Criteria().Count(), 1, Criteria());
                Template(opinionRect, "Opinion", Opinion().Count(), 1, Opinion());

                //Template(buildings, "Building", x =>
                //{
                //    ReligionSettings_AllowedBuildings settings = religion.GetSettings<ReligionSettings_AllowedBuildings>(SettingsTagDefOf.AllowedBuildingsTag);
                //    if (settings == null)
                //    {
                //        Widgets.Label(x, "No buildings");
                //    }
                //    else
                //    {
                //        x.height = 50;
                //        x.width = settings.AllowedBuildings.Count() * 100;
                //        float y = x.y;
                //        x.center = inner.center;
                //        x.y = y;
                //        float curX = x.x;
                //        foreach (var building in settings.AllowedBuildings)
                //        {
                //            Widgets.DrawTextureFitted(new Rect(curX, x.y, 50, 50), building.uiIcon, 1);
                //            Widgets.Label(new Rect(curX, x.y + 60, 50, 50), building.LabelCap);
                //            curX += 100;
                //        }
                //        currentY += 70;
                //    }
                //});

                //Template(opinionRect, "Opinion", x =>
                //{
                //    ReligionSettings_Social oSettings = religion.GetSettings<ReligionSettings_Social>(SettingsTagDefOf.OpinionTag);
                //    if (oSettings == null)
                //    {
                //        Widgets.Label(x, "No opinion");
                //    }
                //    else
                //    {
                //        float localy = x.y;
                //        if (oSettings.DefaultPropety != null)
                //        {
                //            var op = oSettings.DefaultPropety.Witness;
                //            string offset = op == null ? "0" : $"{op.OpinionThought.stages[0].baseOpinionOffset} to {op.OpinionThought.stages.Last().baseOpinionOffset}";
                //            GUI.color = op == null ? Color.white : op.OpinionThought.stages[0].baseOpinionOffset > 0 ? Color.green : Color.red;
                //            Widgets.Label(new Rect(x.x, localy, 60, 20), offset);
                //            GUI.color = Color.white;
                //            Rect i = new Rect(x.x + 65, localy, 80, 20);
                //            GUI.color = Color.gray;
                //            Widgets.Label(i, oSettings.DefaultPropety.PawnCategory.ToString());
                //            GUI.color = Color.white;
                //            Widgets.Label(new Rect(x.x + 160, localy, 1000, 20), "Everyone");
                //            localy += 25;
                //        }
                //        foreach (var opinion in oSettings.Properties)
                //        {
                //            var op = opinion.Witness;
                //            string offset = op == null ? "0" : $"{op.OpinionThought.stages[0].baseOpinionOffset} to {op.OpinionThought.stages.Last().baseOpinionOffset}";
                //            GUI.color = op == null ? Color.white : op.OpinionThought.stages[0].baseOpinionOffset > 0 ? Color.green : Color.red;
                //            Widgets.Label(new Rect(x.x, localy, 60, 20), offset);
                //            GUI.color = Color.white;
                //            Rect i = new Rect(x.x + 65, localy, 80, 20);
                //            GUI.color = Color.gray;
                //            Widgets.Label(i, opinion.PawnCategory.ToString());
                //            GUI.color = Color.white;
                //            Widgets.Label(new Rect(x.x + 160, localy, 1000, 20), opinion.GetObject().LabelCap);
                //            localy += 25;
                //        }
                //    }
                //});
            }
            else if (tab == Dialog_InfoCard.InfoCardTab.Records)
            {
                Template(inner, "Social", 1, 1, S());
                //Template(inner, "Social", x =>
                //{
                //    IEnumerable<ReligionSettings> sSettings = religion.AllSettings.Where(g => g is ReligionSettings_Social);
                //    if(sSettings == null)
                //    {
                //        Widgets.Label(x, "No social");
                //    }
                //    else
                //    {
                //        float offset = x.width / 7;
                //        float curY = x.y;


                //        Draw(x, offset, ref curY, "cause", "category", "pawnCategory", "pietyAsSubject", "thoughtAsSubject", "pietyAsWitness", "thoughtAsWitness");

                //        float height = 0;

                //        Rect outR = new Rect(x.x, curY, x.width, x.height);
                //        Rect viewR = new Rect(x.x, curY, x.width - 16f, listHeight);
                //        Widgets.BeginScrollView(outR, ref scrollPosition, viewR, true);

                //        ReligionSettings_ReligionActivity aSettings = religion.GetSettings<ReligionSettings_ReligionActivity>(SettingsTagDefOf.ActivityTag);
                //        if(aSettings != null)
                //        {
                //            foreach (var p in aSettings.Properties)
                //            {
                //                Draw(new Rect(viewR.x, curY, offset, viewR.height), offset, ref curY,
                //                    p.GetObject() != null ? p.GetObject().LabelCap.ToString() : " ",
                //                    aSettings.Tag.LabelCap,
                //                    p.PawnCategory.ToString(),
                //                    p.Subject?.Piety != null ? GetRange(p.Subject.Piety) : "-",
                //                    p.Subject?.Thought != null ? GetRange(p.Subject.Thought) : "-",
                //                    p.Witness?.Piety != null ? GetRange(p.Witness.Piety) : "-",
                //                    p.Witness?.Thought != null ? GetRange(p.Witness.Thought) : "-");
                //                height += 40;
                //            }
                //        }
                //        foreach (ReligionSettings_Social s in religion.AllSettings.Where(g => g is ReligionSettings_Social))
                //        {
                //            if (s.DefaultPropety != null)
                //            {
                //                Draw(new Rect(viewR.x, curY, offset, viewR.height), offset, ref curY,
                //                    "Default",
                //                    s.Tag.LabelCap,
                //                    s.DefaultPropety.PawnCategory.ToString(),
                //                    s.DefaultPropety.Subject?.Piety != null ? GetRange(s.DefaultPropety.Subject.Piety) : "-",
                //                    s.DefaultPropety.Subject?.Thought != null ? GetRange(s.DefaultPropety.Subject.Thought) : "-",
                //                    s.DefaultPropety.Witness?.Piety != null ? GetRange(s.DefaultPropety.Witness.Piety) : "-",
                //                    s.DefaultPropety.Witness?.Thought != null ? GetRange(s.DefaultPropety.Witness.Thought) : "-");
                //                height += 40;
                //            }
                //            foreach (var p in s.Properties)
                //            {
                //                Draw(new Rect(viewR.x, curY, offset, viewR.height), offset, ref curY,
                //                    p.GetObject() != null ? p.GetObject().LabelCap.ToString() : " ",
                //                    s.Tag.LabelCap,
                //                    p.PawnCategory.ToString(),
                //                    p.Subject?.Piety != null ? GetRange(p.Subject.Piety) : "-",
                //                    p.Subject?.Thought != null ? GetRange(p.Subject.Thought) : "-",
                //                    p.Witness?.Piety != null ? GetRange(p.Witness.Piety) : "-",
                //                    p.Witness?.Thought != null ? GetRange(p.Witness.Thought) : "-");
                //                height += 40;
                //            }
                //        }
                //        Widgets.EndScrollView();
                //        listHeight = height;
                //    }
                //});
            }
        }

        public override void DoWindowContents(Rect inRect)
        {

            List<TabRecord> tabs = new List<TabRecord>();
            TabRecord item = new TabRecord("Main".Translate(), delegate ()
            {
                this.tab = Dialog_InfoCard.InfoCardTab.Stats;
            }, this.tab == Dialog_InfoCard.InfoCardTab.Stats);
            TabRecord item2 = new TabRecord("Social".Translate(), delegate ()
            {
                this.tab = Dialog_InfoCard.InfoCardTab.Records;
            }, this.tab == Dialog_InfoCard.InfoCardTab.Records);
            tabs.Add(item);
            tabs.Add(item2);


            Rect tabRect = inRect.ContractedBy(18f);
            tabRect.yMin += 45f;
            TabDrawer.DrawTabs(tabRect, tabs, 200f);

            Fill(tabRect.ContractedBy(18));
        }

        private float GetY()
        {
            currentY += padding;
            return currentY;
        }

        private IEnumerable<Action<Rect>> Buildings()
        {
            ReligionSettings_AllowedBuildings settings = religion.GetSettings<ReligionSettings_AllowedBuildings>(SettingsTagDefOf.AllowedBuildingsTag);
            if (settings == null)
            {
                yield return x => Widgets.Label(x, "No buildings");
            }
            else
            {
                //x.height = 50;
                //x.width = settings.AllowedBuildings.Count() * 100;
                //float y = x.y;
                //x.center = inner.center;
                //x.y = y;
                //float curX = x.x;
                foreach (var building in settings.AllowedBuildings)
                {
                    yield return x =>
                    {
                        Widgets.DrawTextureFitted(x, building.uiIcon, 1);
                        //Widgets.Label(new Rect(curX, x.y + 60, 50, 50), building.LabelCap);
                        //curX += 100;
                    };
                }
                //currentY += 70;
            }
        }

        private IEnumerable<Action<Rect>> Criteria()
        {
            ReligionSettings_JoiningCriteria jSettings = religion.GetSettings<ReligionSettings_JoiningCriteria>(SettingsTagDefOf.JoiningCriteriaTag);
            if (jSettings == null)
            {
                yield return x => Widgets.Label(x, "No criteria");
            }
            else
            {
                foreach (var criteria in jSettings.Criteria.OrderByDescending(o => o.Importance))
                {
                    yield return x =>
                    {
                        Vector2 size = new Vector2(x.width / 3, x.height);
                        Widgets.DrawTextureFitted(new Rect(x.x, x.y, size.x, size.y), criteria.ShouldHave ? GraphicsCache.CheckboxOnTex : GraphicsCache.CheckboxOffTex, 1);
                        GUI.color = Color.gray;
                        Widgets.Label(new Rect(x.x + size.x, x.y, size.x, size.y), criteria.Importance.ToString());
                        GUI.color = Color.white;
                        Widgets.Label(new Rect(x.x + size.x * 2, x.y, size.x, size.y), criteria.Reason);
                    };
                }
            }
        }

        private IEnumerable<Action<Rect>> Opinion()
        {
            ReligionSettings_Social oSettings = religion.GetSettings<ReligionSettings_Social>(SettingsTagDefOf.OpinionTag);
            if (oSettings == null)
            {
                yield return x => Widgets.Label(x, "No opinion");
            }
            else
            {
                if (oSettings.DefaultPropety != null)
                {
                    yield return x =>
                    {
                        Vector2 size = new Vector2(x.width / 3, x.height);
                        var op = oSettings.DefaultPropety.Witness;

                        string offset = op == null ? "0" : $"{op.OpinionThought.stages[0].baseOpinionOffset} to {op.OpinionThought.stages.Last().baseOpinionOffset}";

                        GUI.color = op == null ? Color.white : op.OpinionThought.stages[0].baseOpinionOffset > 0 ? Color.green : Color.red;
                        Widgets.Label(new Rect(x.x, x.y, size.x, size.y), offset);
                        GUI.color = Color.white;
                        GUI.color = Color.gray;
                        Widgets.Label(new Rect(x.x + size.x, x.y, size.x, size.y), oSettings.DefaultPropety.PawnCategory.ToString());
                        GUI.color = Color.white;
                        Widgets.Label(new Rect(x.x + size.x * 2, x.y, size.x, size.y), "Everyone");
                    };
                }
                foreach (var opinion in oSettings.Properties)
                {
                    yield return x =>
                    {
                        Vector2 size = new Vector2(x.width / 3, x.height);
                        var op = opinion.Witness;

                        string offset = op == null ? "0" : $"{op.OpinionThought.stages[0].baseOpinionOffset} to {op.OpinionThought.stages.Last().baseOpinionOffset}";

                        GUI.color = op == null ? Color.white : op.OpinionThought.stages[0].baseOpinionOffset > 0 ? Color.green : Color.red;
                        Widgets.Label(new Rect(x.x, x.y, size.x, size.y), offset);
                        GUI.color = Color.white;
                        GUI.color = Color.gray;
                        Widgets.Label(new Rect(x.x + size.x, x.y, size.x, size.y), oSettings.DefaultPropety.PawnCategory.ToString());
                        GUI.color = Color.white;
                        Widgets.Label(new Rect(x.x + size.x * 2, x.y, size.x, size.y), "Everyone");
                    };
                }
            }
        }

        private IEnumerable<Action<Rect>> Doctrines()
        {
            List<Tuple<string, bool>> settings = new List<Tuple<string, bool>>();
            settings.Add(new Tuple<string, bool>("Have piety", religion.GetSettings(SettingsTagDefOf.NeedTag) != null));
            settings.Add(new Tuple<string, bool>("Can converts through talks", religion.GetSettings(SettingsTagDefOf.TalksTag) != null));
            settings.Add(new Tuple<string, bool>("Can converts through incidents", religion.GetSettings(SettingsTagDefOf.IncidentsTag) != null));
            settings.Add(new Tuple<string, bool>("Can converts through breakdowns", religion.GetSettings(SettingsTagDefOf.MentalBreaksTag) != null));
            settings.Add(new Tuple<string, bool>("Can pray", religion.GetSettings(SettingsTagDefOf.PrayingsTag) != null));
            settings.Add(new Tuple<string, bool>("Have actitvites", religion.GetSettings(SettingsTagDefOf.ActivityTag) != null));

            foreach (var s in settings)
            {
                yield return x =>
                {
                    Vector2 size = x.size / 2;
                    Widgets.Label(new Rect(x.x, x.y, size.x, size.y), s.Item1);
                    GUI.color = s.Item2 ? Color.green : Color.red;
                    Widgets.Label(new Rect(x.x, x.y + size.y, size.x, size.y), s.Item2.ToString());
                    GUI.color = Color.white;
                };
            }
        }

        private IEnumerable<Action<Rect>> S()
        {
            IEnumerable<ReligionSettings> sSettings = religion.AllSettings.Where(g => g is ReligionSettings_Social);
            if (sSettings == null)
            {
                yield return x => Widgets.Label(x, "No social");
            }
            else
            {
                yield return z =>
                {
                    Template(z, 1, 7, new Action<Rect>[7]
                    {
                                u => Widgets.Label(u, "cause"),
                                u => Widgets.Label(u, "settings"),
                                u => Widgets.Label(u, "category"),
                                u => Widgets.Label(u, "subjectPiety"),
                                u => Widgets.Label(u, "subjectThought"),
                                u => Widgets.Label(u, "witnessPiety"),
                                u => Widgets.Label(u, "witnessThought"),
                    });
                    Rect outR = new Rect(z.x, z.y + 50, z.width, z.height);
                    Rect viewR = new Rect(z.x, z.y + 50, z.width - 16, z.height - 50);
                    Widgets.BeginScrollView(outR, ref scrollPosition, viewR, true);
                    Template(viewR, AAA().Count(), 1, AAA());
                    Widgets.EndScrollView();
                };

            }
        }

        private IEnumerable<Action<Rect>> AAA()
        {
            foreach (ReligionSettings_Social s in religion.AllSettings.Where(g => g is ReligionSettings_Social))
            {
                if (s.DefaultPropety != null)
                {
                    yield return z =>
                    {
                        Template(z, 1, 7, new Action<Rect>[7]
                        {
                                u => Widgets.Label(u, "Default"),
                                u => Widgets.Label(u, s.Tag.LabelCap),
                                u => Widgets.Label(u, s.DefaultPropety.PawnCategory.ToString()),
                                u => Widgets.Label(u, s.DefaultPropety.Subject?.Piety != null ? GetRange(s.DefaultPropety.Subject.Piety) : "-"),
                                u => Widgets.Label(u, s.DefaultPropety.Subject?.Thought != null ? GetRange(s.DefaultPropety.Subject.Thought) : "-"),
                                u => Widgets.Label(u, s.DefaultPropety.Witness?.Piety != null ? GetRange(s.DefaultPropety.Witness.Piety) : "-"),
                                u => Widgets.Label(u, s.DefaultPropety.Witness?.Thought != null ? GetRange(s.DefaultPropety.Witness.Thought) : "-"),
                        });
                    };
                }
                foreach (var p in s.Properties)
                {
                    yield return z =>
                    {
                        Template(z, 1, 7, new Action<Rect>[7]
                        {
                                u => Widgets.Label(u, p.GetObject().LabelCap),
                                u => Widgets.Label(u, s.Tag.LabelCap),
                                u => Widgets.Label(u, p.PawnCategory.ToString()),
                                u => Widgets.Label(u, p.Subject?.Piety != null ? GetRange(p.Subject.Piety) : "-"),
                                u => Widgets.Label(u, p.Subject?.Thought != null ? GetRange(p.Subject.Thought) : "-"),
                                u => Widgets.Label(u, p.Witness?.Piety != null ? GetRange(p.Witness.Piety) : "-"),
                                u => Widgets.Label(u, p.Witness?.Thought != null ? GetRange(p.Witness.Thought) : "-"),
                        });
                    };
                }
            }
        }

        //private Action<Rect> Social()
        //{
        //    IEnumerable<ReligionSettings> sSettings = religion.AllSettings.Where(g => g is ReligionSettings_Social);
        //    if (sSettings == null)
        //    {
        //        return x => Widgets.Label(x, "No social");
        //    }
        //    else
        //    {
        //        return x =>
        //        {
        //            //float offset = x.width / 7;
        //            //float curY = x.y;


        //            //Draw(x, offset, ref curY, "cause", "category", "pawnCategory", "pietyAsSubject", "thoughtAsSubject", "pietyAsWitness", "thoughtAsWitness");

        //            float height = 0;

        //            //Rect outR = new Rect(x.x, curY, x.width, x.height);
        //            //Rect viewR = new Rect(x.x, curY, x.width - 16f, listHeight);
        //            //Widgets.BeginScrollView(outR, ref scrollPosition, viewR, true);

        //            List<Action<Rect>> acts = new List<Action<Rect>>();

        //            ReligionSettings_ReligionActivity aSettings = religion.GetSettings<ReligionSettings_ReligionActivity>(SettingsTagDefOf.ActivityTag);
        //            if (aSettings != null)
        //            {
        //                foreach (var p in aSettings.Properties)
        //                {
        //                    acts.Add(z =>
        //                    {
        //                        Draw(z, offset, ref curY,
        //                            p.GetObject() != null ? p.GetObject().LabelCap.ToString() : " ",
        //                            aSettings.Tag.LabelCap,
        //                            p.PawnCategory.ToString(),
        //                            p.Subject?.Piety != null ? GetRange(p.Subject.Piety) : "-",
        //                            p.Subject?.Thought != null ? GetRange(p.Subject.Thought) : "-",
        //                            p.Witness?.Piety != null ? GetRange(p.Witness.Piety) : "-",
        //                            p.Witness?.Thought != null ? GetRange(p.Witness.Thought) : "-");
        //                        height += 40;
        //                    });
        //                }
        //            }
        //            foreach (ReligionSettings_Social s in religion.AllSettings.Where(g => g is ReligionSettings_Social))
        //            {
        //                if (s.DefaultPropety != null)
        //                {
        //                    acts.Add(z =>
        //                    {
        //                        Draw(z, offset, ref curY,
        //                            "Default",
        //                            s.Tag.LabelCap,
        //                            s.DefaultPropety.PawnCategory.ToString(),
        //                            s.DefaultPropety.Subject?.Piety != null ? GetRange(s.DefaultPropety.Subject.Piety) : "-",
        //                            s.DefaultPropety.Subject?.Thought != null ? GetRange(s.DefaultPropety.Subject.Thought) : "-",
        //                            s.DefaultPropety.Witness?.Piety != null ? GetRange(s.DefaultPropety.Witness.Piety) : "-",
        //                            s.DefaultPropety.Witness?.Thought != null ? GetRange(s.DefaultPropety.Witness.Thought) : "-");
        //                        height += 40;
        //                    });

        //                }
        //                foreach (var p in s.Properties)
        //                {
        //                    acts.Add(z =>
        //                    {
        //                        Draw(new Rect(viewR.x, curY, offset, viewR.height), offset, ref curY,
        //                            p.GetObject() != null ? p.GetObject().LabelCap.ToString() : " ",
        //                            s.Tag.LabelCap,
        //                            p.PawnCategory.ToString(),
        //                            p.Subject?.Piety != null ? GetRange(p.Subject.Piety) : "-",
        //                            p.Subject?.Thought != null ? GetRange(p.Subject.Thought) : "-",
        //                            p.Witness?.Piety != null ? GetRange(p.Witness.Piety) : "-",
        //                            p.Witness?.Thought != null ? GetRange(p.Witness.Thought) : "-");
        //                        height += 40;
        //                    });
        //                }
        //            }
        //            Template(x, "", acts.Count, 7, acts);
        //            Widgets.EndScrollView();
        //            listHeight = height;
        //        };
        //    }
        //}
    }
}
