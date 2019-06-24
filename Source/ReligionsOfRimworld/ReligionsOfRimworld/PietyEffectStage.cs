using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReligionsOfRimworld
{
    public class PietyStage
    {
        private string label;
        private string description;
        private float pietyOffset = 0f;
        private float multiplierValue = 1f;
        private float pietyRate = 0;
        private bool visible = true;

        public string Label => label;
        public string Description => description;
        public float PietyOffset => pietyOffset;
        public float MultiplierValue => multiplierValue;
        public float PietyRate => pietyRate;
        public bool Visible => visible;
    }
}
