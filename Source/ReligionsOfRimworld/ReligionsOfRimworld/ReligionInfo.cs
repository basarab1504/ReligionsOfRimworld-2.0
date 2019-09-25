using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReligionsOfRimworld
{
    public class ReligionInfo: IDescribable
    {
        public string Label { get; }
        private List<ReligionInfoEntry> infoEntries;

        public ReligionInfo(string label)
        {
            this.Label = label;
            infoEntries = new List<ReligionInfoEntry>();
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
