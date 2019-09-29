using RimWorld;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    class ITab_Pawn_Religion : ITab
    {
        public ITab_Pawn_Religion()
        {
            this.size = new Vector2(500f, 400f);
            this.labelKey = "TabReligion";
            this.tutorTag = "ReligionTutorTag";
        }

        protected override void FillTab()
        {
            ReligionCardUtility.DrawReligionCard(new Rect(0.0f, 0.0f, this.size.x, this.size.y), PawnToShowInfoAbout);
        }

        public override bool IsVisible
        {
            get
            {
                return !PawnToShowInfoAbout.Dead;
            }
        }

        private Pawn PawnToShowInfoAbout
        {
            get
            {
                Pawn pawn = (Pawn)null;
                if (this.SelPawn != null)
                {
                    pawn = this.SelPawn;
                }
                else
                {
                    if (this.SelThing is Corpse selThing)
                        pawn = selThing.InnerPawn;
                }
                if (pawn != null)
                    return pawn;
                Log.Error("Religion tab found no selected pawn to display.", false);
                return (Pawn)null;
            }
        }
    }
}
