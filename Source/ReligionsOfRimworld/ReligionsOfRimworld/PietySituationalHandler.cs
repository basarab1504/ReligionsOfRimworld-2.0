using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class PietySituationalHandler
    {
        private Pawn pawn;
        private List<Piety_Situational> cachedPiety;
        private HashSet<PietyDef> tmpCachedPiety;
        private int lastPietyRecalculationTick = -99999;

        public PietySituationalHandler(Pawn p)
        {
            cachedPiety = new List<Piety_Situational>();
            tmpCachedPiety = new HashSet<PietyDef>();
            pawn = p;
        }

        public float TotalSitationalMultiplierValue
        {
            get
            {
                float v = 1f;
                foreach (Piety_Situational m in cachedPiety)
                    if (m.Active)
                        v *= m.MultiplierValue;
                return v;
            }
        }

        public IEnumerable<Piety_Situational> Piety => from t in cachedPiety where t.Active select t;

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
