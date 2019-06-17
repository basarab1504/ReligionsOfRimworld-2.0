using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ReligionsOfRimworld
{
    class MainTabWindow_Religions : MainTabWindow
    {
        private Vector2 scrollPosition = Vector2.zero;
        private float scrollViewHeight;

        public override void DoWindowContents(Rect fillRect)
        {
            base.DoWindowContents(fillRect);
            ReligionsTabUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight);
        }
    }
}
