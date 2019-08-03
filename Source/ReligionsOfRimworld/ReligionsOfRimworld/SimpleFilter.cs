using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class SimpleFilter
    {
        private HashSet<ThingDef> defaultThings;
        private HashSet<ThingDef> allowedThings;

        public SimpleFilter(IEnumerable<ThingDef> defs)
        {
            defaultThings = new HashSet<ThingDef>();
            allowedThings = new HashSet<ThingDef>();

            foreach (ThingDef def in defs)
                defaultThings.Add(def);

            AllowAll();
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
    }
}
