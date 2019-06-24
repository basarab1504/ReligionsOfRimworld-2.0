using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ReligionsOfRimworld
{
    public class PietyState
    {
        private int stageIndex;
        private string reason;
        private const int InactiveIndex = -99999;

        private PietyState(int stageIndex, string reason)
        {
            this.stageIndex = stageIndex;
            this.reason = reason;
        }

        public bool Active
        {
            get
            {
                return this.stageIndex != -99999;
            }
        }

        public int StageIndex
        {
            get
            {
                return this.stageIndex;
            }
        }

        public string Reason
        {
            get
            {
                return this.reason;
            }
        }

        public static PietyState Inactive
        {
            get
            {
                return ActiveAtStage(-99999);
            }
        }

        public static PietyState ActiveDefault
        {
            get
            {
                return ActiveAtStage(0);
            }
        }

        public static PietyState ActiveAtStage(int stageIndex, string reason = "")
        {
            return new PietyState(stageIndex, reason);
        }

        public static implicit operator PietyState(bool value)
        {
            if (value)
                return PietyState.ActiveDefault;
            return PietyState.Inactive;
        }

        public bool ActiveFor(PietyDef def)
        {
            if (!this.Active)
                return false;
            int index = this.StageIndexFor(def);
            if (index >= 0)
                return def.Stages.ElementAt(index) != null;
            return false;
        }

        public int StageIndexFor(PietyDef def)
        {
            return Mathf.Min(this.StageIndex, def.Stages.Count() - 1);
        }
    }
}
