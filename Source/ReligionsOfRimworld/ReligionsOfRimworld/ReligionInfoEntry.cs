using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReligionsOfRimworld
{
    public class ReligionInfoEntry
    {
        public string Label { get; }
        public string Value { get; }
        public string Explanation { get; }

        public ReligionInfoEntry(string label, string value = "", string explanation = "")
        {
            this.Label = label;
            this.Explanation = explanation;
            this.Value = value;
        }
    }
}
