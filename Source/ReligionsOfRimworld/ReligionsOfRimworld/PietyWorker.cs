using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public abstract class PietyWorker
    {
        public PietyDef def;

        public abstract PietyState CurrentState(Pawn p);
    }
}
