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

        public List<PietyStage> Stages
        {
            get
            {
                return stages;
            }
        }

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

        public bool IsSituational
        {
            get
            {
                return this.Worker != null;
            }
        }

        public float DurationDays
        {
            get
            {
                return durationDays;
            }
        }

        public int DurationTicks
        {
            get
            {
                return (int)((double)this.durationDays * 60000.0);
            }
        }
    }
}
