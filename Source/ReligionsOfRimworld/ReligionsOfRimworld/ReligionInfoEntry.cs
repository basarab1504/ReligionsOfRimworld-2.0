using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ReligionsOfRimworld
{
    public class Offset
    {
        private float min;
        private float max;

        public Offset(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public override string ToString()
        {
            return $"{min}...{max}";
        }
        public Color Color => min > 0 ? Color.green : Color.red;
    }

    public class ReligionInfoEntry
    {
        public string Label { get; }
        public string Value { get; }
        public string Explanation { get; }
        public Offset SubjectOffset { get; }
        public Offset WitnessOffset { get; }

        public ReligionInfoEntry(string label, string value = "", string explanation = "", Offset subjectOffset = null, Offset witnessOffset = null)
        {
            this.Label = label;
            this.Explanation = explanation;
            this.Value = value;
            this.SubjectOffset = subjectOffset;
            this.WitnessOffset = witnessOffset;
        }
    }
}
