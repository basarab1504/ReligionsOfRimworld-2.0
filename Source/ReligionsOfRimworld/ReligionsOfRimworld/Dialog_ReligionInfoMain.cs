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
        float height = 30;
        float doubleHeight = 38;
        float offset = 10;
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

        private void Template(Rect inner, string header, int rows, int columns, IEnumerable<Action<Rect>> actions, float height = 0)
        {
            Rect labelRect = new Rect(inner);
            labelRect.height = 40;
            Text.Font = GameFont.Medium;
            Widgets.Label(labelRect, header);
            Text.Font = GameFont.Small;

            Template(inner.ContractedBy(30), rows, columns, actions, height);
        }

        private void Template(Rect inner, int rows, int columns, IEnumerable<Action<Rect>> actions, float height = 0)
        {
            height = height == 0 ? inner.height / rows : height;
            Vector2 size = new Vector2(inner.width / columns, height);
            float curX = inner.x;
            float curY = inner.y;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    actions.ElementAt(columns * i + j)(new Rect(new Vector2(curX, curY), size));
                    curX += size.x;
                }
                curY += height + offset;
                curX = inner.x;
            }
        }

        private string GetRange(PietyDef def, out Color color)
        {
            color = def.Stages.First().PietyOffset >= 0 ? Color.green : Color.red;
            return $"{def.Stages.First().PietyOffset} {"Religion_To".Translate()} {def.Stages.Last().PietyOffset}";
        }

        private string GetRange(ThoughtDef def, out Color color)
        {
            color = def.stages.First().baseMoodEffect >= 0 ? Color.green : Color.red;
            return $"{def.stages.First().baseMoodEffect} {"Religion_To".Translate()} {def.stages.Last().baseMoodEffect}";
        }

        private string GetOpinionRange(ThoughtDef def, out Color color)
        {
            color = def.stages.First().baseOpinionOffset >= 0 ? Color.green : Color.red;
            return $"{def.stages.First().baseOpinionOffset} {"Religion_To".Translate()} {def.stages.Last().baseOpinionOffset}";
        }

        public override void DoWindowContents(Rect inRect)
        {

            List<TabRecord> tabs = new List<TabRecord>();
            TabRecord item = new TabRecord("Religion_Main".Translate(), delegate ()
            {
                this.tab = Dialog_InfoCard.InfoCardTab.Stats;
            }, this.tab == Dialog_InfoCard.InfoCardTab.Stats);
            TabRecord item2 = new TabRecord("Religion_SocialSettings".Translate(), delegate ()
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

                Template(description, religion.Label, 1, 1, new Action<Rect>[1] { x => Widgets.Label(x, religion.Description) });

                Template(doctrines, "Religion_Doctrines".Translate(), 2, 3, Doctrines());
                Template(buildings, "Religion_Buildings".Translate(), 1, Buildings().Count(), Buildings());
                Template(criteriaRect, "Religion_Criteria".Translate(), Criteria().Count(), 1, Criteria(), height);
                Template(opinionRect, "Religion_Opinion".Translate(), Opinion().Count(), 1, Opinion(), height);
            }
            else if (tab == Dialog_InfoCard.InfoCardTab.Records)
            {
                Template(inner, "Religion_SocialSettings".Translate(), 1, 1, SocialSettingsHeader());
            }
        }

        private IEnumerable<Action<Rect>> Buildings()
        {
            ReligionSettings_AllowedBuildings settings = religion.GetSettings<ReligionSettings_AllowedBuildings>(SettingsTagDefOf.AllowedBuildingsTag);
            if (settings == null)
            {
                yield return x => Widgets.Label(x, "Religion_NoSuchSettings".Translate());
            }
            else
            {
                foreach (var building in settings.AllowedBuildings)
                {
                    yield return x =>
                    {
                        Widgets.DrawTextureFitted(x, building.uiIcon, 1);
                        TooltipHandler.TipRegion(x, (TipSignal)building.LabelCap);
                        if (Mouse.IsOver(x))
                            Widgets.DrawHighlight(x);
                    };
                }
            }
        }

        private IEnumerable<Action<Rect>> Criteria()
        {
            ReligionSettings_JoiningCriteria jSettings = religion.GetSettings<ReligionSettings_JoiningCriteria>(SettingsTagDefOf.JoiningCriteriaTag);
            if (jSettings == null)
            {
                yield return x => Widgets.Label(x, "Religion_NoSuchSettings".Translate());
            }
            else
            {
                foreach (var criteria in jSettings.Criteria.OrderByDescending(o => o.Importance))
                {
                    yield return x =>
                    {
                        Template(x, 1, 3, new Action<Rect>[3]
                        {
                            u =>
                            {
                                Widgets.DrawTextureFitted(u, criteria.ShouldHave ? GraphicsCache.CheckboxOnTex : GraphicsCache.CheckboxOffTex, 1);
                                TooltipHandler.TipRegion(u, (TipSignal)"ShouldHave".Translate());
                                if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                            },
                            u =>
                            {
                                GUI.color = Color.gray;
                                Widgets.Label(u, criteria.Importance.Label());
                                GUI.color = Color.white;
                                TooltipHandler.TipRegion(u, (TipSignal)"CriteriaImportance".Translate());
                                if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                            },
                            u => Widgets.Label(u, criteria.Reason)
                        }, height);
                    };
                }
            }
        }

        private IEnumerable<Action<Rect>> Opinion()
        {
            ReligionSettings_Social oSettings = religion.GetSettings<ReligionSettings_Social>(SettingsTagDefOf.OpinionTag);
            if (oSettings == null)
            {
                yield return x => Widgets.Label(x, "Religion_NoSuchSettings".Translate());
            }
            else
            {
                if (oSettings.DefaultPropety != null)
                {
                    yield return x =>
                    {
                        var op = oSettings.DefaultPropety.Witness;
                        Template(x, 1, 3, new Action<Rect>[3]
                        {
                            u => ColoredOpinionRange(u, op?.OpinionThought),
                            u =>
                            {
                                GUI.color = Color.gray;
                                Widgets.Label(u, oSettings.DefaultPropety.PawnCategory.Label());
                                GUI.color = Color.white;
                                TooltipHandler.TipRegion(u, (TipSignal)"Religion_PawnCategory".Translate());
                                if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                            },
                            u => Widgets.Label(u, "Religion_Everyone".Translate())
                    }, height);
                    };
                }
                foreach (var opinion in oSettings.Properties)
                {
                    yield return x =>
                    {
                        var op = opinion.Witness;
                        Template(x, 1, 3, new Action<Rect>[3]
                        {
                            u => ColoredOpinionRange(u, op?.OpinionThought),
                            u =>
                            {
                                GUI.color = Color.gray;
                                Widgets.Label(u, opinion.PawnCategory.Label());
                                GUI.color = Color.white;
                                TooltipHandler.TipRegion(u, (TipSignal)"Religion_PawnCategory".Translate());
                                if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                            },
                            u => Widgets.Label(u, opinion.GetObject().LabelCap)
                        }, height);
                    };
                }
            }
        }

        private IEnumerable<Action<Rect>> Doctrines()
        {
            List<Tuple<string, ReligionSettings>> settings = new List<Tuple<string, ReligionSettings>>();
            settings.Add(new Tuple<string, ReligionSettings>("Religion_HavePiety".Translate(), religion.GetSettings(SettingsTagDefOf.NeedTag)));
            settings.Add(new Tuple<string, ReligionSettings>("Religion_ConvertTalks".Translate(), religion.GetSettings(SettingsTagDefOf.TalksTag)));
            settings.Add(new Tuple<string, ReligionSettings>("Religion_ConvertIncidents".Translate(), religion.GetSettings(SettingsTagDefOf.IncidentsTag)));
            settings.Add(new Tuple<string, ReligionSettings>("Religion_ConvertMentalBreaks".Translate(), religion.GetSettings(SettingsTagDefOf.MentalBreaksTag)));
            settings.Add(new Tuple<string, ReligionSettings>("Religion_CanPray".Translate(), religion.GetSettings(SettingsTagDefOf.PrayingsTag)));
            settings.Add(new Tuple<string, ReligionSettings>("Religion_HaveActivities".Translate(), religion.GetSettings(SettingsTagDefOf.ActivityTag)));

            foreach (var s in settings)
            {
                yield return x =>
                {
                    Vector2 size = new Vector2(x.size.x, x.size.y / 2);
                    Widgets.Label(new Rect(x.x, x.y, size.x, size.y), s.Item1);
                    GUI.color = s.Item2 != null ? Color.green : Color.red;
                    Widgets.Label(new Rect(x.x, x.y + size.y, size.x, size.y), (s.Item2 != null).ToString());
                    GUI.color = Color.white;
                    TooltipHandler.TipRegion(x, s.Item2?.Description);
                    if (Mouse.IsOver(x))
                        Widgets.DrawHighlight(x);
                };
            }
        }

        private IEnumerable<Action<Rect>> SocialSettingsHeader()
        {
            IEnumerable<ReligionSettings> sSettings = religion.AllSettings.Where(g => g is ReligionSettings_Social);
            if (sSettings == null)
            {
                yield return x => Widgets.Label(x, "Religion_NoSuchSettings".Translate());
            }
            else
            {
                yield return z =>
                {
                    Template(z, 1, 7, new Action<Rect>[7]
                    {
                                u =>
                                {
                                    Widgets.Label(u, "Religion_Cause".Translate());
                                    TooltipHandler.TipRegion(u, "Religion_CauseDesc".Translate());
                                    if (Mouse.IsOver(u))
                                        Widgets.DrawHighlight(u);
                                },
                                u =>
                                {
                                    Widgets.Label(u, "Religion_Settings".Translate());
                                    TooltipHandler.TipRegion(u, "Religion_SettingsDesc".Translate());
                                    if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                                },
                                u =>
                                {
                                    Widgets.Label(u, "Religion_Category".Translate());
                                    TooltipHandler.TipRegion(u, "Religion_PawnCategory".Translate());
                                    if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                                },
                                u =>
                                {
                                    Widgets.Label(u, "Religion_SubjectPiety".Translate());
                                    TooltipHandler.TipRegion(u, "Religion_SubjectPietyDesc".Translate());
                                    if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                                },
                                u =>
                                {
                                    Widgets.Label(u, "Religion_SubjectThought".Translate());
                                    TooltipHandler.TipRegion(u, "Religion_SubjectThoughtDesc".Translate());
                                    if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                                },
                                u =>
                                {
                                    Widgets.Label(u, "Religion_WitnessPiety".Translate());
                                    TooltipHandler.TipRegion(u, "Religion_WitnessPietyDesc".Translate());
                                    if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                                },
                                u =>
                                {
                                    Widgets.Label(u, "Religion_WitnessThought".Translate());
                                    TooltipHandler.TipRegion(u, "Religion_WitnessThoughtDesc".Translate());
                                    if (Mouse.IsOver(u))
                                    Widgets.DrawHighlight(u);
                                },
                    }, height);
                    Rect outR = new Rect(z.x, z.y + 50, z.width, z.height);
                    var settings = SocialSettings().Concat(ActivitySettings());
                    var localHeight = settings.Count() * doubleHeight;
                    Rect viewR = new Rect(z.x, z.y + 50, z.width - 16, localHeight);
                    Widgets.BeginScrollView(outR, ref scrollPosition, viewR, true);
                    Template(viewR, settings.Count(), 1, settings, doubleHeight);
                    Widgets.EndScrollView();
                };
            }
        }

        private IEnumerable<Action<Rect>> SocialSettings()
        {
            foreach (ReligionSettings_Social s in religion.AllSettings.Where(g => g is ReligionSettings_Social))
            {
                if (s.DefaultPropety != null)
                {
                    yield return z =>
                    {
                        Template(z, 1, 7, new Action<Rect>[7]
                        {
                                u => Widgets.Label(u, "Default".Translate()),
                                u =>
                                {
                                    Widgets.Label(u, s.Tag.LabelCap);
                                    if(Mouse.IsOver(u))
                                        TooltipHandler.TipRegion(u, s.Tag.description);
                                },
                                u =>
                                {
                                    GUI.color = Color.gray;
                                    Widgets.Label(u, s.DefaultPropety.PawnCategory.Label());
                                    GUI.color = Color.white;
                                    TooltipHandler.TipRegion(u, (TipSignal)"Religion_PawnCategory".Translate());
                                    if (Mouse.IsOver(u))
                                        Widgets.DrawHighlight(u);
                                },
                                u => ColoredRange(u, s.DefaultPropety.Subject?.Piety),
                                u => ColoredRange(u, s.DefaultPropety.Subject?.Thought),
                                u => ColoredRange(u, s.DefaultPropety.Witness?.Piety),
                                u => ColoredRange(u, s.DefaultPropety.Witness?.Thought),
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
                                u =>
                                {
                                    Widgets.Label(u, s.Tag.LabelCap);
                                    if(Mouse.IsOver(u))
                                        TooltipHandler.TipRegion(u, s.Tag.description);
                                },
                                u =>
                                {
                                    GUI.color = Color.gray;
                                    Widgets.Label(u, p.PawnCategory.Label());
                                    GUI.color = Color.white;
                                    TooltipHandler.TipRegion(u, (TipSignal)"Religion_PawnCategory".Translate());
                                    if (Mouse.IsOver(u))
                                        Widgets.DrawHighlight(u);
                                },
                                u => ColoredRange(u, p.Subject?.Piety),
                                u => ColoredRange(u, p.Subject?.Thought),
                                u => ColoredRange(u, p.Witness?.Piety),
                                u => ColoredRange(u, p.Witness?.Thought),
                        });
                    };
                }
            }
        }

        private IEnumerable<Action<Rect>> ActivitySettings()
        {
            ReligionSettings_ReligionActivity aSettings = religion.GetSettings<ReligionSettings_ReligionActivity>(SettingsTagDefOf.ActivityTag);
            if (aSettings != null)
            {
                foreach (var p in aSettings.Properties)
                {
                    yield return z =>
                    {
                        Template(z, 1, 7, new Action<Rect>[7]
                        {
                                u => Widgets.Label(u, p.GetObject().LabelCap),
                                u =>
                                {
                                    Widgets.Label(u, aSettings.Tag.LabelCap);
                                    if(Mouse.IsOver(u))
                                        TooltipHandler.TipRegion(u, aSettings.Tag.description);
                                },
                                u =>
                                {
                                    GUI.color = Color.gray;
                                    Widgets.Label(u, p.PawnCategory.Label());
                                    GUI.color = Color.white;
                                    TooltipHandler.TipRegion(u, (TipSignal)"Religion_PawnCategory".Translate());
                                    if (Mouse.IsOver(u))
                                        Widgets.DrawHighlight(u);
                                },
                                u => ColoredRange(u, p.Subject?.Piety),
                                u => ColoredRange(u, p.Subject?.Thought),
                                u => ColoredRange(u, p.Witness?.Piety),
                                u => ColoredRange(u, p.Witness?.Thought),
                        });
                    };
                }
            }
        }

        private void ColoredRange(Rect r, PietyDef def)
        {
            if (def != null)
            {
                string l = GetRange(def, out Color color);
                GUI.color = color;
                Widgets.Label(r, l);
                GUI.color = Color.white;
            }
            else
                Widgets.Label(r, "-");
        }

        private void ColoredRange(Rect r, ThoughtDef def)
        {
            if (def != null)
            {
                string l = GetRange(def, out Color color);
                GUI.color = color;
                Widgets.Label(r, l);
                GUI.color = Color.white;
            }
            else
                Widgets.Label(r, "-");
        }

        private void ColoredOpinionRange(Rect r, ThoughtDef def)
        {
            if (def != null)
            {
                string l = GetOpinionRange(def, out Color color);
                GUI.color = color;
                Widgets.Label(r, l);
                GUI.color = Color.white;
            }
            else
                Widgets.Label(r, "-");
        }
    }
}
