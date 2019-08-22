using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class SimpleFilter : IExposable
    {
        private HashSet<ThingDef> defaultThings;
        private HashSet<ThingDef> allowedThings;

        public SimpleFilter(IEnumerable<ThingDef> defs)
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            {
                defaultThings = new HashSet<ThingDef>();
                allowedThings = new HashSet<ThingDef>();

                foreach (ThingDef def in defs)
                    defaultThings.Add(def);

                AllowAll();
            }
        }

        public IEnumerator<ThingDef> GetEnumerator()
        {
            return allowedThings.GetEnumerator();
        }

        public IEnumerable<ThingDef> AvaliableThings => defaultThings;
        public int Count => allowedThings.Count;

        public void DisallowAll()
        {
            allowedThings.Clear();
        }

        public void AllowAll()
        {
            allowedThings.Clear();
            allowedThings.AddRange(defaultThings);
        }

        public void SetAllowance(ThingDef def, bool allowance)
        {
            if (allowance)
                allowedThings.Add(def);
            else
                allowedThings.Remove(def);
        }

        public bool Allows(ThingDef def)
        {
            return allowedThings.Contains(def);
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<ThingDef>(ref this.defaultThings, "defaultThings", LookMode.Def);
            Scribe_Collections.Look<ThingDef>(ref this.allowedThings, "allowedThings", LookMode.Def);
        }
    }
}
