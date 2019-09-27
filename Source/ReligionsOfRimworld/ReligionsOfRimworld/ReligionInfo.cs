using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionInfoCategory
    {
        public string Label { get; }
        private ReligionInfoEntry descriptionEntry;
        private List<ReligionInfoEntry> infoEntries;

        public ReligionInfoCategory(string label, string description)
        {
            this.Label = label;
            descriptionEntry = new ReligionInfoEntry("ReligionInfo_Description".Translate(), "", description);
            infoEntries = new List<ReligionInfoEntry>();
        }

        public ReligionInfoCategory(string label, string description, IEnumerable<ReligionInfoEntry> entries)
        {
            this.Label = label;
            descriptionEntry = new ReligionInfoEntry("ReligionInfo_Description".Translate(), "", description);
            infoEntries = new List<ReligionInfoEntry>();
            infoEntries.AddRange(entries);
        }

        public IEnumerable<ReligionInfoEntry> GetInfoEntries() => infoEntries;

        public void Add(ReligionInfoEntry religionInfoEntry)
        {
            infoEntries.Add(religionInfoEntry);
        }

        public void AddRange(IEnumerable<ReligionInfoEntry> religionInfoEntries)
        {
            infoEntries.AddRange(religionInfoEntries);
        }
    }
}
