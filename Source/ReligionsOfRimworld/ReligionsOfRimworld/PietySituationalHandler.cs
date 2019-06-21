using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class PietySituationalHandler
    {
        private List<Piety_Situational> cachedPiety = new List<Piety_Situational>();
        private HashSet<PietyDef> tmpCachedPiety = new HashSet<PietyDef>();
        private int lastPietyRecalculationTick = -99999;
        Pawn pawn;

        public PietySituationalHandler(Pawn p)
        {
            pawn = p;
        }

        //public float Value
        //{
        //    get
        //    {
        //        float v = 1f;
        //        foreach (Piety_Situational m in cachedMultipliers)
        //            if (m.Active)
        //                v *= m.Value;
        //        return v;
        //    }
        //}

        //public List<Piety_Situational> Multipliers
        //{
        //    get
        //    {
        //        return (from t in cachedMultipliers where t.Active select t).ToList();
        //    }
        //}

        public void Interval()
        {
            CheckRecalculate();
        }

        private void CheckRecalculate()
        {
            int ticksGame = Find.TickManager.TicksGame;
            if (ticksGame - this.lastPietyRecalculationTick < 100)
                return;
            this.lastPietyRecalculationTick = ticksGame;
            try
            {
                this.tmpCachedPiety.Clear();
                for (int index = 0; index < this.cachedPiety.Count; ++index)
                {
                    this.cachedPiety[index].RecalculateState();
                    this.tmpCachedPiety.Add(this.cachedPiety[index].Def);
                }
                List<PietyDef> situationalPietyList = PietyUtility.situationalPietyList;
                int index1 = 0;
                for (int count = situationalPietyList.Count; index1 < count; ++index1)
                {
                    if (!this.tmpCachedPiety.Contains(situationalPietyList[index1]))
                    {
                        if (situationalPietyList[index1].Worker.CurrentState(pawn).ActiveFor(situationalPietyList[index1]))
                        {
                            Piety_Situational pietyMultiplier = TryCreatePiety(situationalPietyList[index1]);
                            if (pietyMultiplier != null)
                                this.cachedPiety.Add(pietyMultiplier);
                        }
                    }
                }
            }
            finally
            {
            }
        }

        private Piety_Situational TryCreatePiety(PietyDef def)
        {
            if (!def.Worker.CurrentState(this.pawn).ActiveFor(def))
                return (Piety_Situational)null;
            Piety_Situational piety = new Piety_Situational(pawn, def);
            piety.RecalculateState();
            return piety;
        }
    }
}
