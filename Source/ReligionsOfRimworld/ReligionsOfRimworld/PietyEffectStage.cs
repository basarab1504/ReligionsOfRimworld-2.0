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
        private bool visible = true;

        public string Label => label;
        public string Description => description;
        public float PietyOffset => pietyOffset;
        public bool Visible => visible;
    }
}
