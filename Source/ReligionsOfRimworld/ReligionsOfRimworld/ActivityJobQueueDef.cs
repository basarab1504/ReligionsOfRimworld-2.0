using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ActivityJobQueueDef : Def
    {
        private List<ActivityJobNode> activityNodes = new List<ActivityJobNode>();

        public IEnumerable<ActivityJobNode> ActivityNodes => activityNodes;
    }
}
