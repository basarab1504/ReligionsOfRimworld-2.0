using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class PietyDef : Def
    {
        private List<PietyStage> stages = new List<PietyStage>();
        private System.Type workerClass;
        private PietyWorker workerInt;
        private float durationDays;

        public IEnumerable<PietyStage> Stages => stages;
        public bool IsSituational => this.Worker != null;
        public float DurationDays => durationDays;
        public int DurationTicks => (int)((double)this.durationDays * 60000.0);

        public PietyWorker Worker
        {
            get
            {
                if (this.workerInt == null && this.workerClass != null)
                {
                    this.workerInt = (PietyWorker)Activator.CreateInstance(this.workerClass);
                    this.workerInt.def = this;
                }
                return this.workerInt;
            }
        }
    }
}
